using System;

namespace CSharpComicViewer.Service
{
    public interface IComicService
    {
        event EventHandler ComicLoaded;

        void TriggerComicLoaded(object sender);
    }
}