using System.Collections.Generic;
using System.IO;

namespace Gma.CodeCloud
{
    internal class TextFileBlacklist : IBlacklist
    {
        private readonly HashSet<string> m_BlacklistHasSet;

        public TextFileBlacklist(string fileName) : this(new FileInfo(fileName))
        {
        }

        public TextFileBlacklist(FileInfo fileInfo) : this(fileInfo.OpenText())
        {
        }

        public TextFileBlacklist(StreamReader reader)
        {
            m_BlacklistHasSet = new HashSet<string>();
            using (reader)
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    m_BlacklistHasSet.Add(line);
                    line = reader.ReadLine();
                }
            }
        }

        public bool Countains(string word)
        {
            return m_BlacklistHasSet.Contains(word);
        }
    }
}