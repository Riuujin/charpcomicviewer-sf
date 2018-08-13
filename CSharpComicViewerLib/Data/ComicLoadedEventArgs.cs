using CSharpComicViewerLib.Comic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComicViewerLib.Data
{
    public class ComicLoadedEventArgs: EventArgs
    {
        public IComic PreviousComic { get; set; }

        public IComic CurrentComic { get; set; }
    }
}
