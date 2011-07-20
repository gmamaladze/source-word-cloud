using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gma.CodeCloud.Base.Languages
{
    public abstract class WordExtractorBase : IWordExtractor
    {
        private const string s_SinglelineCommentPrefix = @"//";
        private const string s_MultilineCommentSuffix = @"*/";
        private const string s_MultilineCommentPrefix = @"/*";
        private bool m_IsCommentMode;

        private readonly IEnumerable<FileInfo> m_Files;
        private readonly IProgressIndicator m_ProgressIndicator;

        protected WordExtractorBase(IEnumerable<FileInfo> files, IProgressIndicator progressIndicator)
        {
            m_Files = files;
            m_ProgressIndicator = progressIndicator;
        }

        public IEnumerable<string> GetWords()
        {
            foreach (FileInfo fileInfo in m_Files)
            {
                m_ProgressIndicator.SetMessage(Shorten(fileInfo.FullName, 60));
                using (StreamReader reader = fileInfo.OpenText())
                {
                    IEnumerable<string> words = GetWords(reader);
                    foreach (string word in words)
                    {
                        yield return word;
                    }
                    m_ProgressIndicator.Increment(1);
                }
            }   
        }

        private static string Shorten(string fullFileName, int maxLength)
        {
            if (fullFileName.Length<=maxLength)
            {
                return fullFileName;
            }

            int partLength = maxLength/2 - 2;

            return string.Concat(
                fullFileName.Remove(partLength),
                "...",
                fullFileName.Substring(fullFileName.Length - partLength));
        }

        public IEnumerable<string> GetWords(StreamReader reader)
        {
            string line = reader.ReadLine();
            while (line != null)
            {   
                if (CanSkipFile(line))
                {
                    break;
                }

                IEnumerable<string> wordsInLine = GetWordsInLine(line);
                foreach (string word in wordsInLine)
                {
                    yield return word;
                }
                line = reader.ReadLine();
            }
        }

        protected abstract bool CanSkipFile(string line);

        private IEnumerable<string> GetWordsInLine(string line)
        {
            line = Normalize(line);
            StringBuilder word = new StringBuilder();
            foreach (char ch in line)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    word.Append(ch);
                }
                else
                {
                    if (word.Length > 1)
                    {
                        yield return word.ToString();
                    }
                    word.Clear();
                }
            }
        }

        protected string Normalize(string text)
        {
            string result = text.Trim();
            result = RemoveMultiLineComment(result);
            result = RemoveSingleLineComment(result);
            result = RemoveLiterals(result);
            result = IgnoreRegionsAndUsings(result);

            return result;
        }

        protected string RemoveLiterals(string text)
        {
            string result = text;
            int indexOfStart = result.IndexOf("\"");
            while (!(indexOfStart < 0) && indexOfStart + 1 < text.Length)
            {
                int indexOfEnd = result.IndexOf("\"", indexOfStart + 1);
                if (indexOfEnd < 0)
                {
                    break;
                }
                result = result.Remove(indexOfStart, indexOfEnd - indexOfStart + 1);
                indexOfStart = result.IndexOf("\"");
            }
            return result;
        }

        protected abstract string IgnoreRegionsAndUsings(string text);

        private string RemoveMultiLineComment(string text)
        {
            if (text.Length == 0)
            {
                return text;
            }

            int indexOfStart = 0;
            int indexOfEnd = text.Length;
            if (!m_IsCommentMode)
            {
                indexOfStart = text.IndexOf(s_MultilineCommentPrefix);
                if (indexOfStart < 0)
                {
                    return text;
                }
                m_IsCommentMode = true;
            }

            if (m_IsCommentMode)
            {
                indexOfEnd = text.IndexOf(s_MultilineCommentSuffix);
                if (indexOfEnd < 0)
                {
                    return text.Remove(indexOfStart);
                }
                m_IsCommentMode = false;
            }


            return text.Remove(indexOfStart, indexOfEnd - indexOfStart + 2);
        }

        protected virtual string SinglelineCommentPrefix
        {
            get { return s_SinglelineCommentPrefix; }
        }

        protected string RemoveSingleLineComment(string text)
        {
            int indexOfStart = text.IndexOf(SinglelineCommentPrefix);
            if (indexOfStart < 0)
            {
                return text;
            }
            string result = text.Substring(0, indexOfStart);
            return result;
        }
    }
}