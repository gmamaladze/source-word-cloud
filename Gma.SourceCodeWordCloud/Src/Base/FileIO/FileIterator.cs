using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Gma.CodeCloud.Base.TextAnalyses.Extractors;

namespace Gma.CodeCloud.Base.FileIO
{
    public class FileIterator
    {
        private readonly string m_IncludeFilesPattern;
        private readonly IProgressIndicator m_Progress;
        private readonly Regex m_ExcludeFoldersRegex;

        public FileIterator(string includeFilesPattern, string excludeFoldersPattern, IProgressIndicator progress)
        {
            m_IncludeFilesPattern = includeFilesPattern;
            m_Progress = progress;
            m_ExcludeFoldersRegex = Mask2RegEx(excludeFoldersPattern);
            m_Progress.Maximum = 100;
            m_Progress.SetMessage("Estimating ...");
        }

        private static Regex Mask2RegEx(string excludeFoldersPattern)
        {
            return new Regex(
                '^' +
                excludeFoldersPattern
                    .Replace(".", "[.]")
                    .Replace("*", ".*")
                    .Replace("?", ".")
                + '$',
                RegexOptions.IgnoreCase);
        }

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo directory, ref int estimatedCount)
        {
            m_Progress.Increment(0);
            FileInfo[] files = directory.GetFiles(m_IncludeFilesPattern);
            estimatedCount += files.Length;
            IEnumerable<FileInfo> result = files;
            IEnumerable<DirectoryInfo> subDirectories = GetDirectories(directory);
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                IEnumerable<FileInfo> subFiles = GetFiles(subDirectory, ref estimatedCount);
                result = result.Concat(subFiles);
            }
            return result;
        }

        public IEnumerable<DirectoryInfo> GetDirectories(DirectoryInfo directory)
        {
            return
                directory.GetDirectories()
                .Where(subDirectory => !m_ExcludeFoldersRegex.IsMatch(subDirectory.Name));
        }
    }
}
