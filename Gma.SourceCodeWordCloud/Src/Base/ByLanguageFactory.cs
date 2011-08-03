using System;
using System.Collections.Generic;
using System.IO;
using Gma.CodeCloud.Base.FileIO;
using Gma.CodeCloud.Base.Languages;
using Gma.CodeCloud.Base.TextAnalyses.Blacklist;
using Gma.CodeCloud.Base.TextAnalyses.Extractors;
using Gma.CodeCloud.Base.TextAnalyses.Extractors.Code;
using Gma.CodeCloud.Base.TextAnalyses.Blacklist.En;
using Gma.CodeCloud.Base.TextAnalyses.Stemmers;
using Gma.CodeCloud.Base.TextAnalyses.Stemmers.En;

namespace Gma.CodeCloud.Base
{
    public static class ByLanguageFactory
    {
        public static FileIterator GetFileIterator(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return new FileIterator("*.cs", "*test*");

                case Language.Java:
                    return new FileIterator("*.java", "*test*");

                case Language.VbNet:
                    return new FileIterator("*.vb", "*test*");

                case Language.EnglishTxt:
                case Language.AnyTxt:
                    return new FileIterator("*.txt", string.Empty);


                default:
                    ThrowNotSupportedLanguageException(language);
                    break;
            }
            return null;
        }

        public static Language GetLanguageFromString(string languageName)
        {
            switch (languageName)
            {
                case "c#":
                    return Language.CSharp;

                case "Java":
                    return Language.Java;

                case "VB.NET":
                    return Language.VbNet;

                case "English *.txt":
                    return Language.EnglishTxt;

                case "Any *.txt":
                    return Language.AnyTxt;

                default:
                    ThrowNotSupportedLanguageException(languageName);
                    break;
            }
            return 0;
        }

        public static IEnumerable<string> GetWordExtractor(Language language, string  file)
        {
            switch (language)
            {
                case Language.CSharp:
                    return new CSharpExtractor(file);

                case Language.Java:
                    return new JavaExtractor(file);

                case Language.VbNet:
                    return new VbExtractor(file);

                case Language.EnglishTxt:
                case Language.AnyTxt:
                    return new TextExtractor(file);

                default:
                    ThrowNotSupportedLanguageException(language);
                    break;
            }
            return null;
        }

        private const string s_CSharpBlacklistFileName = "CSharpBlacklist.txt";
        private const string s_JavaBlacklistFileName = "JavaBlacklist.txt";
        private const string s_VbNetBlacklistFileName = "VBNetBlacklist.txt";
        private const string s_CustomBlacklistFileName = "CustomBlacklist.txt";

        public static IBlacklist GetBlacklist(Language language)
        {
            string fileName = GetBlacklistFileName(language);
            IBlacklist result = CommonBlacklist.CreateFromTextFile(fileName);

            if (language==Language.EnglishTxt)
            {
                result.UnionWith(new EnglishCommonWords());
            }
            return result;
        }

        public static string GetBlacklistFileName(Language language)
        {
            string result=string.Empty;
            switch (language)
            {
                case Language.CSharp:
                    result = s_CSharpBlacklistFileName;
                    break;

                case Language.Java:
                    result = s_JavaBlacklistFileName;
                    break;

                case Language.VbNet:
                    result = s_VbNetBlacklistFileName;
                    break;

                case Language.EnglishTxt:
                case Language.AnyTxt:
                    result = s_CustomBlacklistFileName;
                    break;

                default:
                    ThrowNotSupportedLanguageException(language);
                    break;
            }
            EnsureFileExists(result);
            return result;
        }

        private static void EnsureFileExists(string fileName)
        {
            if (!File.Exists(fileName))
            {
                using (StreamWriter writer = File.CreateText(fileName))
                {
                    writer.WriteLine("IgnoreMeOne");
                    writer.WriteLine("IgnoreMeTwo");
                }
            }
        }

        public static IWordStemmer GetStemmer(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                case Language.Java:
                case Language.VbNet:
                case Language.AnyTxt:
                    return new LowerCaseStemmer();

                case Language.EnglishTxt:
                    return new PorterStemmer();

                default:
                    ThrowNotSupportedLanguageException(language);
                    break;
            }
            return null;
        }


        private static void ThrowNotSupportedLanguageException(object language)
        {
            throw new NotSupportedException(string.Format("Language {0} is not supported.", language));
        }
    }
}
