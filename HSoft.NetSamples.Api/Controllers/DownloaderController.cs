using HSoft.NetSamples.Api.Infrastructure.Network.Downloads;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSoft.NetSamples.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloaderController : ControllerBase
    {
        private readonly DownloadManager _downloadManager;

        public DownloaderController(DownloadManager downloadManager)
        {
            _downloadManager = downloadManager;
        }

        [HttpGet("download-blob")]
        public async Task<IActionResult> DownloadBlob()
        {
            var file = System.IO.Path.GetTempFileName();
            var result = await _downloadManager.DownloadLargeBlobAsync("CONTAINER_NAME", "BLOB_PATH_FILE_NAME", file);
            return Ok(result);
        }

        [HttpGet("download-file")]
        public async Task<IActionResult> DownloadFile()
        {
            var file = System.IO.Path.GetTempFileName();
            var result = await _downloadManager.DownloadLargeFileAsync("https://dbeaver.io/files/dbeaver-ce-latest-x86_64-setup.exe", file);
            return Ok(result);
        }
    }
}
