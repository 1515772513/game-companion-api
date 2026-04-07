namespace GameCompanion.Api.DTO.File
{
    public class FileDownloadRespDto
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
        public string FileName { get; set; } = string.Empty;
        public bool Success => FileBytes.Length > 0;
    }
}