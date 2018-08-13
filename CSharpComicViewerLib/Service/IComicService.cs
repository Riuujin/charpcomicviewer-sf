using System;
using CSharpComicViewerLib.Data;

namespace CSharpComicViewerLib.Service
{
    public interface IComicService
    {
        event EventHandler<ComicLoadedEventArgs> ComicLoaded;
        event EventHandler<PageChangedEventArgs> PageChange;

        void TriggerComicLoaded(object sender, ComicLoadedEventArgs e);
        void TriggerPageChange(object sender, PageChangedEventArgs e);
    }
}