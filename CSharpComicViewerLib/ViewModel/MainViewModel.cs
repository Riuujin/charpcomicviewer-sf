using CSharpComicViewerLib.Comic;
using CSharpComicViewerLib.Data;
using CSharpComicViewerLib.Service;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSharpComicViewerLib.ViewModel
{
    public class MainViewModel : ObservableRecipient
    {
        private ICommand addBookmarkCommand;
        private IComic comic;
        private ICommand exitCommand;
        private bool isFullscreen;
        private ICommand nextPageCommand;
        private string notificationText;
        private int numberOfPages;
        private ICommand openAboutCommand;
        private ICommand openBookmarkManagerCommand;
        private ICommand openCommand;
        private byte[] page;
        private bool pageCountVisible;
        private int pageNumber;
        private ICommand previousPageCommand;
        private ICommand resumeCommand;
        private Bookmark resumeData;
        private ICommand toggleFullscreenCommand;
        private ICommand toggleViewModeCommand;
        private ViewMode viewMode;
        private bool adjustBackgroundColor;
        private RelayCommand toggleAdjustBackgroundColorCommand;
        private RelayCommand<string> setTranslationCultureCommand;
        private readonly IComicService comicService;
        private readonly ITranslationService translationService;
        private readonly IApplicationService applicationService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IComicService comicService, ITranslationService translationService, IApplicationService applicationService)
        {
            InitBookmarkContextMenu();
            this.comicService = comicService;
            this.translationService = translationService;
            this.applicationService = applicationService;
        }

        public ICommand AddBookmarkCommand
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
                        NotificationText = null;
                        NotificationText = translationService.Translate("Notification_BookmarkAdded");
                    }, () => { return Comic != null; });
                }

                return addBookmarkCommand;
            }
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
                SetProperty(ref comic, value, true);
                PageCountVisible = value != null;
            }
        }

        public void SetResumeData(Bookmark resumeData)
        {
            this.resumeData = resumeData;
        }

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand(() =>
                    {
                        applicationService.ApplicationShutdown();
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
                SetProperty(ref isFullscreen, value, true);
            }
        }

        public ICommand NextPageCommand
        {
            get
            {
                if (nextPageCommand == null)
                {
                    nextPageCommand = new AsyncRelayCommand(async () =>
                   {
                       var page = await Comic.GetPage(PageNumber + 1);
                       if (page != null)
                       {
                           PageNumber++;
                           Page = page;

                           comicService.TriggerPageChange(this, new PageChangedEventArgs { PreviousPage = PageNumber - 1, CurrentPage = PageNumber });
                       }
                       else
                       {
                           NotificationText = translationService.Translate("Notification_UnableToGetNextPage");

                       }

                   }, () => { return Comic != null && PageNumber < NumberOfPages; });
                }

                return nextPageCommand;
            }
        }

        public string NotificationText
        {
            get { return notificationText; }
            set
            {
                SetProperty(ref notificationText, value, true);
            }
        }

        public int NumberOfPages
        {
            get { return numberOfPages; }
            set
            {
                SetProperty(ref numberOfPages, value, true);
            }
        }

        public ICommand OpenAboutCommand
        {
            get
            {
                if (openAboutCommand == null)
                {
                    openAboutCommand = new RelayCommand(() =>
                    {
                        applicationService.OpenAboutWindow();
                    });
                }

                return openAboutCommand;
            }
        }



        public ICommand OpenBookmarkManagerCommand
        {
            get
            {
                if (openBookmarkManagerCommand == null)
                {
                    openBookmarkManagerCommand = new RelayCommand(() =>
                    {
                        applicationService.OpenBookmarkManagerWindow();
                    }, () => Bookmarks.Count > 0);
                }

                return openBookmarkManagerCommand;
            }
        }


        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new AsyncRelayCommand(
                           async () =>
                           {

                               var initialFileDialogPath = comic?.GetFilePaths()[0] ?? resumeData?.FilePaths[0];
                               if (initialFileDialogPath != null)
                               {
                                   initialFileDialogPath = System.IO.Path.GetDirectoryName(initialFileDialogPath);
                               }

                               var files = applicationService.OpenFileDialog(initialFileDialogPath);

                               if (files?.Length <= 0)
                               {
                                   return;
                               }

                               Array.Sort(files);

                               await Task.Run(async () =>
                               {
                                   try
                                   {
                                       await OpenComic(files, 1);
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

        public byte[] Page
        {
            get { return page; }
            set
            {
                SetProperty(ref page, value, true);
            }
        }

        public bool PageCountVisible
        {
            get { return pageCountVisible; }
            set
            {
                SetProperty(ref pageCountVisible, value, true);
            }
        }

        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                SetProperty(ref pageNumber, value, true);
            }
        }

        public ICommand PreviousPageCommand
        {
            get
            {
                if (previousPageCommand == null)
                {
                    previousPageCommand = new AsyncRelayCommand(async () =>
                    {
                        var page = await Comic.GetPage(PageNumber - 1);
                        if (page != null)
                        {
                            PageNumber--;
                            Page = page;

                            comicService.TriggerPageChange(this, new PageChangedEventArgs { PreviousPage = PageNumber + 1, CurrentPage = PageNumber });
                        }
                        else
                        {
                            NotificationText = translationService.Translate("Notification_UnableToGetPreviousPage");

                        }

                    }, () => { return Comic != null && PageNumber > 1; });
                }

                return previousPageCommand;
            }
        }

        public ICommand ResumeCommand
        {
            get
            {
                if (resumeCommand == null)
                {
                    resumeCommand = new AsyncRelayCommand(async () =>
                    {
                        await OpenComic(resumeData.FilePaths, resumeData.Page);
                    }, () => { return resumeData?.FilePaths.Length > 0; });
                }

                return resumeCommand;
            }
        }

        public ICommand ToggleFullscreenCommand
        {
            get
            {
                if (toggleFullscreenCommand == null)
                {
                    toggleFullscreenCommand = new RelayCommand(() =>
                    {
                        IsFullscreen = applicationService.ToggleFullscreen();
                    });
                }

                return toggleFullscreenCommand;
            }
        }

        public ICommand ToggleAdjustBackgroundColorCommand
        {
            get
            {
                if (toggleAdjustBackgroundColorCommand == null)
                {
                    toggleAdjustBackgroundColorCommand = new RelayCommand(() =>
                    {
                        AdjustBackgroundColor = !AdjustBackgroundColor;
                    });
                }

                return toggleAdjustBackgroundColorCommand;
            }
        }

        public ICommand SetTranslationCultureCommand
        {
            get
            {
                if (setTranslationCultureCommand == null)
                {
                    setTranslationCultureCommand = new RelayCommand<string>((string cultureName) =>
                    {
                        translationService.SetTranslationCultureName(cultureName);
                    });
                }

                return setTranslationCultureCommand;
            }
        }

        public ICommand ToggleViewModeCommand
        {
            get
            {
                if (toggleViewModeCommand == null)
                {
                    toggleViewModeCommand = new RelayCommand(() =>
                    {
                        NotificationText = null;

                        if (ViewMode == ViewMode.Normal)
                        {
                            ViewMode = ViewMode.FitToScreen;
                            NotificationText = translationService.Translate("Notification_FitToScreen");
                        }
                        else if (ViewMode == ViewMode.FitToScreen)
                        {
                            ViewMode = ViewMode.FitToHeight;
                            NotificationText = translationService.Translate("Notification_FitToHeight");
                        }
                        else if (ViewMode == ViewMode.FitToHeight)
                        {
                            ViewMode = ViewMode.FitToWidth;
                            NotificationText = translationService.Translate("Notification_FitToWidth");
                        }
                        else if (ViewMode == ViewMode.FitToWidth)
                        {
                            ViewMode = ViewMode.Normal;
                            NotificationText = translationService.Translate("Notification_Normal");
                        }
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
                SetProperty(ref viewMode, value, true);
            }
        }

        public bool AdjustBackgroundColor
        {
            get { return adjustBackgroundColor; }
            set
            {
                SetProperty(ref adjustBackgroundColor, value, true);
            }
        }

        public string WindowTitle => "C# Comic Viewer";

        /// <summary>
        /// Loads from storage.
        /// </summary>
        public void LoadFromStorage()
        {

        }
        public async Task OpenComic(string[] files, int pageNumber)
        {
            var previousComic = Comic;
            var comic = ComicFactory.Create(files);
            if (comic != null)
            {
                NumberOfPages = await comic.GetNumberOfPages();
                PageNumber = pageNumber;
                var page = await comic.GetPage(PageNumber);
                Page = page;
                Comic = comic;

                comicService.TriggerComicLoaded(this, new ComicLoadedEventArgs { PreviousComic = previousComic, CurrentComic = Comic });
            }
        }


        public void HandleException(Exception ex)
        {
            NotificationText = ex.Message;
        }
        private void InitBookmarkContextMenu()
        {
            Bookmarks.CollectionChanged += delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.OldItems != null)
                {
                    foreach (Bookmark bookmark in e.OldItems)
                    {
                        var toDelete = BookmarkMenu.OfType<BookmarkContextMenuItem>().Where(x => x.Bookmark == bookmark).ToArray();
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
                            Command = new AsyncRelayCommand<Bookmark>(async (Bookmark b) =>
                            {
                                await OpenComic(b.FilePaths, b.Page);
                            }),
                            IsEnabled = bookmark.AllFilesExist
                        });
                    }
                }
            };
        }
    }
}