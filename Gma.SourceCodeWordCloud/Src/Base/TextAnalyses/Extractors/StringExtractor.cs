using System.Collections.Generic;

namespace Gma.CodeCloud.Base.TextAnalyses.Extractors
{
    public sealed class StringExtractor : BaseExtractor
    {
        private readonly string m_Text;

        public StringExtractor(string text, IProgressIndicator progressIndicator)
            : base(progressIndicator)
        {
            m_Text = text;
            ProgressIndicator = progressIndicator;
            ProgressIndicator.Maximum = m_Text.Length;
        }

        public override IEnumerable<string> GetWords()
        {
            return GetWordsInLine(m_Text);
        }

        protected override void OnCharPorcessed(char ch)
        {
            ProgressIndicator.Increment(1);
        }
    }
}
