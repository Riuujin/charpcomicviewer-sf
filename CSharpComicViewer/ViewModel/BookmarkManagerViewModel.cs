using CSharpComicViewer.Data;
using CSharpComicViewer.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace CSharpComicViewer.ViewModel
{
    public class BookmarkManagerViewModel : ViewModelBase
    {

        private Bookmark selectedBookmark;
        private RelayCommand<Bookmark> deleteBookmarkCommand;
        private RelayCommand openSelectedBookmarkCommand;

        public BookmarkManagerViewModel()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                Bookmarks.Add(new Bookmark
                {
                    FilePaths = new string[] { "c:\\test.zip" },
                    Name = "test",
                    Page = 1
                });

                Bookmarks.Add(new Bookmark
                {
                    FilePaths = new string[] { "c:\\test2.zip" },
                    Name = "test2",
                    Page = 55
                });
            }
            else
            {
                var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
                Bookmarks = mv.Bookmarks;
            }
        }

        public ObservableCollection<Bookmark> Bookmarks { get; set; } = new ObservableCollection<Bookmark>();

        public Bookmark SelectedBookmark
        {
            get { return selectedBookmark; }
            set
            {
                Set(ref selectedBookmark, value);
            }
        }

        public RelayCommand<Bookmark> DeleteBookmarkCommand
        {
            get
            {
                if (deleteBookmarkCommand == null)
                {
                    deleteBookmarkCommand = new RelayCommand<Bookmark>((bookmark) =>
                    {
                        var ws = CommonServiceLocator.ServiceLocator.Current.GetService(typeof(IWindowService)) as IWindowService;
                        if (ws.Confirm("Are you sure?", "Delete confirmation"))
                        {
                            Bookmarks.Remove(bookmark);
                        }
                    });
                }

                return deleteBookmarkCommand;
            }
        }

        public RelayCommand OpenSelectedBookmarkCommand
        {
            get
            {
                if (openSelectedBookmarkCommand == null)
                {
                    openSelectedBookmarkCommand = new RelayCommand(() =>
                    {
                        var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
                        mv.OpenComic(SelectedBookmark.FilePaths, SelectedBookmark.Page);
                    }, () => SelectedBookmark != null);
                }

                return openSelectedBookmarkCommand;
            }
        }
    }
}