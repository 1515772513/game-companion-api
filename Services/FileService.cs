using System.Net.Http;
using System.Security.Claims;
using GameCompanion.Api.Data;
using GameCompanion.Api.DTO.File;
using GameCompanion.Api.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCompanion.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly GameCompanionContext _dbContext;
        private readonly ILogger<FileService> _logger;
        private readonly string _wwwRootPath;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(
            IWebHostEnvironment webHostEnv,
            GameCompanionContext dbContext,
            ILogger<FileService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnv = webHostEnv;
            _dbContext = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            // 🔥 自动获取 wwwroot 路径，彻底避免 null
            _wwwRootPath = Path.Combine(_webHostEnv.ContentRootPath, "wwwroot");
            if (!Directory.Exists(_wwwRootPath))
                Directory.CreateDirectory(_wwwRootPath);
        }

        public async Task<FileUploadRespDto> UploadFileAsync(IFormFile file, string module)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("上传文件不能为空");

            // 模块默认值兜底
            module = string.IsNullOrWhiteSpace(module) ? "common" : module.ToLower();

            long? userId = null;
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User.Identity?.IsAuthenticated == true)
            {
                var userIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                long.TryParse(userIdStr, out long uid);
                userId = uid;
            }

            var moduleFolder = Path.Combine("uploads", module);
            var saveFolder = Path.Combine(_wwwRootPath, moduleFolder);
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            var ext = Path.GetExtension(file.FileName).Trim('.').ToLower();
            var fileName = $"{Guid.NewGuid():N}{(string.IsNullOrEmpty(ext) ? "" : $".{ext}")}";
            var relativePath = Path.Combine(moduleFolder, fileName);
            var fullPath = Path.Combine(_wwwRootPath, relativePath);
            var fileUrl = $"/{relativePath.Replace("\\", "/")}";

            // 🔥 修复：正确枚举 FileMode.Create
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            
            var req = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{req.Scheme}://{req.Host.Value}";
            var entity = new SysFile
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                FileUrl = $"{baseUrl}{fileUrl}",
                FilePath = fullPath,
                FileSize = file.Length,
                FileExt = ext,
                ContentType = file.ContentType,
                UploadUser = userId,
                UploadPlatform = "Web",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };

            _dbContext.SysFiles.Add(entity);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation($"上传文件成功：{_httpContextAccessor.HttpContext.Request.PathBase.Value}");
            return new FileUploadRespDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                FileUrl = $"{baseUrl}{entity.FileUrl}",
                FileSize = entity.FileSize,
                FileExt = entity.FileExt ?? "",
                ContentType = entity.ContentType ?? ""
            };
        }

        public async Task<FileDownloadRespDto> DownloadFileAsync(string fileUrl)
        {
            var file = await _dbContext.SysFiles
                .FirstOrDefaultAsync(f => f.FileUrl == fileUrl && f.IsDeleted == 0);

            if (file == null || !File.Exists(file.FilePath))
                return new FileDownloadRespDto();

            var bytes = await File.ReadAllBytesAsync(file.FilePath);
            return new FileDownloadRespDto
            {
                FileBytes = bytes,
                ContentType = file.ContentType ?? "application/octet-stream",
                FileName = file.FileName
            };
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                var file = await _dbContext.SysFiles
                    .FirstOrDefaultAsync(f => f.FileUrl == fileUrl && f.IsDeleted == 0);

                if (file == null) return false;

                file.IsDeleted = 1;
                file.UpdateTime = DateTime.Now;

                if (File.Exists(file.FilePath))
                    File.Delete(file.FilePath);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除文件失败：{fileUrl}", fileUrl);
                return false;
            }
        }
    }
}