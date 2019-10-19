using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpComicViewerLib.Service
{
    public interface ITranslationService
    {
        void SetTranslationCultureName(string code);
        string Translate(string text);
        string GetTranslationCultureName();
    }
}
