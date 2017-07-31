using System;
using System.IO;
using System.Linq;

namespace CSharpComicViewer.Data
{
    public static class SupportedExtensions
    {
        public static readonly string[] SupportedArchives = Enum.GetNames(typeof(SupportedArchives));

        public static readonly string[] SupportedImages = Enum.GetNames(typeof(SupportedImages));

        public static readonly string[] SupportedTextFiles = Enum.GetNames(typeof(SupportedTextFiles));


        public static bool IsSupportedArchive(string filePath)
        {
            var extension = Path.GetExtension(filePath).Substring(1);
            return SupportedExtensions.SupportedArchives.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsSupportedImage(string filePath)
        {
            var extension = Path.GetExtension(filePath).Substring(1);
			return SupportedExtensions.SupportedImages.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsSupportedTextFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).Substring(1);
			return SupportedExtensions.SupportedTextFiles.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Contains supported image extensions.
    /// </summary>
    public enum SupportedImages
	{
		jpeg,
		jpg,
		bmp,
		png
	}

	/// <summary>
	/// Contains supported archive extensions.
	/// </summary>
	public enum SupportedArchives
	{
		zip,
		rar,
		cbr,
		cbz
	}

	/// <summary>
	/// Contains supported information text extensions.
	/// </summary>
	public enum SupportedTextFiles
	{
		txt
	}
}
