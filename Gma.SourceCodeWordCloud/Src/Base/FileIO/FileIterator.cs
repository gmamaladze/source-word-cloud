using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gma.CodeCloud.Base.TextAnalyses.Extractors;

namespace Gma.CodeCloud.Base.FileIO
{
    public class FileIterator
    {
        private readonly string[] m_IncludeFilesPatterns;
        private readonly string m_ExcludeFoldersPattern;

        public FileIterator(string includeFilesPattern, string excludeFoldersPattern)
            : this(new [] {includeFilesPattern}, excludeFoldersPattern)
        {
        }

        public FileIterator(string[] includeFilesPatterns, string excludeFoldersPattern)
        {
            m_IncludeFilesPatterns = includeFilesPatterns;
            m_ExcludeFoldersPattern = excludeFoldersPattern;
        }


        public IEnumerable<string> GetFiles(string path)
        {
            IEnumerable<string> result = new string[0];

            foreach (string pattern in m_IncludeFilesPatterns)
            {
                string[] files = Directory.GetFiles(path, pattern);
                result = result.Concat(files);
            }
           
            IEnumerable<string> subDirectories = GetDirectories(path);
            foreach (string subDirectory in subDirectories)
            {
                IEnumerable<string> subFiles = GetFiles(subDirectory);
                result = result.Concat(subFiles);
            }
            return result;
        }

        private IEnumerable<string> GetDirectories(string path)
        {
            return 
                Directory.EnumerateDirectories(path)
                .Where(subDirectory => !PatternMatcher.Matches(m_ExcludeFoldersPattern, subDirectory));
        }
    }
}