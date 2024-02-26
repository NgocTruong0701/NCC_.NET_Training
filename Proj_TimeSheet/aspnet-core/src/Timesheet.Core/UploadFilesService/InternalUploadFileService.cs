using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Ncc.Authorization.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Constants;
using Timesheet.Helper;
using Timesheet.Uitls;

namespace Timesheet.UploadFilesService
{
    public class InternalUploadFileService : IUploadFileService
    {
        private readonly string WWWRootFolder = "wwwroot";

        private void CheckValidFile(IFormFile file, string[] allowFileTypes)
        {
            var fileExt = FileUtils.GetFileExtension(file);
            if (!allowFileTypes.Contains(fileExt))
                throw new UserFriendlyException($"Wrong file type {file.ContentType}. Allow file types: {string.Join(", ", allowFileTypes)}");
        }

        public async Task<string> UploadAvatarAsync(IFormFile file, string tenantName)
        {
            CheckValidFile(file, ConstantUploadFile.AllowImageFileTypes);
            var avatarFolder = Path.Combine(WWWRootFolder, ConstantUploadFile.AvatarFolder, tenantName);
            string fileLocation = UploadImages.CreateFolderIfNotExists(avatarFolder);

            var fileName = $"{DateTimeUtils.NowToYYYYMMddHHmmss()}_{Guid.NewGuid()}.{FileUtils.GetFileExtension(file)}";
            var filePath = $"{ConstantUploadFile.AvatarFolder?.TrimEnd('/')}/{tenantName}/{fileName}";

            await UploadImages.UploadFileAsync(fileLocation, file, fileName);

            return filePath;
        }

        public Task<string> UploadFileAsync(IFormFile file, string[] allowFileTypes, string filePath)
        {
            throw new NotImplementedException();
        }


    }
}
