using System;
using Gma.CodeCloud.Base.FileIO;

namespace Gma.CodeCloud.Base.Languages
{
    public static class ByLanguageFactory
    {
        public static FileIterator GetFileIterator(Language language, IProgressIndicator progress)
        {
            switch (language)
            {
                case Language.CSharp:
                    return new FileIterator("*.cs", "*test*", progress);

                case Language.Java:
                    return new FileIterator("*.java", "*test*", progress);

                case Language.VbNet:
                    return new FileIterator("*.vb", "*test*", progress);

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

                default:
                    ThrowNotSupportedLanguageException(languageName);
                    break;
            }
            return 0;
        }

        public static IWordExtractor GetWordExtractor(Language language, System.Collections.Generic.IEnumerable<System.IO.FileInfo> fileInfos, IProgressIndicator progress)
        {
            switch (language)
            {
                case Language.CSharp:
                    return new CSharpWordExtractor(fileInfos, progress);

                case Language.Java:
                    return new JavaWordExtractor(fileInfos, progress);

                case Language.VbNet:
                    return new VbWordExtractor(fileInfos, progress);

                default:
                    ThrowNotSupportedLanguageException(language);
                    break;
            }
            return null;
        }

        private const string s_CSharpBlacklistFileName = "CSharpBlacklist.txt";
        private const string s_JavaBlacklistFileName = "JavaBlacklist.txt";
        private const string s_VbNetBlacklistFileName = "VBNetBlacklist.txt";

        public static IBlacklist GetBlacklist(Language language)
        {
            string fileName = GetBlacklistFileName(language);
            return new TextFileBlacklist(fileName);
        }

        public static string GetBlacklistFileName(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return s_CSharpBlacklistFileName;

                case Language.Java:
                    return s_JavaBlacklistFileName;

                case Language.VbNet:
                    return s_VbNetBlacklistFileName;

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
