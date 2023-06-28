using LazarovEAV.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using LazarovEAV.Util.Util;
using System.Windows.Threading;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty ActivePageProperty =
                                                      DependencyProperty.Register("ActivePage", typeof(UiPageType), typeof(MainWindow),
                                                      new PropertyMetadata(UiPageType.None, (o, arg) => { ((MainWindow)o).onSetActivePage((UiPageType)arg.NewValue); }));

        internal UiPageType ActivePage { get { return (UiPageType)GetValue(ActivePageProperty); } set { SetValue(ActivePageProperty, value); this.ActiveOverlay = UiOverlayType.None; } }


        public static readonly DependencyProperty ActiveOverlayProperty =
                                                      DependencyProperty.Register("ActiveOverlay", typeof(UiOverlayType), typeof(MainWindow),
                                                      new PropertyMetadata(UiOverlayType.None, (o, arg) => { ((MainWindow)o).onSetActiveOverlay((UiOverlayType)arg.NewValue); }));

        internal UiOverlayType ActiveOverlay { get { return (UiOverlayType)GetValue(ActiveOverlayProperty); } set { SetValue(ActiveOverlayProperty, value); } }


        public static readonly DependencyProperty MenuOverlayTypeProperty =
                                                      DependencyProperty.Register("MenuOverlayType", typeof(UiOverlayType), typeof(MainWindow),
                                                      new PropertyMetadata(UiOverlayType.None));

        internal UiOverlayType MenuOverlayType { get { return (UiOverlayType)GetValue(MenuOverlayTypeProperty); } set { SetValue(MenuOverlayTypeProperty, value); } }


        
        private Point movePoint;
        private DispatcherTimer inactivityTimer;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(this);

            this.ActiveOverlay = UiOverlayType.SelectPatient;


            (this.inactivityTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.ApplicationIdle, 
                (e, a) => {
                    var idleTime = IdleTimeDetector.GetIdleTimeInfo();

                    if (idleTime.IdleTime.TotalMinutes >= 5)
                    {
                        this.ActiveOverlay = UiOverlayType.SelectPatient;
                    }
                }, this.Dispatcher)).Start();


            this.Loaded += (s, e) =>
            {
                Border border = (Border)VisualTreeHelper.GetChild(this.console, 0);
                ScrollViewer sv = border != null ? (ScrollViewer)VisualTreeHelper.GetChild(border, 0) : null;

                if (sv == null)
                    return;

                sv.ScrollChanged += (s1, e1) =>
                {
                    if (e1.ExtentHeightChange > 0.0)
                        ((ScrollViewer)e1.OriginalSource).ScrollToEnd();
                };
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_ToggleConsoleView(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.mainGrid.RowDefinitions[1].ActualHeight < 1)
                this.mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            else
                this.mainGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.movePoint = e.GetPosition(this);
            this.titleBar.CaptureMouse();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                Point pt = this.PointToScreen(e.GetPosition(this));

                this.Left = pt.X - this.movePoint.X;
                this.Top = pt.Y - this.movePoint.Y;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.titleBar.ReleaseMouseCapture();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tool"></param>
        private void focusTool(FrameworkElement tool)
        {
            if (this.overlayContent.Content != null)
            {                
                object dc = ((UserControl)this.overlayContent.Content).DataContext;

                if (dc is DependencyObject)
                    BindingOperations.ClearAllBindings((DependencyObject)dc);

                if (this.overlayContent.Content is IDisposable)
                    ((IDisposable)this.overlayContent.Content).Dispose();
            }

            this.overlayContent.Content = tool;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private void onSetActivePage(UiPageType pageType)
        {
            switch (pageType)
            {
                case UiPageType.None:
                    setContent(null);
                    break;
                case UiPageType.Default:
                    {
                        if (this.clientContent.Content == null)
                            this.ActivePage = UiPageType.SubstanceTest;
                    }
                    break;
                case UiPageType.DiagView:
                    setContent(new DiagModeView());
                    break;
                case UiPageType.SubstanceTest:
                    setContent(new SubstanceTestView());
                    break;
                case UiPageType.Report:
                    setContent(new ReportView());
                    break;
                case UiPageType.Backup:
                    this.saveSession();
                    setContent(new BackupView());
                    ((MainViewModel)this.DataContext).ActivePatient = null;
                    break;
                case UiPageType.PatientEditor:
                    setContent(new PatientEditor());
                    break;
                case UiPageType.SubstanceEditor:
                    setContent(new SubstanceEditor());
                    break;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiOverlayType"></param>
        private void onSetActiveOverlay(UiOverlayType uiOverlayType)
        {
            this.MenuOverlayType = UiOverlayType.MainMenu;

            switch (uiOverlayType)
            {
                case UiOverlayType.None:
                    focusTool(null);
                    break;
                case UiOverlayType.MainMenu:
                    {
                        MainMenu tool = new MainMenu();
                        tool.DataContext = this;
                        tool.HasActivePatient = hasActivePatient();

                        if (tool.HasActivePatient)
                        {
                            this.MenuOverlayType = UiOverlayType.None;
                        }
                        else
                        {
                            SetValue(ActivePageProperty, UiPageType.None);
                        }

                        focusTool(tool);
                    }
                    break;
                case UiOverlayType.SelectPatient:
                    {
                        saveSession();

                        SelectPatientBox tool = new SelectPatientBox();
                        DependencyObjectUtil.BindByName((DependencyObject)tool.DataContext, "ActivePatient", this.DataContext);
                        DependencyObjectUtil.BindByName((DependencyObject)tool, "ActivePage", this);
                        DependencyObjectUtil.BindByName((DependencyObject)tool, "ActiveOverlay", this);
         
                        focusTool(tool);
                    }
                    break;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void saveSession()
        {
            object oCmd = DependencyObjectUtil.GetValueByPOCOPropertyName(this.DataContext, "SaveSessionCommand");

            if (oCmd != null && oCmd is ICommand)
                ((ICommand)oCmd).Execute(null);
        }


        /// <summary>
        /// 
        /// </summary>
        private void setContent(UserControl content, bool fDefaultBindings = true)
        {
            if (this.clientContent.Content != null)
            {
                object dc = ((UserControl)this.clientContent.Content).DataContext;

                if (dc != null && dc != this.DataContext)
                {
                    if (dc is DependencyObject)
                        BindingOperations.ClearAllBindings((DependencyObject)dc);

                    if (dc is IDisposable)
                        ((IDisposable)dc).Dispose();
                }

                if (this.clientContent.Content is DependencyObject)
                    BindingOperations.ClearAllBindings((DependencyObject)this.clientContent.Content);

                if (this.clientContent.Content is IDisposable)
                    ((IDisposable)this.clientContent.Content).Dispose();
            }


            if (fDefaultBindings && content != null && content.DataContext != null && content.DataContext is DependencyObject)
            {
                DependencyObjectUtil.BindByName((DependencyObject)content.DataContext, "Meridians", this.DataContext, BindingMode.OneWay);
                DependencyObjectUtil.BindByName((DependencyObject)content.DataContext, "EavDevice", this.DataContext, BindingMode.OneWay);
                DependencyObjectUtil.BindByName((DependencyObject)content.DataContext, "ActivePatient", this.DataContext);
                DependencyObjectUtil.BindByName((DependencyObject)content.DataContext, "Status", this.DataContext, BindingMode.OneWay);
            }

            this.clientContent.Content = content;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool hasActivePatient()
        {
            return ((MainViewModel)this.DataContext).ActivePatient != null;
        }
    }
}
