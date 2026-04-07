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
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;
        
        public FileService(
            IWebHostEnvironment webHostEnv,
            ApplicationDbContext dbContext,
            IFileService fileService)
        {
            _webHostEnv = webHostEnv;
            _dbContext = dbContext;
            _fileService = fileService;
        }

        public async Task<FileUploadRespDto> UploadFileAsync(IFormFile file, string module, long? userId = null)
        {
            if (file == null || file.Length == 0)
                throw new Exception("上传文件不能为空");

            // 路径
            var root = _webHostEnv.WebRootPath;
            var moduleDir = Path.Combine("uploads", module.ToLower());
            var saveDir = Path.Combine(root, moduleDir);

            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // 文件信息
            var ext = Path.GetExtension(file.FileName).Trim('.').ToLower();
            var uuidFileName = $"{Guid.NewGuid():N}.{ext}";
            var relativePath = Path.Combine(moduleDir, uuidFileName);
            var fullPath = Path.Combine(root, relativePath);
            var fileUrl = $"/{relativePath.Replace("\\", "/")}";

            // 保存文件
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 入库
            var entity = new SysFile
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                FileUrl = fileUrl,
                FilePath = fullPath,
                FileSize = file.Length,
                FileExt = ext,
                ContentType = file.ContentType,
                UploadUser = userId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            _dbContext.SysFiles.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new FileUploadRespDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                FileUrl = entity.FileUrl,
                FileSize = entity.FileSize,
                FileExt = entity.FileExt,
                ContentType = entity.ContentType
            };
        }

        public async Task<FileDownloadRespDto> DownloadFileAsync(string fileUrl)
        {
            var file = await _dbContext.SysFiles
                .FirstOrDefaultAsync(f => f.FileUrl == fileUrl && f.IsDeleted == 0);

            if (file == null || !System.IO.File.Exists(file.FilePath))
                return new FileDownloadRespDto();

            var bytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);

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

                // 逻辑删除
                file.IsDeleted = 1;
                file.UpdateTime = DateTime.Now;

                // 物理删除
                if (System.IO.File.Exists(file.FilePath))
                    System.IO.File.Delete(file.FilePath);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}