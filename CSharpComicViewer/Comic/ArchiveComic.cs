﻿using CSharpComicViewer.Data;
using SharpCompress.Archives;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpComicViewer.Comic
{
    internal class ArchiveComic : IComic
    {
        private string filePath;
        private int? pages;
        private string informationText;
        private bool tryGetInformationText;

        public ArchiveComic(string filePath)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            tryGetInformationText = false;
        }

        public string getBookmarkName(int pageNumber)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public string[] GetFilePaths()
        {
            return new string[] { filePath };
        }

        public async Task<string> GetInformationText()
        {
            if (tryGetInformationText)
            {
                tryGetInformationText = false;

                using (var archive = ArchiveFactory.Open(filePath))
                {
                    var file = archive.Entries.FirstOrDefault(x => !x.IsDirectory &&
                                                                    SupportedExtensions.IsSupportedTextFile(x.Key) &&
                                                                    !x.Key.StartsWith("__MACOSX/", StringComparison.OrdinalIgnoreCase));
                    if (file != null)
                    {
                        using (var entryStream = file.OpenEntryStream())
                        {
                            using (StreamReader sr = new StreamReader(entryStream))
                            {
                                informationText = await sr.ReadToEndAsync();
                            }
                        }
                    }
                }
            }

            return informationText;
        }

        public async Task<byte[]> GetPage(int pageNumber)
        {
            byte[] data;

            using (var archive = ArchiveFactory.Open(filePath))
            {
                var files = archive.Entries.Where(x => !x.IsDirectory &&
                                                        SupportedExtensions.IsSupportedImage(x.Key) &&
                                                        !x.Key.StartsWith("__MACOSX/", StringComparison.OrdinalIgnoreCase))
                                           .OrderBy(x => x.Key)
                                           .ToList();

                var file = files[pageNumber - 1];

                using (var entryStream = file.OpenEntryStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await entryStream.CopyToAsync(ms);
                        data = ms.ToArray();
                    }
                }
            }

            return data;
        }

        public int Pages()
        {
            if (pages == null)
            {
                using (var archive = ArchiveFactory.Open(filePath))
                {
                    var files = archive.Entries.Where(x => !x.IsDirectory &&
                                                            SupportedExtensions.IsSupportedImage(x.Key) &&
                                                            !x.Key.StartsWith("__MACOSX/", StringComparison.OrdinalIgnoreCase));
                    pages = files.Count();
                }
            }

            return pages.Value;
        }
    }
}