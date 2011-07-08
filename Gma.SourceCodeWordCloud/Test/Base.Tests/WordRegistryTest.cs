using Gma.CodeCloud.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Base.Tests
{


    [TestClass()]
    public class WordRegistry_Construction_Tests
    {
        [TestMethod()]
        public void Newly_created_registry_is_empty()
        {
            IWordRegistry target = new WordRegistry();
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod()]
        public void Newly_created_registry_with_capacity_grater_than_0_is_also_empty()
        {
            IWordRegistry target = new WordRegistry(0xfff, StringComparer.InvariantCultureIgnoreCase);
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod()]
        public void A_registry_constructed_withdefault_constructor_is_case_insensitive()
        {
            IWordRegistry target = new WordRegistry(0, StringComparer.InvariantCultureIgnoreCase);
            
            target.AddOccurance("PuBlic");
            target.AddOccurance("public");
            target.AddOccurance("PUBLIC");

            target.AddOccurance("VoiD");
            target.AddOccurance("void");
            target.AddOccurance("VOID");

            target.AddOccurance("mETHODnAME");
            target.AddOccurance("methodname");
            target.AddOccurance("METHODNAME");

            Assert.AreEqual(3, target.Count);
        }

        [TestMethod()]
        public void A_registry_constructed_with_case_aware_StringComparer_is_case_sensitive()
        {
            IWordRegistry target = new WordRegistry(0, StringComparer.InvariantCulture);

            target.AddOccurance("PuBlic");
            target.AddOccurance("public");
            target.AddOccurance("PUBLIC");

            target.AddOccurance("VoiD");
            target.AddOccurance("void");
            target.AddOccurance("VOID");

            target.AddOccurance("mETHODnAME");
            target.AddOccurance("methodname");
            target.AddOccurance("METHODNAME");

            Assert.AreEqual(9, target.Count);
        }
    }
}
