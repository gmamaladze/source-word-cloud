using System.Collections.Generic;

namespace Gma.CodeCloud.Base
{
    public interface IWordExtractor
    {
        IEnumerable<string> GetWords();
    }
}