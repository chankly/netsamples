using Azure;
using Azure.Storage.Blobs;
using Polly;

namespace HSoft.NetSamples.Api.Infrastructure.Network.Downloads
{
    public class DownloadManager
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly HttpClient _httpClient;
        private readonly int _chunkSize = 1024 * 1024 * 1; // 1 MB

        public DownloadManager(BlobServiceClient blobServiceClient, IHttpClientFactory httpClientFactory)
        {
            _blobServiceClient = blobServiceClient;
            _httpClient = httpClientFactory.CreateClient("DownloadClient");
        }

        public async Task<DownloadChunkResult> DownloadLargeBlobAsync(string containerName, string blobName, string destinationFile)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (!Directory.Exists(Path.GetDirectoryName(destinationFile)))
            {
                return new DownloadChunkResult { IsSucess = false, Message = "Destination directory doesn't exists." };
            }

            if (!await blobClient.ExistsAsync())
            {
                return new DownloadChunkResult { IsSucess = false, Message = "File doesn't exists." };
            }

            var blobProperties = await blobClient.GetPropertiesAsync();
            long blobSize = blobProperties.Value.ContentLength;

            long bytesDownloaded = 0;
            if (File.Exists(destinationFile))
            {
                bytesDownloaded = new FileInfo(destinationFile).Length;
            }

            var retryPolicy = Policy
                            .Handle<Exception>()
                            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

            return await
            retryPolicy.ExecuteAsync(async () =>
            {
                while (bytesDownloaded < blobSize)
                {
                    // Get range to download.
                    long rangeStart = bytesDownloaded;
                    long rangeEnd = Math.Min(rangeStart + _chunkSize - 1, blobSize - 1);

                    try
                    {
                        // Download chunk
                        HttpRange range = new HttpRange(rangeStart, rangeEnd - rangeStart + 1);
                        var response = await blobClient.DownloadAsync(range);

                        // Write chunk
                        using (var fileStream = new FileStream(destinationFile, FileMode.Append, FileAccess.Write))
                        {
                            await response.Value.Content.CopyToAsync(fileStream);
                        }

                        // Update downloaded bytes
                        bytesDownloaded = rangeEnd + 1;
                    }
                    catch (Exception ex)
                    {
                        return new DownloadChunkResult { IsSucess = false, Chunk = rangeStart, Message = ex.Message };
                    }
                }

                return new DownloadChunkResult { IsSucess = true };
            });
        }

        public async Task<DownloadChunkResult> DownloadLargeFileAsync(string sourceUrl, string destinationFile)
        {
            try
            {
                long bytesDownloaded = 0;
                if (File.Exists(destinationFile))
                {
                    FileInfo fileInfo = new FileInfo(destinationFile);
                    bytesDownloaded = fileInfo.Length;
                }

                long totalFileSize = await GetFileSizeAsync(sourceUrl);


                while (bytesDownloaded < totalFileSize)
                {
                    var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

                    await retryPolicy.ExecuteAsync(async () =>
                    {
                        // Get range to download.
                        long rangeStart = bytesDownloaded;
                        long rangeEnd = Math.Min(rangeStart + _chunkSize - 1, totalFileSize - 1);

                        // Download chunk
                        var request = new HttpRequestMessage(HttpMethod.Get, sourceUrl);
                        request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(rangeStart, rangeEnd);

                        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Error al descargar el archivo: {response.StatusCode}");
                        }

                        // Write chunk on file
                        using (var fileStream = new FileStream(destinationFile, FileMode.Append, FileAccess.Write))
                        using (var httpStream = await response.Content.ReadAsStreamAsync())
                        {
                            await httpStream.CopyToAsync(fileStream);
                        }

                        // Update downloaded bytes
                        bytesDownloaded = rangeEnd + 1;

                        // Muestra el progreso
                        double progressPercentage = (double)bytesDownloaded / totalFileSize * 100;
                        Console.WriteLine($"Progress: {progressPercentage:F2}%");
                    });
                }

                Console.WriteLine("Descarga completada correctamente.");

                return new DownloadChunkResult { IsSucess = true };
            }
            catch (Exception ex)
            {
                // Si ocurre un error después de los reintentos
                Console.WriteLine($"Error durante la descarga: {ex.Message}");
                return new DownloadChunkResult { IsSucess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Get file size from URL
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<long> GetFileSizeAsync(string fileUrl)
        {
            var headResponse = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, fileUrl));
            if (!headResponse.IsSuccessStatusCode)
            {
                throw new Exception("No se pudo obtener el tamaño del archivo.");
            }

            // Obtén el tamaño del archivo desde la cabecera Content-Length
            long totalFileSize = headResponse.Content.Headers.ContentLength ?? throw new Exception("No se pudo obtener el tamaño del archivo.");

            return totalFileSize;
        }
    }
}
