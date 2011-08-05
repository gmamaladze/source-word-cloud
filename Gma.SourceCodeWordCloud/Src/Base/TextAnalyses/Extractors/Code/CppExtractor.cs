namespace Gma.CodeCloud.Base.TextAnalyses.Extractors.Code
{
    public class CppExtractor : CodeExtractorBase
    {
        public CppExtractor(string file) 
            : base(file)
        {
        }

        protected override bool CanSkipFile(string line)
        {
            return false;
        }


        protected override string IgnoreRegionsAndUsings(string text)
        {
            if (text.StartsWith("#include")) return string.Empty;
            return text;
        }
    }
}
