using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Gma.CodeCloud.Base.Geometry
{
    public class RandomLayout : BaseLayout
    {
        private readonly DistanceToCenterComparer m_Comparer;

        public RandomLayout(IEnumerable<RectangleF> emptyBoxes, BoxCutter boxCutter, int minimalSideLength, PointF center)
            : base(emptyBoxes, boxCutter, minimalSideLength)
        {
            m_Comparer = new DistanceToCenterComparer(center);
        }


        protected int FindFirstFitIndex(SizeF size)
        {
            throw new NotImplementedException();
        }

        private sealed class DistanceToCenterComparer : IComparer<RectangleF>
        {
            private readonly PointF m_Center;

            public DistanceToCenterComparer(PointF center)
            {
                m_Center = center;
            }

            public int Compare(RectangleF rectangle1, RectangleF rectangle2)
            {
                if (rectangle1.Height == rectangle2.Height) return 0;
                return rectangle1.Height > rectangle2.Height ? 1 : -1;
            }
        }
    }
}
