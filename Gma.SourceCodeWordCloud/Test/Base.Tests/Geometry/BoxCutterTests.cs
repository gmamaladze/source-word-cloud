using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Gma.CodeCloud.Base.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base.Tests.Geometry
{
    [TestClass]
    public class BoxCutterTests
    {
        //--------------------------
        //|     |       1          |
        //|     |                  |
        //|  0  -------------------|
        //|     |          |       |
        //|     |   mid    |       |
        //|     |          |  2    |
        //|------------------------|
        //|                        |
        //|      3                 |
        //--------------------------

        [TestMethod]
        public void Cut_from_top_left_corner()
        {
            
            RectangleF original = new RectangleF(0, 0, 10, 10);
            SizeF toCut = new SizeF(5, 5);
            var result = new BoxCutter(null).CutFromCorner(original, toCut, 0);
            Assert.AreEqual(result.Middle, new RectangleF(new PointF(0,0), toCut));
            AssertBoxIsEmpty(result.EdgeBoxes[0]);
            AssertBoxIsEmpty(result.EdgeBoxes[1]);
            Assert.AreEqual(new RectangleF(5, 0, 5, 5), result.EdgeBoxes[2]);
            Assert.AreEqual(new RectangleF(0, 5, 10, 5), result.EdgeBoxes[3]);
        }

        public void AssertBoxIsEmpty(RectangleF rectangle)
        {
            Assert.AreEqual(0, rectangle.Width * rectangle.Height, "Rectangle {0} is expected to be empty.", rectangle);
        }

        [TestMethod]
        public void Cut_from_middle()
        {
            RectangleF original = new RectangleF(0, 0, 15, 15);
            RectangleF toCut = new RectangleF(5, 5, 5, 5);
            var result = BoxCutter.CutAnyFromMiddle(original, toCut);
            Assert.AreEqual(result.Middle, toCut);
            Assert.AreEqual(result.EdgeBoxes[0], new RectangleF(0, 0, 5, 10));
            Assert.AreEqual(result.EdgeBoxes[1], new RectangleF(5, 0, 10, 5));
            Assert.AreEqual(result.EdgeBoxes[2], new RectangleF(10, 5, 5, 5));
            Assert.AreEqual(result.EdgeBoxes[3], new RectangleF(0, 10, 15, 5));
        }


    }
}
