using System.Collections.Generic;

namespace Gma.CodeCloud.Base.TextAnalyses.Blacklist
{
    public interface IBlacklist : IEnumerable<string>
    {
        bool Countains(string word);
        int Count { get; }
        void UnionWith(IBlacklist other);
    }
}
