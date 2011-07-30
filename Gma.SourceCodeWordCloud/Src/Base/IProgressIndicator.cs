namespace Gma.CodeCloud.Base.TextAnalyses.Extractors
{
    public interface IProgressIndicator
    {
        int Maximum { get; set; }
        void Increment(int value);
        void SetMessage(string text);
    }
}
