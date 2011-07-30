using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gma.CodeCloud.Base.TextAnalyses.Extractors
{
    public abstract class BaseExtractor : IEnumerable<string>
    {
        protected BaseExtractor(IProgressIndicator progressIndicator)
        {
            ProgressIndicator = progressIndicator;
        }

        protected IProgressIndicator ProgressIndicator { get; set; }

        protected virtual IEnumerable<string> GetWordsInLine(string line)
        {
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
                        OnWordPorcessed(word);
                    }
                    word.Clear();
                }
                OnCharPorcessed(ch);
            }
            OnLinePorcessed(line);
        }

        protected virtual void OnCharPorcessed(char ch) { }
        protected virtual void OnWordPorcessed(StringBuilder word) { }
        protected virtual void OnLinePorcessed(string line) { }

        public IEnumerator<string> GetEnumerator()
        {
            return GetWords().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract IEnumerable<string> GetWords();
    }
}