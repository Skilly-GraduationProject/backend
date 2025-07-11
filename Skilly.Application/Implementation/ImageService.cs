﻿using Microsoft.AspNetCore.Http;
using Skilly.Application.Abstract;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skilly.Application.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

          
            if (filePath.StartsWith("/"))
                filePath = filePath.Substring(1);
           
            var relativePath = filePath.Replace("https://skilly.runasp.net/", "").Replace("http://skilly.runasp.net/", "");

            
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

        }


        public async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            string extension = Path.GetExtension(file.FileName).ToLower();
            string fullFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }

            var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".mp4", ".mov", ".avi", ".mkv", ".m4v" };

            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                throw new InvalidOperationException("Only image, video, or PDF files are allowed.");
            }

            string uniqueFileName = Guid.NewGuid().ToString() + extension;

            string filePath = Path.Combine(fullFolderPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            string baseUrl = "https://skilly.runasp.net/";
            string fileUrl = $"{baseUrl}{folderPath.Replace("\\", "/")}{uniqueFileName}";

            return fileUrl;
        }





    }
}