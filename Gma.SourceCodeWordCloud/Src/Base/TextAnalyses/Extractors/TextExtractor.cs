using System.Collections.Generic;
using System.IO;
using System.Text;
using Gma.CodeCloud.Base.TextAnalyses.Extractors;

namespace Gma.CodeCloud.Base.Languages
{
    public class TextExtractor : BaseExtractor
    {
        public TextExtractor(IEnumerable<FileInfo> files, IProgressIndicator progressIndicator)
            : base(progressIndicator)
        {
            Files = files;
        }

        protected IEnumerable<FileInfo> Files { get; set; }

        public override IEnumerable<string> GetWords()
        {
            foreach (FileInfo fileInfo in Files)
            {
                ProgressIndicator.SetMessage(Shorten(fileInfo.FullName, 60));
                using (StreamReader reader = fileInfo.OpenText())
                {
                    IEnumerable<string> words = GetWords(reader);
                    foreach (string word in words)
                    {
                        yield return word;
                    }
                    OnFileProcessed();
                }
            }   
        }

        private void OnFileProcessed()
        {
            ProgressIndicator.Increment(1);
        }

        protected static string Shorten(string fullFileName, int maxLength)
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

        public virtual IEnumerable<string> GetWords(StreamReader reader)
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

        protected override IEnumerable<string> GetWordsInLine(string line)
        {
            StringBuilder word = new StringBuilder();
            foreach (char ch in line)
            {
                if (char.IsLetter(ch))
                {
                    word.Append(ch);
                }
                else
                {
                    if (word.Length > 1)
                    {
                        yield return word.ToString();
                        OnWordPorcessed(word);
                    }
                    word.Clear();
                }
                OnCharPorcessed(ch);
            }
            OnLinePorcessed(line);
        }

        protected virtual bool CanSkipFile(string line)
        {
            return false;
        }
    }
}
