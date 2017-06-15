using CSharpComicViewer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComicViewer.Service
{
    public class ComicService : IComicService
    {
        public event EventHandler<ComicLoadedEventArgs> ComicLoaded;

        public event EventHandler<PageChangedEventArgs> PageChange;

        public void TriggerComicLoaded(object sender, ComicLoadedEventArgs e)
        {
            ComicLoaded?.BeginInvoke(sender, e, null, null);
        }

        public void TriggerPageChange(object sender, PageChangedEventArgs e)
        {
            PageChange?.BeginInvoke(sender, e, null, null);
        }
    }
}
