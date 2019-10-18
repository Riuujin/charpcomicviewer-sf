using CSharpComicViewerLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComicViewerLib.Service
{
    public class ComicService : IComicService
    {
        public event EventHandler<ComicLoadedEventArgs> ComicLoaded;

        public event EventHandler<PageChangedEventArgs> PageChange;

        public void TriggerComicLoaded(object sender, ComicLoadedEventArgs e)
        {
            Task.Run(() => ComicLoaded.Invoke(sender, e));
        }

        public void TriggerPageChange(object sender, PageChangedEventArgs e)
        {
            Task.Run(() => PageChange.Invoke(sender, e));
        }
    }
}
