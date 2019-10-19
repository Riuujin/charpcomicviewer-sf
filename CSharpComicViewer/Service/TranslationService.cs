using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpComicViewerLib.Service;

namespace CSharpComicViewer.Service
{
    class TranslationService : ITranslationService
    {
        private const string ResourceId = "CSharpComicViewer.Resources.Localization";

        private readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslationService).Assembly));

        public static event EventHandler LanguageChangedEvent;

        private CultureInfo translationCulture = new CultureInfo("en");

        public void SetTranslationCultureName(string name)
        {
            if (name != translationCulture.Name)
            {
                translationCulture = new CultureInfo(name);
                Task.Run(() => LanguageChangedEvent?.Invoke(this, null));
            }
        }

        public string GetTranslationCultureName()
        {
            return translationCulture.Name;
        }

        /// <summary>
        /// Translates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The translation</returns>
        /// <exception cref="ArgumentException">Text</exception>
        public string Translate(string text)
        {
            if (text == null)
                return "";

            var cultureInfo = translationCulture;

            var translation = ResMgr.Value.GetString(text, cultureInfo);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", text, ResourceId, cultureInfo.Name),
                    "Text");
#else
				translation = text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
