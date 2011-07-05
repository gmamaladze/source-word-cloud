namespace Gma.CodeCloud
{
    public interface IProgressIndicator
    {
        int Maximum { get; set; }
        void Increment(int value);
    }
}
