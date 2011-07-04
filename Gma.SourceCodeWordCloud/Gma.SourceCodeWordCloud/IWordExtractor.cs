using System.Collections.Generic;

namespace CodeWordCloud
{
    public interface IWordExtractor
    {
        IEnumerable<string> GetWords();
    }
}