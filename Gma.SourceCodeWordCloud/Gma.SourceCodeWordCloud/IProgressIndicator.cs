namespace CodeWordCloud
{
    public interface IProgressIndicator
    {
        int Maximum { get; set; }
        void Increment(int value);
    }
}
