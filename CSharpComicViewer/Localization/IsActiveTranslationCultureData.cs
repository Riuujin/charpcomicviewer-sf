using CSharpComicViewer.Service;
using CSharpComicViewerLib.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.ComponentModel;

namespace CSharpComicViewer.Localization
{
    public class IsActiveTranslationCultureData : INotifyPropertyChanged, IDisposable
    {
        private string cultureName;

        public event PropertyChangedEventHandler PropertyChanged;

        public IsActiveTranslationCultureData(string cultureName)
        {
            this.cultureName = cultureName;
            TranslationService.LanguageChangedEvent += TranslationData_LanguageChangedEventManager;
        }

        private void TranslationData_LanguageChangedEventManager(object sender, EventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("Value"));
        }

        ~IsActiveTranslationCultureData()
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

        public bool Value
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ITranslationService>().GetTranslationCultureName() == cultureName;
            }
            set
            {
                //Don't do anything here, the setter is only here so wpf doesn't throw an exception.
            }
        }
    }
}
