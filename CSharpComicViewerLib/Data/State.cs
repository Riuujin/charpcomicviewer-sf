using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComicViewerLib.Data
{
   public class State
    {
        public ViewMode ViewMode { get; set; }

        public bool IsFullScreen { get; set; }

        public bool AdjustBackgroundColor { get; set; }

        public string CultureName { get; set; }
    }
}
