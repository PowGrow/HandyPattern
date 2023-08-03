using HandyPattern.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace HandyPattern
{
    public partial class CreateElementName : Window
    {
        private MainWindow _mainWindow;
        private bool _isFolder;
        public event EventHandler<CreateElementNameClosedEventArgs> OnCreateElementNameClosed;
        public CreateElementName(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _isFolder = false;
            this.patternNameTextBlock.Focus();
        }

        public CreateElementName(MainWindow mainWindow,bool isFolder)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _isFolder = isFolder;
            this.patternNameTextBlock.Focus();
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && patternNameTextBlock.Text != string.Empty)
            {
                UIElement newElement;
                if(_isFolder)
                {
                    newElement = new PatternFolder(FirstCharToUpper(patternNameTextBlock.Text), new List<IElement>());
                    OnCreateElementNameClosed?.Invoke(this, new CreateElementNameClosedEventArgs(null, (PatternFolder)newElement));
                }
                else
                {
                    newElement = new Pattern(FirstCharToUpper(patternNameTextBlock.Text), Data.ConvertFlowDocument(new FlowDocument()), false);
                    OnCreateElementNameClosed?.Invoke(this, new CreateElementNameClosedEventArgs((Pattern)newElement,null));
                }
                this.Close();
            }
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

        public string FirstCharToUpper(string input)
        {
            if(string.IsNullOrEmpty(input))
                return string.Empty;

            Span<char> destination = stackalloc char[1];
            input.AsSpan(0, 1).ToUpperInvariant(destination);
            return $"{destination}{input.AsSpan(1)}";
        }
    }

    public class CreateElementNameClosedEventArgs : EventArgs
    {
        public Pattern? Pattern { get; set; }
        public PatternFolder? PatternFolder { get; set; }

        public CreateElementNameClosedEventArgs(Pattern? pattern, PatternFolder? patternFolder)
        {
            Pattern = pattern;
            PatternFolder = patternFolder;
        }
    }
}
