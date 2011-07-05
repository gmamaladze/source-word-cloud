using System.Collections.Generic;

namespace Gma.CodeCloud.Base
{
    public interface IWordRegistry : IEnumerable<KeyValuePair<string, int>>
    {
        void AddOccurance(string word);
        void AddOccurances(string word, int increment);
        KeyValuePair<string, int>[] GetSortedByOccurances();
        int Count { get; }
    }
}
