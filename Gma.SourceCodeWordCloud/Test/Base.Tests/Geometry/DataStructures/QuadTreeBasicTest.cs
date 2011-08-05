using Gma.CodeCloud.Base.Geometry;
using Gma.CodeCloud.Base.Geometry.DataStructures;
using Gma.CodeCloud.Base.Portability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base.Tests.Geometry.DataStructures
{
    [TestClass]
    public class QuadTreeBasicTest
    {
        [TestMethod]
        public void Newly_created_tree_has_no_content1()
        {
            Newly_created_tree_has_no_content(0f, 0f, 0f, 0f);
        }

        [TestMethod]
        public void Newly_created_tree_has_no_content2()
        {
            Newly_created_tree_has_no_content(0f, 0f, 1f, 1f);
        }

        [TestMethod]
        public void Newly_created_tree_has_no_content3()
        {
            Newly_created_tree_has_no_content(0f, 0f, 1000f, 1000f);
        }

        public void Newly_created_tree_has_no_content(float x, float y, float width, float height)
        {
            Rectangle surface = new Rectangle(x, x, width, height);
            var target = CreateQuadTree(surface);
            Assert.AreEqual(0, target.Count);
            Assert.IsFalse(target.HasContent(surface));
        }

        [TestMethod]
        public void Inserting_one_point_and_query_except_0x0_1()
        {
            Inserting_one_point_and_query_except_0x0(5f, 5f);
        }

        [TestMethod]
        public void Inserting_one_point_and_query_except_0x0_2()
        {
            Inserting_one_point_and_query_except_0x0(1f, 1f);
        }

        [TestMethod]
        public void Inserting_one_point_and_query_except_0x0_3()
        {
            Inserting_one_point_and_query_except_0x0(25.67f, 356.65f);
        }

        [TestMethod]
        public void Inserting_one_point_and_query_except_0x0_4()
        {
            Inserting_one_point_and_query_except_0x0(435.525f, 56.12f);
        }
        
        public void Inserting_one_point_and_query_except_0x0(float pointX, float pointY)
        {
            Rectangle surface = new Rectangle(0, 0, 1000, 1000);
            var target = CreateQuadTree(surface);

            var itemRectangle = new Rectangle(pointX, pointY, 0, 0);
            var item = new LayoutItem(itemRectangle, null);
            target.Insert(item);

            Assert.AreEqual(1, target.Count);
            Assert.IsTrue(target.HasContent(surface));

            var emptyRectangle = new Rectangle(0, 0, pointX - 1f, pointY - 1f);
            Assert.IsFalse(target.HasContent(emptyRectangle));
        }

        [TestMethod]
        public void Inserting_0x0_point_and_query()
        {
            Rectangle surface = new Rectangle(0, 0, 1000, 1000);
            var target = CreateQuadTree(surface);

            var itemRectangle = new Rectangle(0, 0, 0, 0);
            var item = new LayoutItem(itemRectangle, null);
            target.Insert(item);

            Assert.AreEqual(1, target.Count);
            Assert.IsTrue(target.HasContent(surface));

            var emptyRectangle = new Rectangle(0 + 1f, 0 + 1f, surface.Width - 1f, surface.Width - 1f);
            Assert.IsFalse(target.HasContent(emptyRectangle));
        }



        [TestMethod]
        public void Inserting_rectangle_and_query_except_startingin_0x0_1()
        {
            Inserting_rectangle_and_query_except_startingin_0x0(5f, 5f, 1, 1);
        }

        [TestMethod]
        public void Inserting_rectangle_and_query_except_startingin_0x0_2()
        {
            Inserting_rectangle_and_query_except_startingin_0x0(1, 1, 1, 1);
        }

        [TestMethod]
        public void Inserting_rectangle_and_query_except_startingin_0x0_3()
        {
            Inserting_rectangle_and_query_except_startingin_0x0(25.67f, 356.65f, 435.525f, 56.12f);
        }

        [TestMethod]
        public void Inserting_rectangle_and_query_except_startingin_0x0_4()
        {
            Inserting_rectangle_and_query_except_startingin_0x0(25.67f, 356.65f, 435.525f, 56.12f);
        }

        [TestMethod]
        public void Inserting_rectangle_and_query_except_startingin_0x0_5()
        {
            Inserting_rectangle_and_query_except_startingin_0x0(100f, 100f, 200f, 200f);
        }

        public void Inserting_rectangle_and_query_except_startingin_0x0(float x, float y, float width, float height)
        {
            Rectangle surface = new Rectangle(0, 0, 1000, 1000);
            var target = CreateQuadTree(surface);

            var itemRectangle = new Rectangle(x, y, width, height);
            var item = new LayoutItem(itemRectangle, null);
            target.Insert(item);

            Assert.AreEqual(1, target.Count);
            Assert.IsTrue(target.HasContent(surface));

            var bitLargerThanItemRectangle = new Rectangle(itemRectangle.X - 1f, itemRectangle.Y - 1f, itemRectangle.Width + 2f, itemRectangle.Height + 2f);
            Assert.IsTrue(target.HasContent(bitLargerThanItemRectangle));

            var emptyRectangle = new Rectangle(0, 0, x - 1f, y - 1f);
            Assert.IsFalse(target.HasContent(emptyRectangle));
        }

        protected virtual QuadTree<LayoutItem> CreateQuadTree(Rectangle rectangle)
        {
            return new QuadTree<LayoutItem>(rectangle);
        }
    }
}