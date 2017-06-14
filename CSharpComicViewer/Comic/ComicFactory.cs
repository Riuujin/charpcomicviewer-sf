using CSharpComicViewer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace CSharpComicViewer.Comic
{
    public static class ComicFactory
    {
        public static IComic Create(string[] filePaths)
        {
            if (filePaths.Length == 1)
            {
                return Create(filePaths[0]);
            }
            else
            {
                foreach (var filePath in filePaths)
                {
                    if (!SupportedExtensions.IsSupportedArchive(filePath)
                        && !SupportedExtensions.IsSupportedImage(filePath))
                    {
                        return null;
                    }
                }

                return new SpannedComic(filePaths);
            }
        }

        public static IComic Create(string filePath)
        {
            if (SupportedExtensions.IsSupportedArchive(filePath))
            {
                return new ArchiveComic(filePath);
            }
            else if (SupportedExtensions.IsSupportedImage(filePath))
            {
                return new ImageComic(filePath);
            }

            return null;
        }
    }
}