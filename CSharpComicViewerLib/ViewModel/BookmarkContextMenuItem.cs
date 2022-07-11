using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpComicViewerLib.Data;
using CSharpComicViewerLib.ViewModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CSharpComicViewerLib.ViewModel
{
    public class BookmarkContextMenuItem : ObservableRecipient
    {
        private string header;
        private string inputGestureText;
        private string toolTip;

        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        public string ToolTip
        {
            get { return toolTip; }
            set
            {
                SetProperty(ref toolTip, value, true);
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public string Header
        {
            get { return header; }
            set
            {
                SetProperty(ref header, value, true);
            }
        }

        /// <summary>
        /// Gets or sets the input gesture text.
        /// </summary>
        /// <value>
        /// The input gesture text.
        /// </value>
        public string InputGestureText
        {
            get { return inputGestureText; }
            set
            {
                SetProperty(ref inputGestureText, value, true);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public ICommand Command
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the bookmark.
        /// </summary>
        /// <value>
        /// The bookmark.
        /// </value>
        public Bookmark Bookmark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled { get; set; }
    }
}
