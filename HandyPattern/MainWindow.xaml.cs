using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using HandyPattern.Models;

namespace HandyPattern
{
    public partial class MainWindow : Window
    {
        private bool _isWorkspaceShowen;
        private DoubleAnimation? _mainWorkspaceCloseAnimation;
        private DoubleAnimation? _mainWorkspaceOpenAnimation;
        public ContentView ContentView { get; private set; }
        public IWindowFactory WindowFactory;

        public MainWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.Height;
            _isWorkspaceShowen = true;
            WindowFactory = new WindowFactory(this);
        }

        private void InitializeVisualElements()
        {
            ContentView = new ContentView();
            contentGrid.Children.Add(ContentView);
            //Temporary Topmost
            this.Topmost = true;
            //
            MinimizeButton.IsEnabled = true;
            CloseButton.IsEnabled = true;
        }
        void InitializeAnimations()
        {
            Animation mainWindowAnimations = new Animation(this);
            _mainWorkspaceCloseAnimation = mainWindowAnimations.CloseMainWorkspaceAnimation();
            _mainWorkspaceOpenAnimation = mainWindowAnimations.OpenMainWorkspaceAnimation();
        }
        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeVisualElements();
            InitializeAnimations();
            LoadPatterns();
        }
        private void LoadPatterns()
        {
            List<IElement> data = Data.DeserializeAndLoadCollection();
            if (data != null)
            {
                foreach (IElement model in data)
                {
                    if(model.IsFolder)
                    {
                        ContentView.Children().Add(new PatternFolder(model.Title,model.ContentTree));
                    }
                    else
                    {
                        ContentView.Children().Add(new Pattern(model.Title, model.FlowDocumentXAML, model.IsSubstitutionPattern));
                    }
                }
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<IElement> data = Data.CreateSerializeCollection(ContentView.Children());
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
                ContentView.Children().Add(pattern);
                pattern.Open();
            };
            WindowFactory.CreateNameWindow(false);
        }
        private void CreateFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            WindowFactory.OnNameFolderWindowClosed += (folder) =>
            {
                ContentView.Children().Add(folder);
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
