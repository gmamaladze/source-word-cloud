using System.Globalization;

namespace Gma.CodeCloud.Base.TextAnalyses.Stemmers
{
    public class LowerCaseStemmer : IWordStemmer
    {
        public string GetStem(string word)
        {
            return word.ToLower(CultureInfo.InvariantCulture);
        }
    }
}
