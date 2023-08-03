using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HandyPattern
{
    public partial class ContentView : UserControl,IDragSource
    {
        
        public ContentView()
        {
            InitializeComponent();
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        public UIElementCollection Children()
        {
            return contentStackPanel.Children;
        }

        public void DragCancelled()
        {
            
        }

        public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
            
        }

        public void Dropped(IDropInfo dropInfo)
        {
            
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            return false;
        }
    }
}
