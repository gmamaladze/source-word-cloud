using System.Collections.Generic;

namespace Gma.CodeCloud
{
    public interface IWordExtractor
    {
        IEnumerable<string> GetWords();
    }
}