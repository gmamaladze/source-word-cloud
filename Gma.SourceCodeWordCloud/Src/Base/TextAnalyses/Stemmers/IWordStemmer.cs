namespace Gma.CodeCloud.Base.TextAnalyses.Stemmers
{
    public interface IWordStemmer
    {
        string GetStem(string word);
    }
}
