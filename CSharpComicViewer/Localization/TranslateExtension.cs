using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace CSharpComicViewer.Localization
{
    [ContentProperty("Text")]
    public class TranslateExtension : MarkupExtension
    {
        private string text;

        public TranslateExtension(string text)
        {
            this.text = text;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
#if DEBUG
            if (App.IsInDesignMode)
            {
                var x = new Service.TranslationService();
                return x.Translate(text);
            }
#endif

            var binding = new Binding("Value")
            {
                Source = new TranslationData(text)
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
