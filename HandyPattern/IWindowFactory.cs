using HandyPattern.Models;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace HandyPattern
{
    public interface IWindowFactory
    {
        public event Action OnEditWindowClosed;
        public event Action<Pattern> OnNamePatternWindowClosed;
        public event Action<PatternFolder> OnNameFolderWindowClosed;
        public event Action OnSubstitutionWindowClosed;

        public void CreateNameWindow(bool isFolder);
        public void CreateEditWindow(FlowDocument flowDocument, string title, Pattern patternView);
        public void CreateSubstitutionWindow(List<Substitution> patternSubstitutions);

    }
}
