using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace HandyPattern
{
    public partial class EditWindow : Window
    {
        private FlowDocument? _tempPatternDocument;
        private Pattern _patternPreviewInstance;
        public EditWindow(string patternName, FlowDocument? patternDocument, Pattern patternPreview)
        {
            _patternPreviewInstance = patternPreview;
            _tempPatternDocument = patternDocument;
            InitializeComponent();
            this.patternName.Text = patternName;
            this.richTextBox.Focus();
        }

        public EditWindow(string patternName, Pattern patternPreview)
        {
            _patternPreviewInstance = patternPreview;
            InitializeComponent();
            this.patternName.Text = patternName;
            this.richTextBox.Focus();
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
            TryToCloseWindow();

            //ADD NAME ERROR ANIMATION
        }

        private void TryToCloseWindow()
        {
            if (this.patternName.Text != "")
            {
                _patternPreviewInstance.SetName(this.patternName.Text);
                _patternPreviewInstance.FlowDocument = richTextBox.Document;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(_tempPatternDocument != null)
                CopyFlowDocument(_tempPatternDocument);
        }

        private void CopyFlowDocument(FlowDocument tempPatternDocument)
        {
            TextRange range = new TextRange(tempPatternDocument.ContentStart, tempPatternDocument.ContentEnd);
            MemoryStream stream = new MemoryStream();
            System.Windows.Markup.XamlWriter.Save(range, stream);
            range.Save(stream, DataFormats.XamlPackage);
            TextRange range2 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            range2.Load(stream, DataFormats.XamlPackage);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                TryToCloseWindow();
        }
    }
}
