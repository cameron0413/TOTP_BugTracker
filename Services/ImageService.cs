using TOTP_BugTracker.Services.Interfaces;

namespace TOTP.Services
{
    public class ImageService : IImageService
    {
        private readonly string _defaultUserImageSrc = "/img/DefaultAvatarImage.jpg";

        private readonly string _defaultCompanyImageSrc = "/img/DefaultCompanyImage.png";

        private readonly string _defaultProjectImageSrc = "/img/DefaultProjectImage.png";


        public string? ConvertByteArrayToFile(byte[] fileData, string? extension, int imageType)
        {
            if (fileData == null || fileData.Length == 0)
            {
                switch (imageType)
                {
                    //user - 1
                    //post - 2
                    //category - 3

                    case 1: return _defaultUserImageSrc;
                    case 2: return _defaultCompanyImageSrc;
                    case 3: return _defaultProjectImageSrc;
                }
            }
            try
            {
                string? imageBase64Data = Convert.ToBase64String(fileData!);
                return string.Format($"data:{extension};base64, {imageBase64Data}");
                //^^^^^ Interpolated code
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            try
            {
                using MemoryStream memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] byteArray = memoryStream.ToArray();
                return byteArray;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

