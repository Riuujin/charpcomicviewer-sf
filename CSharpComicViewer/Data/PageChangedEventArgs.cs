using System;

namespace CSharpComicViewer.Data
{
    public class PageChangedEventArgs : EventArgs
    {
        public int PreviousPage { get; set; }
        public int CurrentPage { get; set; }
    }
}
