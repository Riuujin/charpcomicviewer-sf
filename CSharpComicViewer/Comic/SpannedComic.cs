using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpComicViewer.Comic
{
    internal class SpannedComic : IComic
    {
        private List<IComic> comics;

        public SpannedComic(string[] filePaths)
        {
            if (filePaths == null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            if (filePaths.Length == 0)
            {
                throw new ArgumentException("files must contain at least 1 file path", nameof(filePaths));
            }

            comics = new List<IComic>();

            foreach (var file in filePaths)
            {
                var comic = ComicFactory.Create(file);
                if (comic != null)
                {
                    comics.Add(comic);
                }
            }
        }

        public string getBookmarkName(int pageNumber)
        {
            int count = 0;

            foreach (var comic in comics)
            {
                int pages = count + comic.Pages();

                if (pageNumber <= pages)
                {
                    return comic.getBookmarkName(pageNumber - count);
                }
                count = pages;
            }

            return null;
        }

        public string[] GetFilePaths()
        {
            List<string> filePaths = new List<string>();
            comics.ForEach(c => filePaths.AddRange(c.GetFilePaths()));
            return filePaths.ToArray();
        }

        public Task<string> GetInformationText()
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetPage(int pageNumber)
        {
            int count = 0;

            foreach (var comic in comics)
            {
                int pages = count + comic.Pages();

                if (pageNumber <= pages)
                {
                    return await comic.GetPage(pageNumber - count);
                }
                count = pages;
            }

            return null;
        }

        public int Pages()
        {
            int count = 0;

            foreach (var comic in comics)
            {
                count += comic.Pages();
            }

            return count;
        }
    }
}