﻿namespace TOTP_BugTracker.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        public string? ConvertByteArrayToFile(byte[] fileData, string? extension, int imageType);
    }
}
