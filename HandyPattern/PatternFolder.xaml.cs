using HandyPattern.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HandyPattern
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PatternFolder : UserControl,IFolder
    {
        private IWindowFactory _windowFactory;

        [JsonProperty]
        public string Title { get; private set; }
        [JsonProperty]
        public List<IElement> ContentTree { get; private set; }

        public PatternFolder(string title,List<IElement> content)
        {
            InitializeComponent();
            Title = title;
            folderNamePreview.Text = title;
            ContentTree = content;
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            _windowFactory = mainWindow.WindowFactory;
        }

        public void Open()
        {
            if (content.Visibility == Visibility.Collapsed)
            {
                content.Visibility = Visibility.Visible;
                folderNamePreview.Background = Brushes.Tan;
                
            }
            else
            {
                content.Visibility = Visibility.Collapsed;
                folderNamePreview.Background = Brushes.Beige;
            }
        }

        private void CreateFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            _windowFactory.OnNameFolderWindowClosed += (folder) =>
            {
                ContentTree.Add(folder);
                UpdateChildrenObject(ContentTree);
            };
            _windowFactory.CreateNameWindow(true);
        }

        private void CreatePatternButtonClicked(object sender, RoutedEventArgs e)
        {
            _windowFactory.OnNamePatternWindowClosed += (pattern) =>
            {
                ContentTree.Add(pattern);
                UpdateChildrenObject(ContentTree);
                pattern.Open();
            };
            _windowFactory.CreateNameWindow(false);
        }

        private void DeleteFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            
            //DELETING FOLDER ELEMENTS
        }

        public void ShowControls()
        {
            FolderControls.Visibility = Visibility.Visible;
        }

        public void HideControls()
        {
            FolderControls.Visibility = Visibility.Hidden;
        }

        private void ShowControls(object sender, MouseEventArgs e)
        {
            ShowControls();
        }

        private void HideControls(object sender, MouseEventArgs e)
        {
            HideControls();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateChildrenObject(ContentTree);
        }

        private void UpdateChildrenObject(List<IElement> contentTree)
        {
            contentStackPanel.Children.Clear();
            foreach (IElement element in contentTree)
            {
                if (!contentStackPanel.Children.Contains((UIElement)element))
                {
                    _ = element switch
                    {
                        IFolder => contentStackPanel.Children.Add((PatternFolder)element),
                        IPattern => contentStackPanel.Children.Add((Pattern)element),
                        _ => throw new System.NotImplementedException(),
                    };
                }
            }
        }

        private void folderNamePreview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((UIElement)Mouse.DirectlyOver == this.folderNamePreview)
                Open();
        }
    }
}
