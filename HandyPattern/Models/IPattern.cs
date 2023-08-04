namespace HandyPattern.Models
{
    public interface IPattern : IElement
    {
        public string FlowDocumentXAML { get; }

        public bool IsSubstitutionPattern { get;}
    }
}
