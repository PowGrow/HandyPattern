namespace HandyPattern.Models
{
    public class Substitution
    {
        public string Name { get; private set; }
        public string String { get; private set; }
        public string Value { get; private set; }

        public Substitution(string name, string @string)
        {
            Name = name;
            String = @string;
        }

        public void Set(string value)
        {
            Value = value;
        }
    }
}
