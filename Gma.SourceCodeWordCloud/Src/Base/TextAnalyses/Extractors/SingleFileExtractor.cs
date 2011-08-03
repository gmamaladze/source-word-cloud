using System.IO;
using Gma.CodeCloud.Base.Languages;

namespace Gma.CodeCloud.Base.TextAnalyses.Extractors
{
    public class SingleFileExtractor : TextExtractor
    {

        public SingleFileExtractor(string file)
            : base(file)
        {
        }

        protected override bool CanSkipFile(string line)
        {
            return false;
        }
    }
}
