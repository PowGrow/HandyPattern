using System.Collections.Generic;

namespace HandyPattern.Models
{
    public interface IElement: ITitle,IOpenable,IControlable
    {
        public bool IsFolder { get;}

        public string? FlowDocumentXAML { get; }

        public List<IElement>? ContentTree { get; }

        public bool IsSubstitutionPattern { get; }
    }
}
