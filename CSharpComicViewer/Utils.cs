using CSharpComicViewer.Data;
using System;
using System.Resources;
using System.Threading;

namespace CSharpComicViewer
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Utils
    {
		private const string ResourceId = "CSharpComicViewer.Resources.Localization";

		private static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(Utils).Assembly));

		/// <summary>
		/// Gets the file loader filter.
		/// </summary>
		/// <returns>The filter.</returns>
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

		/// <summary>
		/// Translates the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The translation</returns>
		/// <exception cref="ArgumentException">Text</exception>
		public static string Translate(string text)
		{
			if (text == null)
				return "";

			var cultureInfo = Thread.CurrentThread.CurrentUICulture;

			var translation = ResMgr.Value.GetString(text, cultureInfo);

			if (translation == null)
			{
#if DEBUG
				throw new ArgumentException(
					String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", text, ResourceId, cultureInfo.Name),
					"Text");
#else
				translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
			}
			return translation;
		}
	}
}