namespace Gma.CodeCloud.Base
{
    public interface IBlacklist
    {
        bool Countains(string word);
        void Add(string line);
        int Count { get; }
    }
}
