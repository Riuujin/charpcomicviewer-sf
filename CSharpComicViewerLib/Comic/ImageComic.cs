﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace CSharpComicViewerLib.Comic
{
    internal class ImageComic : IComic
    {
        private string filePath;

        public ImageComic(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File not found: {filePath}", nameof(filePath));
            }

            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public string getBookmarkName(int pageNumber)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public string[] GetFilePaths()
        {
            return new string[] { filePath };
        }

        public Task<string> GetInformationText()
        {
            return null;
        }

        public async Task<byte[]> GetPage(int pageNumber)
        {
            if (!File.Exists(this.filePath))
            {
                return null;
            }

            byte[] data;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await fs.CopyToAsync(ms);
                    data = ms.ToArray();
                }
            }

            return data;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<int> GetNumberOfPages()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!File.Exists(this.filePath))
            {
                return 0;
            }

            return 1;
        }
    }
}