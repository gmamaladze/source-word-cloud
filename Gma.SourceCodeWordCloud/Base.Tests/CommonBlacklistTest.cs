using System.IO;
using System.Text;
using Gma.CodeCloud.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Base.Tests
{
    [TestClass()]
    public class CommonBlacklist_Construction_Tests
    {
        [TestMethod()]
        public void Newly_created_blacklist_is_empty()
        {
            CommonBlacklist target = new CommonBlacklist();
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod()]
        public void Newly_created_blacklist_using_array_parameter_has_appropriate_words()
        {
            var inputArray = new[] { "public", "void", "MethodName" };
            CommonBlacklist target = new CommonBlacklist(inputArray);
            foreach (var inputWord in inputArray)
            {
                Assert.IsTrue(target.Countains(inputWord));
            }
        }

        [TestMethod()]
        public void A_blacklist_constructed_without_StringComparer_parameter_is_case_insensitive()
        {
            var inputArray = new[] { "publiC", "Void", "MethodName" };
            CommonBlacklist target = new CommonBlacklist(inputArray);

            foreach (var exactMatchWord in inputArray)
            {
                Assert.IsTrue(target.Countains(exactMatchWord));
            }

            Assert.IsTrue(target.Countains("PuBlic"));
            Assert.IsTrue(target.Countains("public"));
            Assert.IsTrue(target.Countains("PUBLIC"));

            Assert.IsTrue(target.Countains("VoiD"));
            Assert.IsTrue(target.Countains("void"));
            Assert.IsTrue(target.Countains("VOID"));

            Assert.IsTrue(target.Countains("mETHODnAME"));
            Assert.IsTrue(target.Countains("methodname"));
            Assert.IsTrue(target.Countains("METHODNAME"));
        }

        [TestMethod()]
        public void A_blacklist_constructed_with_explicit_case_aware_StringComparer_is_case_sensitive()
        {
            var inputArray = new[] { "publiC", "Void", "MethodName" };
            CommonBlacklist target = new CommonBlacklist(inputArray, StringComparer.InvariantCulture);

            foreach (var exactMatchWord in inputArray)
            {
                Assert.IsTrue(target.Countains(exactMatchWord));
            }

            Assert.IsFalse(target.Countains("PuBlic"));
            Assert.IsFalse(target.Countains("public"));
            Assert.IsFalse(target.Countains("PUBLIC"));

            Assert.IsFalse(target.Countains("VoiD"));
            Assert.IsFalse(target.Countains("void"));
            Assert.IsFalse(target.Countains("VOID"));

            Assert.IsFalse(target.Countains("mETHODnAME"));
            Assert.IsFalse(target.Countains("methodname"));
            Assert.IsFalse(target.Countains("METHODNAME"));
        }

        [TestMethod()]
        public void Stream_containing_words_in_lines_results_blacklist_containing_same_words()
        {
            var inputArray = new[] { "public", "void", "MethodName" };
            var lineBrakeDelimitedString = string.Join(Environment.NewLine, inputArray);
            var stringReader = new StringReader(lineBrakeDelimitedString);

            IBlacklist target = CommonBlacklist.CreateFromStremReader(stringReader);

            Assert.IsInstanceOfType(target, typeof(CommonBlacklist));
            foreach (var inputWord in inputArray)
            {
                Assert.IsTrue(target.Countains(inputWord));
            }
            Assert.AreEqual(inputArray.Length, target.Count);
        }
    }

    [TestClass()]
    public class CommonBlacklist_Contains_Tests
    {
        [TestMethod()]
        public void Contains_returns_true_for_words_added()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add("public");
            target.Add("void");
            
            Assert.IsTrue(target.Countains("public"));
            Assert.IsTrue(target.Countains("void"));
        }

        [TestMethod()]
        public void Contains_returns_false_for_words_not_added()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add("public");
            target.Add("void");

            Assert.IsFalse(target.Countains("private"));
            Assert.IsFalse(target.Countains("int"));
        }

        [TestMethod()]
        public void Contains_of_empty_blacklist_returns_false_for_all_words()
        {
            CommonBlacklist target = new CommonBlacklist();

            Assert.IsFalse(target.Countains("private"));
            Assert.IsFalse(target.Countains("int"));
            Assert.IsFalse(target.Countains("public"));
            Assert.IsFalse(target.Countains("void"));
        }
    }

    [TestClass()]
    public class CommonBlacklist_Add_Tests
    {
        [TestMethod()]
        public void Contains_returns_true_for_identical_word_added_twice()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add("private");
            target.Add("private");
            Assert.IsTrue(target.Countains("private"));
        }

        [TestMethod()]
        public void Contains_returns_true_for_case_different_same_word_added_twice()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add("PriVate");
            target.Add("pRIVAte");
            Assert.IsTrue(target.Countains("private"));
        }


        [TestMethod()]
        public void Null_will_be_added_usually_no_exception()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add(null);
            Assert.AreEqual(1, target.Count);
            Assert.IsTrue(target.Countains(null));
        }

        [TestMethod()]
        public void Empty_string_will_be_added_usually()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add(string.Empty);
            Assert.AreEqual(1, target.Count);
            Assert.IsTrue(target.Countains(string.Empty));
        }
    }

    [TestClass()]
    public class CommonBlacklist_Count_Tests
    {
        [TestMethod()]
        public void Count_returns_1_when_1_word_added()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add("private");
            Assert.AreEqual(1, target.Count);
        }


        [TestMethod()]
        public void Count_returns_3_when_3_word_added()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add("private");
            target.Add("void");
            target.Add("MethodName");
            Assert.AreEqual(3, target.Count);
        }

        [TestMethod()]
        public void Newly_created_blacklist_using_array_parameter_has_appropriate_count()
        {
            var inputArray = new[] { "public", "void", "MethodName" };
            CommonBlacklist target = new CommonBlacklist(inputArray);
            Assert.AreEqual(inputArray.Length, target.Count);
        }

        [TestMethod()]
        public void Count_returns_1_fow_case_different_same_word_added_twice()
        {
            CommonBlacklist target = new CommonBlacklist();
            target.Add("PriVate");
            target.Add("pRIVAte");
            Assert.AreEqual(1, target.Count);
        }
    }
}
