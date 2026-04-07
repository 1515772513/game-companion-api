namespace GameCompanion.Api.DTO.File
{
    public class FileUploadRespDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FileExt { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}