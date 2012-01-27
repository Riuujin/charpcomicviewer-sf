using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpComicLoader.File
{
    public interface IFileLoader
    {
        LoadedFilesData LoadComicBook(string[] files);
    }
}
