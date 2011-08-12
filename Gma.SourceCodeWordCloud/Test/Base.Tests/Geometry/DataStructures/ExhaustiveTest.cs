using System.Data;
using Gma.CodeCloud.Base.Geometry;
using Gma.CodeCloud.Base.Geometry.DataStructures;
using Gma.CodeCloud.Base.Geometry.Portability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base.Tests.Geometry.DataStructures
{
    [TestClass]
    public class ExhaustiveTest
    {
        public static QuadTree<LayoutItem> QuadTree { get; set; }


        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            QuadTree = new QuadTree<LayoutItem>(new Rectangle(new Point(0, 0), new Size(2000, 2000)));
        }

        [ClassCleanup]
        public static void TearDown()
        {
            QuadTree = null;
        }

        public TestContext TestContext
        {
            get; set;
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TestData.csv", "TestData#csv", DataAccessMethod.Sequential)] 
        [DeploymentItem("Test\\Base.Tests\\TestData.csv")]
        public void AssertDataRow()
        {
            AssertDataRow(TestContext.DataRow, QuadTree);
        }

        private void AssertDataRow(DataRow dataRow, QuadTree<LayoutItem> quadTree)
        {
            string methodName = dataRow[0].ToString();
            Assert.IsNotNull(methodName);

            float x;
            Assert.IsTrue(float.TryParse(dataRow[1].ToString(), out x));
            float y;
            Assert.IsTrue(float.TryParse(dataRow[2].ToString(), out y));
            float width;
            Assert.IsTrue(float.TryParse(dataRow[3].ToString(), out width));
            float height;
            Assert.IsTrue(float.TryParse(dataRow[4].ToString(), out height));

            bool result;
            Assert.IsTrue(bool.TryParse(dataRow[5].ToString(), out result));

            Rectangle rectangle = new Rectangle(x, y, width, height);

            Assert.AreEqual(result, InvokeMethod(methodName, rectangle, quadTree));
        }


        public bool InvokeMethod(string methodName, Rectangle rectangle, QuadTree<LayoutItem> quadTree)
        {
            switch (methodName)
            {
                case "Insert":
                    quadTree.Insert(new LayoutItem(rectangle, null));
                    return true;

                case "HasContent":
                    var result = quadTree.HasContent(rectangle);
                    return result;
            }

            Assert.Fail("Unknown method {0}", methodName);
            return false;
        }
    }
}
