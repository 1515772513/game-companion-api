using GameCompanion.Api.DTO.File;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        [HttpPost("upload")]
        public async Task<ApiResponse<FileUploadRespDto>> Upload(
            IFormFile file,
            string module = "common")
        {
            var result = await _fileService.UploadFileAsync(file, module);
            return ApiResponse<FileUploadRespDto>.Success(result);
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        [HttpGet("download")]
        public async Task<IActionResult> Download(string fileUrl)
        {
            var result = await _fileService.DownloadFileAsync(fileUrl);
            if (!result.Success)
                return NotFound("文件不存在");

            return File(result.FileBytes, result.ContentType, result.FileName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        [HttpDelete("delete")]
        public async Task<ApiResponse<bool>> Delete(string fileUrl)
        {
            var res = await _fileService.DeleteFileAsync(fileUrl);
            return ApiResponse<bool>.Success(res);
        }
    }
}