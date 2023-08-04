using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HandyPattern
{
    public partial class ContentView : UserControl
    {
        public UIElementCollection Children
        {
            get
            {
                return contentStackPanel.Children;
            }
        }
        
        public ContentView()
        {
            InitializeComponent();
        }
    }
}
