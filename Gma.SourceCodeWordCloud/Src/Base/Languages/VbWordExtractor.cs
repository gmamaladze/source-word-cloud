﻿using System.Collections.Generic;
using System.IO;

namespace Gma.CodeCloud.Base.Languages
{
    public class VbWordExtractor : WordExtractorBase
    {
        private const string s_VbSinglelineCommentPrefix = "'";

        public VbWordExtractor(IEnumerable<FileInfo> files, IProgressIndicator progressIndicator) 
            : base(files, progressIndicator)
        {
        }

        protected override string SinglelineCommentPrefix
        {
            get { return s_VbSinglelineCommentPrefix; }
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
            if (text.StartsWith("Imports")) return string.Empty;
            if (text.StartsWith("Namespace")) return string.Empty;
            if (text.StartsWith("#") & (text.Contains("region") || text.Contains("endregion"))) return string.Empty;
            return text;
        }
    }
}