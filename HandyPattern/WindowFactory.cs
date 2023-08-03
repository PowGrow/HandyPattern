using HandyPattern.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace HandyPattern
{
    public class WindowFactory : IWindowFactory
    {
        public event Action? OnEditWindowClosed;
        public event Action<Pattern>? OnNamePatternWindowClosed;
        public event Action<PatternFolder>? OnNameFolderWindowClosed;
        public event Action? OnSubstitutionWindowClosed;

        private MainWindow _mainWindow;
        public WindowFactory(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public void CreateNameWindow(bool isFolder)
        {
            CreateElementName createNameWindow = new CreateElementName(_mainWindow, isFolder);
            if (isFolder)
                ShowFolderNameWindow(OnNameFolderWindowClosed, createNameWindow);
            else
                ShowPatternNameWindow(OnNamePatternWindowClosed, createNameWindow);
        }

        public void CreateEditWindow(FlowDocument flowDocument,string title, Pattern patternView)
        {
            EditWindow editWindow;
            if (flowDocument != null)
                editWindow = new EditWindow(title, flowDocument, patternView);
            else
                editWindow = new EditWindow(title, patternView);
            ShowWindow(OnEditWindowClosed, editWindow);
        }

        public void CreateSubstitutionWindow(List<Substitution> patternSubstitutions)
        {
            SubstitutionWindow substitutionWindow = new SubstitutionWindow(patternSubstitutions);
            ShowWindow(OnSubstitutionWindowClosed, substitutionWindow);
        }

        private void ShowWindow(Action? eventOnClosed, Window window)
        {
            Window ownerWindow = _mainWindow;
            window.Owner = ownerWindow;
            window.Closed += (sender, e) => eventOnClosed?.Invoke();
            window.Show();
        }

        private void ShowPatternNameWindow (Action<Pattern>? eventArgOnClosed, CreateElementName window)
        {
            Window ownerWindow = _mainWindow;
            window.Owner = ownerWindow;
            window.OnCreateElementNameClosed += (sender, e) => eventArgOnClosed?.Invoke(e.Pattern);
            window.Show();
        }

        private void ShowFolderNameWindow(Action<PatternFolder>? eventArgOnClosed, CreateElementName window)
        {
            Window ownerWindow = _mainWindow;
            window.Owner = ownerWindow;
            window.OnCreateElementNameClosed += (sender, e) => eventArgOnClosed?.Invoke(e.PatternFolder);
            window.Show();
        }
    }
}
