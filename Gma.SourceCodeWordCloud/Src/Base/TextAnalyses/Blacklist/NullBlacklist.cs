using System;
using System.Collections;
using System.Collections.Generic;

namespace Gma.CodeCloud.Base.TextAnalyses.Blacklist
{
    public class NullBlacklist : IBlacklist
    {
        public bool Countains(string word)
        {
            return false;
        }

        public int Count
        {
            get { return 0; }
        }

        public void UnionWith(IBlacklist other)
        {
            
        }

        public IEnumerator<string> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
