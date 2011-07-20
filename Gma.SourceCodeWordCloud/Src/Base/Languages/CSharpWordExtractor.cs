﻿using System.Collections.Generic;
using System.IO;

namespace Gma.CodeCloud.Base.Languages
{
    public class CSharpWordExtractor : WordExtractorBase
    {
        public CSharpWordExtractor(IEnumerable<FileInfo> files, IProgressIndicator progressIndicator) 
            : base(files, progressIndicator)
        {
        }

        protected override bool CanSkipFile(string line)
        {
            return
                line.Contains("[TestFixture]") ||
                line.Contains("Used version of") ||
                line.Contains("Windows Form Designer generated code") ||
                line.Contains("This code was generated by a tool.");
        }


        protected override string IgnoreRegionsAndUsings(string text)
        {
            if (text.StartsWith("using")) return string.Empty;
            if (text.StartsWith("namespace")) return string.Empty;
            if (text.StartsWith("#") & (text.Contains("region") || text.Contains("endregion"))) return string.Empty;
            return text;
        }
    }
}
