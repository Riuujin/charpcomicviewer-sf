using System;
using System.IO;
using System.Threading.Tasks;

namespace CSharpComicViewer.Comic
{
    internal class ImageComic : IComic
    {
        private string filePath;

        public ImageComic(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            this.filePath = filePath;
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

        public int Pages()
        {
            return 1;
        }
    }
}