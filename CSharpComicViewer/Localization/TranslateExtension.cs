using CSharpComicViewerLib.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CSharpComicViewer.Localization
{
    /// <summary>
    /// Translation extension for translating text.
    /// </summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    [ContentProperty("Text")]
    public class TranslateExtension : MarkupExtension
    {
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public TranslateExtension(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        /// <exception cref="ArgumentException">Text</exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
#if DEBUG
            if (App.IsInDesignMode)
            {
                var x = new Service.UtilityService();
                return x.Translate(text);
            }
#endif
            return SimpleIoc.Default.GetInstance<IUtilityService>().Translate(text);
        }
    }
}
