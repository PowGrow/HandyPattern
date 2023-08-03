using System.Windows.Documents;

namespace HandyPattern.Models
{
    public interface IPattern : ITitle, IOpenable, IControlable
    {
        public string? FlowDocumentXAML { get; }

        public bool IsSubstitutionPattern { get;}
    }
}
