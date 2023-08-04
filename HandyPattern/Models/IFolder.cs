using System.Collections.Generic;

namespace HandyPattern.Models
{
    public interface IFolder: IElement
    {
        public List<IElement> ContentTree { get; }
    }
}
