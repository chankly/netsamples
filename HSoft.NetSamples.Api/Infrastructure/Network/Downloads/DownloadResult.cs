namespace HSoft.NetSamples.Api.Infrastructure.Network.Downloads
{
    public class DownloadResult
    {
        public bool IsSucess { get; set; }
        public string? Message { get; set; }
    }

    public class DownloadChunkResult : DownloadResult
    {
        public long Chunk { get; set; }
    }
}
