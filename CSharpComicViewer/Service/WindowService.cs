using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpComicViewer.Service
{
    public class WindowService : IWindowService
    {
        private Window window;

        public void SetWindow(Window window)
        {
            this.window = window;
        }

        public bool ToggleFullscreen()
        {
            if (window == null) {
                return false;
            }

            if (window.WindowStyle == WindowStyle.None)
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.ResizeMode = ResizeMode.CanResize;
                window.WindowState = System.Windows.WindowState.Normal;
                return false;
            }
            else
            {
                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.WindowState = System.Windows.WindowState.Maximized;
                return true;
            }
        }
    }
}
