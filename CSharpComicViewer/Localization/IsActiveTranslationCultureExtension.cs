using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace CSharpComicViewer.Localization
{
    [ContentProperty("IsChecked")]
    public class IsActiveTranslationCultureExtension : MarkupExtension
    {
        private string cultureName;

        public IsActiveTranslationCultureExtension(string cultureName)
        {
            this.cultureName = cultureName;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
            {
                Source = new IsActiveTranslationCultureData(cultureName)
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
