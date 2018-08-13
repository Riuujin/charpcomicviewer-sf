using System;

namespace CSharpComicViewerLib.Data
{
    public class PageChangedEventArgs : EventArgs
    {
        public int PreviousPage { get; set; }
        public int CurrentPage { get; set; }
    }
}
