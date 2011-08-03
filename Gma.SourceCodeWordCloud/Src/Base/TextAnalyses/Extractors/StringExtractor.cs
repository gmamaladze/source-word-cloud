using System.Collections.Generic;

namespace Gma.CodeCloud.Base.TextAnalyses.Extractors
{
    public sealed class StringExtractor : BaseExtractor
    {
        private readonly string m_Text;

        public StringExtractor(string text)
            : base()
        {
            m_Text = text;
        }

        public override IEnumerable<string> GetWords()
        {
            return GetWordsInLine(m_Text);
        }
    }
}
