using System.Windows;

namespace CSharpComicViewer.Service
{
    public interface IWindowService
    {
        void SetWindow(Window window);
        bool ToggleFullscreen();
    }
}