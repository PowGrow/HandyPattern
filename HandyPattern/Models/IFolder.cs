using System.Collections.Generic;

namespace HandyPattern.Models
{
    public interface IFolder: ITitle,IOpenable,IControlable
    {
        public List<ITitle> Content { get; }
    }
}
