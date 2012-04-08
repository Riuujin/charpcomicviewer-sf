using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpComicLoader.File;

namespace CSharpComicLoader
{
	public static class Utils
	{
		/// <summary>
		/// Validates the archive file extension.
		/// </summary>
		/// <param name="FilePath">The file path.</param>
		/// <returns></returns>
		public static bool ValidateArchiveFileExtension(string FilePath)
		{
			bool returnValue = false;

			string[] supportedExtensions = Enum.GetNames(typeof(SupportedArchives));

			foreach (string extension in supportedExtensions)
			{
				if (FilePath.EndsWith("." + extension, StringComparison.OrdinalIgnoreCase))
				{
					returnValue = true;
					break;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Validates the image file extension.
		/// </summary>
		/// <param name="FilePath">The file path.</param>
		/// <returns></returns>
		public static bool ValidateImageFileExtension(string FilePath)
		{
			bool returnValue = false;

			string[] supportedExtensions = Enum.GetNames(typeof(SupportedImages));

			foreach (string extension in supportedExtensions)
			{
				if (FilePath.EndsWith("." + extension, StringComparison.OrdinalIgnoreCase))
				{
					returnValue = true;
					break;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Validates the text file extension.
		/// </summary>
		/// <param name="FilePath">The file path.</param>
		/// <returns></returns>
		public static bool ValidateTextFileExtension(string FilePath)
		{
			bool returnValue = false;

			string[] supportedExtensions = Enum.GetNames(typeof(SupportedTextFiles));

			foreach (string extension in supportedExtensions)
			{
				if (FilePath.EndsWith("." + extension, StringComparison.OrdinalIgnoreCase))
				{
					returnValue = true;
					break;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Gets the file loader filter.
		/// </summary>
		public static string FileLoaderFilter
		{
			get
			{
				string returnValue = "";

				string[] supportedArchives = Enum.GetNames(typeof(SupportedArchives));
				string[] supportedImages = Enum.GetNames(typeof(SupportedImages));

				//Add Archives to filter
				returnValue += "Supported archive formats (";

				foreach (string extension in supportedArchives)
				{
					returnValue += "*." + extension + ";";
				}

				returnValue += ")|";

				foreach (string extension in supportedArchives)
				{
					returnValue += "*." + extension + ";";
				}

				//Add separator
				returnValue += "|";

				//Add Images to filter
				returnValue += "Supported image formats (";

				foreach (string extension in supportedImages)
				{
					returnValue += "*." + extension + ";";
				}

				returnValue += ")|";

				foreach (string extension in supportedImages)
				{
					returnValue += "*." + extension + ";";
				}

				//Add *.*
				returnValue += "|All files (*.*)|*.*";

				return returnValue;
			}
		}
	}
}
