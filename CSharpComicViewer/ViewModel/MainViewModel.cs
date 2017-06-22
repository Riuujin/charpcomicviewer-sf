﻿using CSharpComicViewer.Comic;
using CSharpComicViewer.Data;
using CSharpComicViewer.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System;

namespace CSharpComicViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand addBookmarkCommand;
        private IComic comic;
        private RelayCommand exitCommand;
        private bool isFullscreen;
        private RelayCommand openCommand;
        private RelayCommand nextPageCommand;
        private int numberOfPages;
        private byte[] page;
        private int pageNumber;
        private RelayCommand previousPageCommand;
        private RelayCommand resumeCommand;
        private Bookmark resumeData;
        private RelayCommand toggleFullscreenCommand;
        private RelayCommand toggleViewModeCommand;
        private ViewMode viewMode;
        private bool pageCountVisible;
        private string notificationText;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            InitBookmarkContextMenu();
        }

        public RelayCommand AddBookmarkCommand
        {
            get
            {
                if (addBookmarkCommand == null)
                {
                    addBookmarkCommand = new RelayCommand(() =>
                    {
                        var bookmark = new Bookmark
                        {
                            Name = Comic.getBookmarkName(PageNumber),
                            FilePaths = Comic.GetFilePaths(),
                            Page = PageNumber
                        };

                        Bookmarks.Add(bookmark);
                    }, () => { return Comic != null; });
                }

                return addBookmarkCommand;
            }
        }

        internal void HandleException(Exception ex)
        {
            NotificationText = ex.Message;
        }

        public ObservableCollection<BookmarkContextMenuItem> BookmarkMenu { get; set; } = new ObservableCollection<BookmarkContextMenuItem>();

        public ObservableCollection<Bookmark> Bookmarks { get; set; } = new ObservableCollection<Bookmark>();

        public IComic Comic
        {
            get
            {
                return comic;
            }
            set
            {
                Set(ref comic, value);
                PageCountVisible = value != null;
            }
        }

        public RelayCommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand(() =>
                    {
                        Application.Current.Shutdown();
                    });
                }

                return exitCommand;
            }
        }

        public bool IsFullscreen
        {
            get { return isFullscreen; }
            set
            {
                Set(ref isFullscreen, value);
            }
        }


        public string NotificationText
        {
            get { return notificationText; }
            set
            {
                Set(ref notificationText, value);
            }
        }

        public RelayCommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new RelayCommand(
                           async () =>
                           {
                               OpenFileDialog openFileDialog = new OpenFileDialog();

                               openFileDialog.Filter = Utils.GetFileLoaderFilter();
                               openFileDialog.Multiselect = true;
                               openFileDialog.ShowDialog();

                               if (openFileDialog.FileNames.Length <= 0)
                               {
                                   return;
                               }

                               await Task.Run(async () =>
                               {
                                   try
                                   {
                                       var files = openFileDialog.FileNames;
                                       await LoadComic(files);
                                   }
                                   catch (Exception ex)
                                   {
                                       HandleException(ex);
                                   }

                               });
                           });
                }

                return openCommand;
            }
        }

        private async Task LoadComic(string[] files)
        {
            var previousComic = Comic;
            var comic = ComicFactory.Create(files);
            if (comic != null)
            {
                NumberOfPages = comic.Pages();
                PageNumber = 1;
                var page = await comic.GetPage(PageNumber);
                Page = page;
                Comic = comic;

                var cs = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IComicService)) as IComicService;
                cs.TriggerComicLoaded(this, new ComicLoadedEventArgs { PreviousComic = previousComic, CurrentComic = Comic });
            }
        }

        public RelayCommand NextPageCommand
        {
            get
            {
                if (nextPageCommand == null)
                {
                    nextPageCommand = new RelayCommand(async () =>
                   {
                       var page = await Comic.GetPage(PageNumber + 1);
                       if (page != null)
                       {
                           PageNumber++;
                           Page = page;

                           var cs = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IComicService)) as IComicService;
                           cs.TriggerPageChange(this, new PageChangedEventArgs { PreviousPage = PageNumber - 1, CurrentPage = PageNumber });
                       }
                       else
                       {
                           NotificationText = "Unable to get next page.";
                       }

                   }, () => { return Comic != null && PageNumber < NumberOfPages; });
                }

                return nextPageCommand;
            }
        }

        public int NumberOfPages
        {
            get { return numberOfPages; }
            set
            {
                Set(ref numberOfPages, value);
            }
        }

        public byte[] Page
        {
            get { return page; }
            set
            {
                Set(ref page, value);
            }
        }

        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                Set(ref pageNumber, value);
            }
        }

        public RelayCommand PreviousPageCommand
        {
            get
            {
                if (previousPageCommand == null)
                {
                    previousPageCommand = new RelayCommand(async () =>
                    {
                        var page = await Comic.GetPage(PageNumber - 1);
                        if (page != null)
                        {
                            PageNumber--;
                            Page = page;

                            var cs = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IComicService)) as IComicService;
                            cs.TriggerPageChange(this, new PageChangedEventArgs { PreviousPage = PageNumber + 1, CurrentPage = PageNumber });
                        }
                        else
                        {
                            NotificationText = "Unable to get previous page.";
                        }

                    }, () => { return Comic != null && PageNumber > 1; });
                }

                return previousPageCommand;
            }
        }

        public RelayCommand ResumeCommand
        {
            get
            {
                if (resumeCommand == null)
                {
                    resumeCommand = new RelayCommand(async () =>
                    {
                        await LoadComic(resumeData.FilePaths);
                    }, () => { return resumeData?.FilePaths.Length > 0; });
                }

                return resumeCommand;
            }
        }

        public RelayCommand ToggleFullscreenCommand
        {
            get
            {
                if (toggleFullscreenCommand == null)
                {
                    toggleFullscreenCommand = new RelayCommand(() =>
                    {
                        var ws = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IWindowService)) as IWindowService;
                        IsFullscreen = ws.ToggleFullscreen();
                    });
                }

                return toggleFullscreenCommand;
            }
        }

        public RelayCommand ToggleViewModeCommand
        {
            get
            {
                if (toggleViewModeCommand == null)
                {
                    toggleViewModeCommand = new RelayCommand(() =>
                    {
                        if (ViewMode == ViewMode.Normal)
                        {
                            ViewMode = ViewMode.FitToScreen;
                        }
                        else if (ViewMode == ViewMode.FitToScreen)
                        {
                            ViewMode = ViewMode.FitToHeight;
                        }
                        else if (ViewMode == ViewMode.FitToHeight)
                        {
                            ViewMode = ViewMode.FitToWidth;
                        }
                        else if (ViewMode == ViewMode.FitToWidth)
                        {
                            ViewMode = ViewMode.Normal;
                        }

                        NotificationText = ViewMode.ToString();
                    });
                }

                return toggleViewModeCommand;
            }
        }

        public ViewMode ViewMode
        {
            get { return viewMode; }
            set
            {
                Set(ref viewMode, value);
            }
        }

        public bool PageCountVisible
        {
            get { return pageCountVisible; }
            set
            {
                Set(ref pageCountVisible, value);
            }
        }

        public string WindowTitle => "C# Comic Viewer";

        /// <summary>
        /// Loads from storage.
        /// </summary>
        public void LoadFromStorage()
        {
            var service = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IDataStorageService)) as IDataStorageService;

            var state = service.Load<State>("state");
            var resumeData = service.Load<Bookmark>("resumeData");
            var bookmarks = service.Load<Bookmark[]>("bookmarks");

            if (state == null && resumeData == null && bookmarks == null)
            {
                var legacyService = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(ILegacyConfigurationMigrationService)) as ILegacyConfigurationMigrationService;
                legacyService.Migrate();

                //Reload data, it might have been changed.
                resumeData = service.Load<Bookmark>("resumeData");
                bookmarks = service.Load<Bookmark[]>("bookmarks");
            }

            if (state != null)
            {
                this.ViewMode = state.ViewMode;

                if (state.IsFullScreen)
                {
                    //Initial state will never be fullscreen, toggle to fullscreen if state requires it.
                    var ws = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IWindowService)) as IWindowService;
                    IsFullscreen = ws.ToggleFullscreen();
                }
            }

            if (resumeData?.FilePaths?.Length >= 1)
            {
                bool allFilesExist = true;

                foreach (var filePath in resumeData.FilePaths)
                {
                    if (!System.IO.File.Exists(filePath))
                    {
                        allFilesExist = false;
                        break;
                    }
                }

                if (allFilesExist)
                {
                    this.resumeData = resumeData;
                }
            }

            if (bookmarks?.Length > 0)
            {
                foreach (var bookmark in bookmarks)
                {
                    Bookmarks.Add(bookmark);
                }
            }
        }

        /// <summary>
        /// Saves to storage.
        /// </summary>
        public void SaveToStorage()
        {
            var service = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IDataStorageService)) as IDataStorageService;

            if (Comic != null)
            {
                var resumeData = new Bookmark
                {
                    FilePaths = Comic.GetFilePaths(),
                    Page = PageNumber
                };

                service.Save("resumeData", resumeData);
            }

            service.Save("bookmarks", Bookmarks);

            service.Save("state", new State
            {
                ViewMode = ViewMode,
                IsFullScreen = IsFullscreen
            });
        }

        private void InitBookmarkContextMenu()
        {
            Bookmarks.CollectionChanged += delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.OldItems != null)
                {
                    foreach (Bookmark bookmark in e.OldItems)
                    {
                        var toDelete = BookmarkMenu.OfType<BookmarkContextMenuItem>().Where(x => x.Bookmark == bookmark);
                        foreach (BookmarkContextMenuItem item in toDelete)
                        {
                            BookmarkMenu.Remove(item);
                        }
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (Bookmark bookmark in e.NewItems)
                    {
                        BookmarkMenu.Add(new BookmarkContextMenuItem()
                        {
                            Header = $"{bookmark.Name}{Environment.NewLine}Page: {bookmark.Page}",
                            ToolTip = $"{string.Join(Environment.NewLine, bookmark.FilePaths)}",
                            Bookmark = bookmark,
                            Command = new RelayCommand<Bookmark>(async (Bookmark b) =>
                            {
                                await LoadComic(b.FilePaths);
                            }),
                            IsEnabled = bookmark.FilePaths.All(filePath => System.IO.File.Exists(filePath))
                        });
                    }
                }
            };
        }
    }
}