using CSharpComicViewerLib.Data;
using CSharpComicViewerLib.Service;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CSharpComicViewer.Controls
{
    /// <summary>
    /// Interaction logic for PageViewer.xaml
    /// </summary>
    public partial class PageViewer : UserControl
    {
        public static readonly DependencyProperty GoToNextCommandProperty =
           DependencyProperty.Register(nameof(GoToNextCommand), typeof(ICommand), typeof(PageViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty GoToPreviousCommandProperty =
           DependencyProperty.Register(nameof(GoToPreviousCommand), typeof(ICommand), typeof(PageViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(PageViewer), new PropertyMetadata(null, new PropertyChangedCallback(OnImageSourceChanged)));

        public static readonly DependencyProperty ViewModeProperty =
            DependencyProperty.Register(nameof(ViewMode), typeof(ViewMode), typeof(PageViewer), new PropertyMetadata(ViewMode.Normal, new PropertyChangedCallback(OnViewModeChanged)));

        public static readonly DependencyProperty AdjustBackgroundColorProperty =
           DependencyProperty.Register(nameof(AdjustBackgroundColor), typeof(bool), typeof(PageViewer), new PropertyMetadata(false, new PropertyChangedCallback(OnAdjustBackgroundColorChanged)));

        private const int PAGE_SWITCH_TRESHOLD = 10;

        private Point? previousMousePosition;
        private int pageSwitchTreshold = PAGE_SWITCH_TRESHOLD;
        private LastScrollDirection lastScrollDirection = LastScrollDirection.Down;
        private bool scrollLock = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewer"/> class.
        /// </summary>
        public PageViewer()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var cs = Ioc.Default.GetRequiredService<IComicService>();
                cs.ComicLoaded += ComicService_ComicLoaded;
                cs.PageChange += ComicService_PageChange;
            }
        }

        private void ComicService_PageChange(object sender, PageChangedEventArgs e)
        {
            if (e.CurrentPage > e.PreviousPage)
            {
                Dispatcher.Invoke(() =>
                {
                    ScrollToBeginning();
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    ScrollToEnd();
                });
            }
        }

        private void ComicService_ComicLoaded(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ScrollToBeginning();
            });
        }

        /// <summary>
        /// Gets or sets the go to next command.
        /// </summary>
        /// <value>
        /// The go to next command.
        /// </value>
        public ICommand GoToNextCommand
        {
            get { return (ICommand)GetValue(GoToNextCommandProperty); }
            set
            {
                SetValue(GoToNextCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the go to previous command.
        /// </summary>
        /// <value>
        /// The go to previous command.
        /// </value>
        public ICommand GoToPreviousCommand
        {
            get { return (ICommand)GetValue(GoToPreviousCommandProperty); }
            set
            {
                SetValue(GoToPreviousCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the view mode.
        /// </summary>
        /// <value>
        /// The view mode.
        /// </value>
        public ViewMode ViewMode
        {
            get { return (ViewMode)GetValue(ViewModeProperty); }
            set
            {
                SetValue(ViewModeProperty, value);
            }
        }

        public bool AdjustBackgroundColor
        {
            get { return (bool)GetValue(AdjustBackgroundColorProperty); }
            set
            {
                SetValue(AdjustBackgroundColorProperty, value);
            }
        }


        /// <summary>
        /// Called when mouse wheel is used.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        public void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (scrollLock)
            {
                e.Handled = true;
                return;
            }

            if (e.Delta < 0)
            {
                //Down
                if (lastScrollDirection != LastScrollDirection.Down)
                {
                    pageSwitchTreshold = PAGE_SWITCH_TRESHOLD;
                    lastScrollDirection = LastScrollDirection.Down;
                }

                if (ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight
                    && ScrollViewer.ScrollableWidth > 0 && ScrollViewer.HorizontalOffset != ScrollViewer.ScrollableWidth)
                {
                    ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + 20);
                    e.Handled = true;
                    return;
                }

                pageSwitchTreshold--;

                if (ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight
                   && ScrollViewer.HorizontalOffset == ScrollViewer.ScrollableWidth
                   && pageSwitchTreshold <= 0
                   && GoToNextCommand?.CanExecute(null) == true)
                {
                    e.Handled = true;
                    scrollLock = true;
                    GoToNextCommand.Execute(null);
                    pageSwitchTreshold = PAGE_SWITCH_TRESHOLD;
                    return;
                }
            }
            else
            {
                //Up
                if (lastScrollDirection != LastScrollDirection.Up)
                {
                    pageSwitchTreshold = PAGE_SWITCH_TRESHOLD;
                    lastScrollDirection = LastScrollDirection.Up;
                }

                if (ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight
                    && ScrollViewer.ScrollableWidth > 0 && ScrollViewer.HorizontalOffset != 0)
                {
                    ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - 20);
                    e.Handled = true;
                    return;
                }

                pageSwitchTreshold--;

                if (ScrollViewer.VerticalOffset == 0
                   && ScrollViewer.HorizontalOffset == 0
                   && pageSwitchTreshold <= 0
                   && GoToPreviousCommand?.CanExecute(null) == true)
                {
                    e.Handled = true;
                    scrollLock = true;
                    GoToPreviousCommand.Execute(null);
                    pageSwitchTreshold = PAGE_SWITCH_TRESHOLD;
                    return;
                }
            }
        }

        /// <summary>
        /// Scrolls to beginning.
        /// </summary>
        public void ScrollToBeginning()
        {
            ScrollViewer.ScrollToLeftEnd();
            ScrollViewer.ScrollToTop();
        }

        /// <summary>
        /// Scrolls to end.
        /// </summary>
        public void ScrollToEnd()
        {
            ScrollViewer.ScrollToRightEnd();
            ScrollViewer.ScrollToBottom();
        }

        private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageViewer pv = d as PageViewer;
            var viewMode = (ViewMode)Enum.Parse(typeof(ViewMode), e.NewValue.ToString());

            //Always clear current bindings
            BindingOperations.ClearBinding(pv.Image, Image.MaxHeightProperty);
            BindingOperations.ClearBinding(pv.Image, Image.MaxWidthProperty);

            if (viewMode == ViewMode.FitToScreen)
            {
                var heightBinding = new Binding(nameof(pv.ScrollViewer.ActualHeight))
                {
                    ElementName = nameof(ScrollViewer)
                };
                pv.Image.SetBinding(Image.MaxHeightProperty, heightBinding);

                var widthBinding = new Binding(nameof(pv.ScrollViewer.ActualWidth))
                {
                    ElementName = nameof(ScrollViewer)
                };
                pv.Image.SetBinding(Image.MaxWidthProperty, widthBinding);
            }
            else if (viewMode == ViewMode.FitToHeight)
            {
                var heightBinding = new Binding(nameof(pv.ScrollViewer.ActualHeight))
                {
                    ElementName = nameof(ScrollViewer)
                };
                pv.Image.SetBinding(Image.MaxHeightProperty, heightBinding);
            }
            else if (viewMode == ViewMode.FitToWidth)
            {
                var widthBinding = new Binding(nameof(pv.ScrollViewer.ActualWidth))
                {
                    ElementName = nameof(ScrollViewer)
                };
                pv.Image.SetBinding(Image.MaxWidthProperty, widthBinding);
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                previousMousePosition = e.GetPosition(this);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var currentMousePosition = e.GetPosition(this);

            if (previousMousePosition == null)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentMousePosition.X < previousMousePosition.Value.X && ImageSource != null)
                {
                    //Drag left
                    ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + (previousMousePosition.Value.X - currentMousePosition.X));
                }
                else if (currentMousePosition.X > previousMousePosition.Value.X && ImageSource != null)
                {
                    //Drag right
                    ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + (previousMousePosition.Value.X - currentMousePosition.X));
                }

                if (currentMousePosition.Y < previousMousePosition.Value.Y && ImageSource != null)
                {
                    //Drag up
                    ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + (previousMousePosition.Value.Y - currentMousePosition.Y));
                }
                else if (currentMousePosition.Y > previousMousePosition.Value.Y && ImageSource != null)
                {
                    //Drag down
                    ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + (previousMousePosition.Value.Y - currentMousePosition.Y));
                }

                previousMousePosition = currentMousePosition;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                previousMousePosition = null;
            }
        }

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageViewer pv = d as PageViewer;
            pv.scrollLock = false;
            var src = e.NewValue as BitmapSource;

            if (src != null && pv.AdjustBackgroundColor)
            {
                var brush = GetBackgroundColor(src);
                pv.ScrollViewer.Background = brush;
            }
            else
            {
                pv.ScrollViewer.Background = null;
            }
        }

        private static void OnAdjustBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageViewer pv = d as PageViewer;
            var src = pv.ImageSource as BitmapSource;

            if (src != null && (bool)e.NewValue)
            {
                var brush = GetBackgroundColor(src);
                pv.ScrollViewer.Background = brush;
            }
            else
            {
                pv.ScrollViewer.Background = null;
            }
        }

        private static Brush GetBackgroundColor(BitmapSource bitmap)
        {
            int dividedBy = 100;
            Color[] colors = new Color[dividedBy * 4];

            //get the color of a pixels at the edge of image
            int i = 0;

            //left
            for (int y = 0; y < dividedBy; y++)
            {
                colors[i++] = GetPixelColor(bitmap, 0, y * (bitmap.PixelHeight / dividedBy));
            }

            //top
            for (int x = 0; x < dividedBy; x++)
            {
                colors[i++] = GetPixelColor(bitmap, x * (bitmap.PixelWidth / dividedBy), 0);
            }

            //right
            for (int y = 0; y < dividedBy; y++)
            {
                colors[i++] = GetPixelColor(bitmap, bitmap.PixelWidth - 1, y * (bitmap.PixelHeight / dividedBy));
            }

            //bottom
            for (int x = 0; x < dividedBy; x++)
            {
                colors[i++] = GetPixelColor(bitmap, x * (bitmap.PixelWidth / dividedBy), bitmap.PixelHeight - 1);
            }

            var colorFrequency = from color in colors
                                 group color by color into grouped
                                 select new { Color = grouped.Key, Freq = grouped.Count() };

            var backColor = colorFrequency
                                .OrderByDescending(x => x.Freq)
                                .First()
                                .Color;

            Color BackColorWPF = new Color();
            BackColorWPF.A = backColor.A;
            BackColorWPF.B = backColor.B;
            BackColorWPF.G = backColor.G;
            BackColorWPF.R = backColor.R;
            return new SolidColorBrush(BackColorWPF);
        }

        public static Color GetPixelColor(BitmapSource bitmap, int x, int y)
        {
            Color color;
            var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            var bytes = new byte[bytesPerPixel];
            var rect = new Int32Rect(x, y, 1, 1);

            bitmap.CopyPixels(rect, bytes, bytesPerPixel, 0);

            if (bitmap.Format == PixelFormats.Bgra32)
            {
                color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            }
            else if (bitmap.Format == PixelFormats.Bgr32)
            {
                color = Color.FromRgb(bytes[2], bytes[1], bytes[0]);
            }
            // handle other required formats
            else
            {
                color = Colors.White;
            }

            return color;
        }

        private enum LastScrollDirection
        {
            Down = 0,
            Up = 1
        }
    }
}