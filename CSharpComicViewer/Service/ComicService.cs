using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComicViewer.Service
{
    public class ComicService : IComicService
    {
        public event EventHandler ComicLoaded;

        public void TriggerComicLoaded(object sender)
        {
            ComicLoaded?.BeginInvoke(sender, new EventArgs(), null, null);
        }
    }
}
