using CSharpComicViewer.Data;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace CSharpComicViewer.ViewModel
{
    public interface IContextItemModel
    {
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        ICommand Command { get; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        string Header { get; set; }

        /// <summary>
        /// Gets or sets the input gesture text.
        /// </summary>
        /// <value>
        /// The input gesture text.
        /// </value>
        string InputGestureText { get; set; }

        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        string ToolTip { get; set; }
    }
}