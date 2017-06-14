using CSharpComicViewer.Data;
using System;

namespace CSharpComicViewer
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Utils
    {
        public static string GetFileLoaderFilter()
        {
            string returnValue = "";

            //Add all supported to filter
            returnValue += "Supported formats|";

            foreach (string extension in SupportedExtensions.SupportedArchives)
            {
                returnValue += "*." + extension + ";";
            }

            foreach (string extension in SupportedExtensions.SupportedImages)
            {
                returnValue += "*." + extension + ";";
            }

            //Add separator
            returnValue += "|";

            //Add Archives to filter
            returnValue += "Supported archive formats (";

            foreach (string extension in SupportedExtensions.SupportedArchives)
            {
                returnValue += "*." + extension + ";";
            }

            returnValue += ")|";

            foreach (string extension in SupportedExtensions.SupportedArchives)
            {
                returnValue += "*." + extension + ";";
            }

            //Add separator
            returnValue += "|";

            //Add Images to filter
            returnValue += "Supported image formats (";

            foreach (string extension in SupportedExtensions.SupportedImages)
            {
                returnValue += "*." + extension + ";";
            }

            returnValue += ")|";

            foreach (string extension in SupportedExtensions.SupportedImages)
            {
                returnValue += "*." + extension + ";";
            }

            //Add *.*
            returnValue += "|All files (*.*)|*.*";

            return returnValue;
        }
    }
}