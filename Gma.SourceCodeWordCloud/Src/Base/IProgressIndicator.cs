namespace Gma.CodeCloud.Base
{
    public interface IProgressIndicator
    {
        int Maximum { get; set; }
        int Value { get; set; }
        void SetMessage(string text);
        void Increment(int value);
    }
}
