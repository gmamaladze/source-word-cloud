using System.IO;
using Gma.CodeCloud.Base.Languages;

namespace Gma.CodeCloud.Base.TextAnalyses.Extractors
{
    public class SingleFileExtractor : TextExtractor
    {

        public SingleFileExtractor(FileInfo fileInfo , IProgressIndicator progressIndicator)
            : base(new[] {fileInfo}, progressIndicator)
        {
        }

        protected override bool CanSkipFile(string line)
        {
            return false;
        }

        protected override void OnLinePorcessed(string line)
        {
            ProgressIndicator.Increment(1);
        }
    }
}
