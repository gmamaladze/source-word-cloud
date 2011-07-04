using System.Collections.Generic;

namespace CodeWordCloud
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
                if (!m_Blacklist.IsListed(word))
                {
                    registry.AddOccurance(word);
                }
            }
            return registry;
        }
    }
}
