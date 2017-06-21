using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpComicViewer.Data;
using CSharpComicViewer.ViewModel;
using GalaSoft.MvvmLight;

namespace CSharpComicViewer.ViewModel
{
    public class BookmarkContextMenuItem : ViewModelBase
    {
        private string header;
        private string inputGestureText;
        private ICommand action;
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
                Set(ref toolTip, value);
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
                Set(ref header, value);
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
                Set(ref inputGestureText, value);
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
            get;set;
        }

        /// <summary>
        /// Gets or sets the bookmark.
        /// </summary>
        /// <value>
        /// The bookmark.
        /// </value>
        public Bookmark Bookmark { get; set; }
    }
}
