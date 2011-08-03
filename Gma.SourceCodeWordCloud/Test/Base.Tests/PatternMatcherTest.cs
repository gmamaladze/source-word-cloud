using Gma.CodeCloud.Base.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Base.Tests
{


    [TestClass]
    public class PatternMatcherTest
    {
        [TestMethod]
        public void Empty_expresstion_does_not_match()
        {
            const string expression = "";
            const string name = "SomeFile.txt";
            Assert.IsFalse(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Empty_expresstion_does_not_match_empty_input()
        {
            const string expression = "";
            const string name = "";
            Assert.IsFalse(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void asterisk_expresstion_does_not_match_empty_input()
        {
            const string expression = "*";
            const string name = "";
            Assert.IsFalse(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Whole_expresstion_without_wildcards_exact_matches()
        {
            const string expression = "SomeFile.txt";
            const string name = "SomeFile.txt";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Whole_expresstion_without_wildcards_different_case1_matches()
        {
            const string expression = "SomeFile.txt";
            const string name = "SOMEFILE.TXT";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Whole_expresstion_without_wildcards_different_case2_matches()
        {
            const string expression = "SOMEFILE.TXT";
            const string name = "SomeFile.txt";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Whole_expresstion_without_wildcards_different_case3_matches()
        {
            const string expression = "SomeFile.txt";
            const string name = "sOMEfILE.TXT";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Whole_expresstion_without_wildcards_does_not_match()
        {
            const string expression = "SomeOtherFile.txt";
            const string name = "SomeFile.txt";
            Assert.IsFalse(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void asterisk_matches_any_non_empty_input()
        {
            const string expression = "*";
            const string name = "SomeFile.txt";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void asterisk_dot_asterisk_matches_any_non_empty_input()
        {
            const string expression = "*.*";
            const string name = "SomeFile";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Prefix_plus_asterisk_matches()
        {
            const string expression = "Some*";
            const string name = "SomeFile.txt";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void Prefix_plus_asterisk_matches_not()
        {
            const string expression = "Some*";
            const string name = "SomZZZFile.txt";
            Assert.IsFalse(PatternMatcher.Matches(expression, name));
        }


        [TestMethod]
        public void asterisk_plus_suffix_matches()
        {
            const string expression = "*.txt";
            const string name = "SomeFile.txt";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void asterisk_plus_suffix_matches_same_string()
        {
            const string expression = "*something";
            const string name = "something";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void asterisk_plus_suffix_matches_any_char_plus_string()
        {
            const string expression = "*something";
            const string name = "Asomething";
            Assert.IsTrue(PatternMatcher.Matches(expression, name));
        }

        [TestMethod]
        public void asterisk_plus_suffix_matches_not()
        {
            const string expression = "*.pdf";
            const string name = "SomZZZFile.txt";
            Assert.IsFalse(PatternMatcher.Matches(expression, name));
        }
    }
}
