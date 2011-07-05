using System.Collections.Generic;

namespace Gma.CodeCloud
{
    public class WordCounter
    {
        private readonly IBlacklist m_Blacklist;

        public WordCounter(IBlacklist blacklist)
        {
            m_Blacklist = blacklist;
        }

        public IWordRegistry Count(IWordExtractor extractor)
        {
            WordRegistry registry = new WordRegistry();
            IEnumerable<string> words = extractor.GetWords();
            foreach (string word in words)
            {
                if (!m_Blacklist.Countains(word))
                {
                    registry.AddOccurance(word);
                }
            }
            return registry;
        }
    }
}
