using GongSolutions.Wpf.DragDrop;
using HandyPattern.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace HandyPattern
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Pattern : UserControl, IElement, IDropTarget
    {
        
        private const int TEXT_PREVIEW_SYMBOLS = 200;
        private const string SUBSTITUTION_PATTERN = @"\{\w+?\}";
        private Action substitutionWorkDelegate;
        private bool IsBlocked = false;
        private IWindowFactory _windowFactory;

        [JsonProperty]
        public string Title { get; private set; }
        [JsonProperty]
        public string FlowDocumentXAML { get; set; }
        [JsonProperty]
        public bool IsSubstitutionPattern { get; private set; }
        [JsonProperty]
        public UIElement parentElement { get; private set; }
        public bool IsFolder { get; private set; }
        public List<IElement>? ContentTree { get; private set; }
        public List<Substitution>? PatternSubstitutions { get; private set; }
        public FlowDocument FlowDocument
        {
            get { return Data.ConvertXmalString(FlowDocumentXAML); }
            set { FlowDocumentXAML = Data.ConvertFlowDocument(value); }
        }

        public Pattern(string title,string flowDocumentXAML, bool isSubstitutionPattern)
        {
            InitializeComponent();
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            _windowFactory = mainWindow.WindowFactory;
            IsFolder = false;
            IsSubstitutionPattern = isSubstitutionPattern;
            FlowDocumentXAML = flowDocumentXAML;
            Title = title;
            substitutionWorkDelegate += SearchForSubstitution;
            if (IsSubstitutionPattern) SearchForSubstitution();
            UpdatePatternName();
        }

        public void SetName(string patternName)
        {
            this.Title = patternName;
        }


        public void UpdatePatternPreview()
        {
            try
            { 
                if(FlowDocumentXAML != null)
                {
                    var flowDocument = FlowDocument;
                    var textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                    textRange.Select(flowDocument.ContentStart, flowDocument.ContentEnd);
                    if(textRange.Text.Length > TEXT_PREVIEW_SYMBOLS)
                        textRange.Select(flowDocument.ContentStart, flowDocument.ContentStart.GetPositionAtOffset(TEXT_PREVIEW_SYMBOLS));
                    //patternNamePreview.Document.Blocks.Clear();
                    //this.patternNamePreview.Document.Blocks.Add(new Paragraph(new Run(textRange.Text + ".....")));
                }
            }
            catch(ArgumentNullException)
            {
                //patternNamePreview.Document.Blocks.Clear();
            }
        }

        public void UpdatePatternName()
        {
            patternNamePreview.Text = Title;
        }

        private void ShowEditButton(object sender, MouseEventArgs e)
        {
            ShowControls();
        }

        private void EditButtonClicked(object sender, RoutedEventArgs e)
        {
            Open();
        }

        public void Open()
        {
            if (!IsBlocked)
            {
                _windowFactory.OnEditWindowClosed += EditWindowClosed;
                _windowFactory.CreateEditWindow(FlowDocument, Title, this);
            }
        }

        public async void EditWindowClosed()
        {
            await SearchForSubstitutionAsync();
            UpdatePatternName();
            _windowFactory.OnEditWindowClosed -= EditWindowClosed;
        }
        public void ShowControls()
        {
            PatternControls.Visibility = Visibility.Visible;
        }

        public void HideControls()
        {
            PatternControls.Visibility = Visibility.Hidden;
        }

        private void MouseRightButtonUpClicked(object sender, MouseButtonEventArgs e)
        {
            if(IsSubstitutionPattern && PatternSubstitutions != null)
            {
                _windowFactory.OnSubstitutionWindowClosed += SubstitutionWindowClosed;
                _windowFactory.CreateSubstitutionWindow(PatternSubstitutions);
            }
            else
            {
                FlowDocument flowDocument = FlowDocument;
                var textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                Clipboard.SetText(textRange.Text, TextDataFormat.UnicodeText);
            }
        }

        private void SubstitutionWindowClosed()
        {
            string resultText = GetResultPatternText(FlowDocument);
            Clipboard.SetText(resultText.ToString());
            _windowFactory.OnSubstitutionWindowClosed -= SubstitutionWindowClosed;
        }

        private string GetResultPatternText(FlowDocument document)
        {
            try
            {
                var patternTextRange = new TextRange(document.ContentStart, document.ContentEnd);
                string[] splitedPatternText = Regex.Split(patternTextRange.Text, SUBSTITUTION_PATTERN);
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i < splitedPatternText.Length - 1; i++)
                {
                    resultText.Append(splitedPatternText[i]);
                    if (PatternSubstitutions[i].Value != null)
                        resultText.Append(PatternSubstitutions[i].Value);
                    else
                        resultText.Append(PatternSubstitutions[i].Name);
                }
                return resultText.ToString();
            }
            catch(NullReferenceException)
            {
                return "";
            } 
        }

        private void ShowControls(object sender, MouseEventArgs e)
        {
            ShowControls();
        }

        private void HideControls(object sender, MouseEventArgs e)
        {
            HideControls();
        }

        private void DeletePatternButtonClicked(object sender, RoutedEventArgs e)
        {
            var stackPanel = (StackPanel)this.Parent;
            stackPanel.Children.Remove(this);
        }

        private async Task SearchForSubstitutionAsync()
        {
            await Task.Run(substitutionWorkDelegate);
        }

        private void SearchForSubstitution()
        {
            PatternSubstitutions = new List<Substitution>();
            IsBlocked = true;
            FlowDocument flowDocument = FlowDocument;
            string patternText = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd).Text;
            var substitutions = Regex.Matches(patternText, SUBSTITUTION_PATTERN).ToList();
            if(substitutions.Count != 0)
            {
                IsSubstitutionPattern = true;
                foreach (Match substitution in substitutions)
                {
                    var name = substitution.Value.Substring(1, substitution.Value.Length - 2);
                    Substitution data = new Substitution(name, substitution.Value);
                    PatternSubstitutions.Add(data);
                }
            }
            IsBlocked = false;
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            //WIP
        }
    }
}
