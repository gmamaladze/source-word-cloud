using System;
using System.Collections.Generic;
using System.IO;

namespace Gma.CodeCloud.Base
{
    public class CommonBlacklist : IBlacklist
    {
        public CommonBlacklist() :  this(new string[] {})
        {
        }

        public CommonBlacklist(IEnumerable<string> excludedWords) 
            : this(excludedWords, StringComparer.InvariantCultureIgnoreCase)
        {
        }


        public static IBlacklist CreateFromTextFile(string fileName)
        {
            return CreateFromStremReader(new FileInfo(fileName).OpenText());
        }

        public static IBlacklist CreateFromStremReader(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            CommonBlacklist commonBlacklist = new CommonBlacklist();
            using (reader)
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    line.Trim();
                    commonBlacklist.Add(line);
                    line = reader.ReadLine();
                }
            }
            return commonBlacklist;
        }

        public CommonBlacklist(IEnumerable<string> excludedWords, StringComparer comparer)
        {
            m_ExcludedWordsHasSet = new HashSet<string>(excludedWords, comparer);   
        }

        private readonly HashSet<string> m_ExcludedWordsHasSet;

        public bool Countains(string word)
        {
            return m_ExcludedWordsHasSet.Contains(word);
        }

        public void Add(string line)
        {
            m_ExcludedWordsHasSet.Add(line);
        }

        public int Count
        {
            get { return m_ExcludedWordsHasSet.Count; }
        }
    }
}