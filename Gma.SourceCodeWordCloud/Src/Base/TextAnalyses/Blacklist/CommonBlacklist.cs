using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Gma.CodeCloud.Base.TextAnalyses.Blacklist
{
    public class CommonBlacklist : IBlacklist
    {
        private readonly HashSet<string> m_ExcludedWordsHashSet;

        public CommonBlacklist() :  this(new string[] {})
        {
        }

        public CommonBlacklist(IEnumerable<string> excludedWords) 
            : this(excludedWords, StringComparer.InvariantCultureIgnoreCase)
        {
        }


        public static IBlacklist CreateFromTextFile(string fileName)
        {
            return 
                !File.Exists(fileName) 
                    ? new NullBlacklist() 
                    : CreateFromStremReader(new FileInfo(fileName).OpenText());
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
            m_ExcludedWordsHashSet = new HashSet<string>(excludedWords, comparer);   
        }

        public bool Countains(string word)
        {
            return m_ExcludedWordsHashSet.Contains(word);
        }

        public void Add(string word)
        {
            m_ExcludedWordsHashSet.Add(word);
        }

        public int Count
        {
            get { return m_ExcludedWordsHashSet.Count; }
        }

        public void UnionWith(IBlacklist other)
        {
            this.m_ExcludedWordsHashSet.UnionWith(other);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.m_ExcludedWordsHashSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}