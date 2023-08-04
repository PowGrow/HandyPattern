using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using HandyPattern.Models;

namespace HandyPattern
{
    public partial class MainWindow : Window
    {
        public ContentView ContentView { get; private set; }
        public IWindowFactory WindowFactory;

        public MainWindow()
        {
            InitializeComponent();
            ContentView = new ContentView();
            SizeToContent = SizeToContent.Height;
            WindowFactory = new WindowFactory(this);
        }

        private void InitializeVisualElements()
        {
            contentGrid.Children.Add(ContentView);
            //Temporary Topmost
            this.Topmost = true;
            //
            MinimizeButton.IsEnabled = true;
            CloseButton.IsEnabled = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeVisualElements();
            LoadPatterns();
        }
        private void LoadPatterns()
        {
            List<IElement> data = Data.DeserializeAndLoadCollection();
            if (data != null)
            {
                foreach (IElement model in data)
                {
                    if(model is IFolder)
                    {
                        IFolder folder = model as IFolder;
                        ContentView.Children.Add(new PatternFolder(folder.Title, folder.ContentTree));
                    }
                    else
                    {
                        IPattern pattern = model as IPattern;
                        ContentView.Children.Add(new Pattern(pattern.Title, pattern.FlowDocumentXAML, pattern.IsSubstitutionPattern));
                    }
                }
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<IElement> data = Data.CreateSerializeCollection(ContentView.Children);
            Data.SerializeAndSaveCollection(data);
        }
        private void DropDownButtonClicked(object sender, RoutedEventArgs e)
        {
            if (contentGrid.Visibility == Visibility.Visible)
                contentGrid.Visibility = Visibility.Collapsed;
            else
                contentGrid.Visibility = Visibility.Visible;
        }
        private void CreatePatternButtonClicked(object sender, RoutedEventArgs e)
        {
            WindowFactory.OnNamePatternWindowClosed += (pattern) =>
            {
                ContentView.Children.Add(pattern);
                pattern.Open();
            };
            WindowFactory.CreateNameWindow(false);
        }
        private void CreateFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            WindowFactory.OnNameFolderWindowClosed += (folder) =>
            {
                ContentView.Children.Add(folder);
            };
            WindowFactory.CreateNameWindow(true);
        }
        private void GrdHeaderPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void MinimizeButtonClicked(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
