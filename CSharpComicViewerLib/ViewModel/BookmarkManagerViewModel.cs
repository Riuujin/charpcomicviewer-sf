using CSharpComicViewerLib.Data;
using CSharpComicViewerLib.Service;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace CSharpComicViewerLib.ViewModel
{
    public class BookmarkManagerViewModel : ObservableRecipient
    {

        private Bookmark selectedBookmark;
        private RelayCommand<Bookmark> deleteBookmarkCommand;
        private AsyncRelayCommand openSelectedBookmarkCommand;

        public BookmarkManagerViewModel()
        {
            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    Bookmarks.Add(new Bookmark
            //    {
            //        FilePaths = new string[] { "c:\\test.zip" },
            //        Name = "test",
            //        Page = 1
            //    });

            //    Bookmarks.Add(new Bookmark
            //    {
            //        FilePaths = new string[] { "c:\\test2.zip" },
            //        Name = "test2",
            //        Page = 55
            //    });
            //}
            //else
            //{
                var mv = Ioc.Default.GetRequiredService<MainViewModel>();
                Bookmarks = mv.Bookmarks;
        //    }
        }

        public ObservableCollection<Bookmark> Bookmarks { get; set; } = new ObservableCollection<Bookmark>();

        public Bookmark SelectedBookmark
        {
            get { return selectedBookmark; }
            set
            {
                SetProperty(ref selectedBookmark, value,true);
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
                        var ws = Ioc.Default.GetRequiredService<IApplicationService>();
                        if (ws.Confirm("Are you sure?", "Delete confirmation"))
                        {
                            Bookmarks.Remove(bookmark);
                        }
                    });
                }

                return deleteBookmarkCommand;
            }
        }

        public AsyncRelayCommand OpenSelectedBookmarkCommand
        {
            get
            {
                if (openSelectedBookmarkCommand == null)
                {
                    openSelectedBookmarkCommand = new AsyncRelayCommand(async () =>
                    {
                        var mv = Ioc.Default.GetRequiredService<MainViewModel>();
                        await mv.OpenComic(SelectedBookmark.FilePaths, SelectedBookmark.Page);
                    }, () => SelectedBookmark != null);
                }

                return openSelectedBookmarkCommand;
            }
        }
    }
}