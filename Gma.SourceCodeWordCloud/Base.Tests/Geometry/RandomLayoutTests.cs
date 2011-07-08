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
    public class RandomLayoutTests
    {
        private RectangleF s_EmptyRectangle = new RectangleF(0,0,0,0);

        [TestMethod]
        public void Cut_from_left_upper_edge()
        {
            //RectangleF original = new RectangleF(0, 0, 10, 10);
            //RectangleF toCut = new RectangleF(0, 0, 5, 5);
            //var result = RandomLayout.CutFromMiddleAndSlpit(original, toCut);
            //Assert.AreEqual(result.Item1, toCut);
            //Assert.AreEqual(result.Item2, s_EmptyRectangle);
            //Assert.AreEqual(result.Item3, s_EmptyRectangle);
            //Assert.AreEqual(result.Item4, new RectangleF(5, 0, 5, 10));
            //Assert.AreEqual(result.Item5, new RectangleF(0, 5, 5, 5));
        }

        [TestMethod]
        public void Cut_from_middle()
        {
            //RectangleF original = new RectangleF(0, 0, 15, 15);
            //RectangleF toCut = new RectangleF(5, 5, 5, 5);
            //var result = RandomLayout.CutFromMiddleAndSlpit(original, toCut);
            //Assert.AreEqual(result.Item1, toCut);
            //Assert.AreEqual(result.Item2, new RectangleF(0, 0, 5, 10));
            //Assert.AreEqual(result.Item3, new RectangleF(5, 0, 10, 5));
            //Assert.AreEqual(result.Item4, new RectangleF(10, 5, 5, 10));
            //Assert.AreEqual(result.Item5, new RectangleF(0, 10, 10, 5));
        }

    }
}
