using HandyPattern.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HandyPattern
{
    public partial class SubstitutionWindow : Window
    {
        private const int ROW_HEIGHT = 20;
        private const int NAME_COLUMN_ID = 0;
        private const int VALUE_COLUMN_ID = 1;

        private List<Substitution> substitutionList = new List<Substitution>();
        public SubstitutionWindow(List<Substitution> substitutions)
        {
            InitializeComponent();
            substitutionList = substitutions;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateSubstitutionGrid();
        }

        private void CreateSubstitutionGrid()
        {
            for (int index = 0; index < substitutionList.Count; index++)
            {
                AddRowDefinition(index);

                Label substitutionLabel = new Label();
                substitutionLabel.Name = $"substitutionLabel{index}";
                substitutionLabel.Content = substitutionList[index].Name;

                TextBox substitutionText = new TextBox();
                substitutionText.Name = $"substitutionText{index}";

                SetPositionInGrid(substitutionLabel, NAME_COLUMN_ID);
                SetPositionInGrid(substitutionText, VALUE_COLUMN_ID);
            }
        }

        private void SetPositionInGrid(UIElement uiElement, int columnId)
        {
            Grid.SetRow(uiElement, substitutionsGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(uiElement, columnId);
            substitutionsGrid.Children.Add(uiElement);
        }

        private void AddRowDefinition(int index)
        {
            GridLength rowHeight = new GridLength(ROW_HEIGHT);
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = rowHeight;
            rowDefinition.Name = $"substitutionRowDefinition{index}";
            substitutionsGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void SubstitutePatternValues()
        {
            int index = 0;
            foreach (UIElement element in substitutionsGrid.Children)
            {
                if (element.GetType() == typeof(TextBox))
                {
                    TextBox? textBox = element as TextBox;
                    substitutionList[index].Set(textBox.Text);
                    index++;
                }
            }
        }

        private void GrdHeaderPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            SubstitutePatternValues();
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                SubstitutePatternValues();
                this.Close();
            }
        }
    }
}
