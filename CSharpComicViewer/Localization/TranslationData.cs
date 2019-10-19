using CSharpComicViewer.Service;
using CSharpComicViewerLib.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.ComponentModel;

namespace CSharpComicViewer.Localization
{
    public class TranslationData : INotifyPropertyChanged, IDisposable
    {
        private string text;

        public event PropertyChangedEventHandler PropertyChanged;

        public TranslationData(string text)
        {
            this.text = text;
            TranslationService.LanguageChangedEvent += TranslationData_LanguageChangedEventManager;
        }

        private void TranslationData_LanguageChangedEventManager(object sender, EventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("Value"));
        }

        ~TranslationData()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TranslationService.LanguageChangedEvent -= TranslationData_LanguageChangedEventManager;
            }
        }

        public string Value
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ITranslationService>().Translate(text);
            }
        }
    }
}
