using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gma.CodeCloud.Base.TextAnalyses.Extractors;

namespace Gma.CodeCloud.Base.FileIO
{
    public class FileIterator
    {
        private readonly string m_IncludeFilesPattern;
        private readonly string m_ExcludeFoldersPattern;

        public FileIterator(string includeFilesPattern, string excludeFoldersPattern)
        {
            m_IncludeFilesPattern = includeFilesPattern;
            m_ExcludeFoldersPattern = excludeFoldersPattern;
        }


        public IEnumerable<string> GetFiles(string path)
        {
            string[] files = Directory.GetFiles(path, m_IncludeFilesPattern);

            IEnumerable<string> result = files;
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