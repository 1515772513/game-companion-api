using GameCompanion.Api.DTO.File;
using Microsoft.AspNetCore.Http;

namespace GameCompanion.Api.Services
{
    public interface IFileService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        Task<FileUploadRespDto> UploadFileAsync(IFormFile file, string module, long? userId = null);

        /// <summary>
        /// 下载文件
        /// </summary>
        Task<FileDownloadRespDto> DownloadFileAsync(string fileUrl);

        /// <summary>
        /// 删除文件（物理+数据库）
        /// </summary>
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}