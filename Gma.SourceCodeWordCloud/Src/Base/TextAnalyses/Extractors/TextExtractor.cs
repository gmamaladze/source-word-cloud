using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Gma.CodeCloud.Base.TextAnalyses.Extractors;

namespace Gma.CodeCloud.Base.Languages
{
    public class TextExtractor : BaseExtractor
    {
        private readonly string m_File;

        public TextExtractor(string file)
            : base()
        {
            m_File = file;
        }


        public override IEnumerable<string> GetWords()
        {
            using (StreamReader reader = File.OpenText(m_File))
            {
                IEnumerable<string> words = GetWords(reader);
                foreach (string word in words)
                {
                    yield return word;
                }
            }
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
                    }
                    word.Clear();
                }
            }
        }

        protected virtual bool CanSkipFile(string line)
        {
            return false;
        }
    }
}
