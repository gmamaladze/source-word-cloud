using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public class BaseLayout : ILayout
    {
        private readonly int m_MinimalSideLength;
        private readonly List<RectangleF> m_EmptyBoxes;
        private readonly BoxCutter m_BoxCutter;

        public virtual IEnumerable<RectangleF> EmptyBoxes
        {
            get { return m_EmptyBoxes; }
        }

        public BaseLayout(IEnumerable<RectangleF> emptyBoxes, BoxCutter boxCutter, int minimalSideLength)
        {
            m_BoxCutter = boxCutter;
            m_MinimalSideLength = minimalSideLength;
            m_EmptyBoxes = new List<RectangleF>(emptyBoxes);
        }

        public bool IsTooSmall(RectangleF rectangle)
        {
            return rectangle.Size.Width < m_MinimalSideLength || rectangle.Size.Height < m_MinimalSideLength;
        }

        public virtual RectangleF Add(SizeF size)
        {
            int index = FindFirstFitIndex(size);
            if (index<0)
            {
                return RectangleF.Empty;
            }

            RectangleF found = m_EmptyBoxes[index];
            m_EmptyBoxes.RemoveAt(index);

            CutResult cutResult = m_BoxCutter.CutFromCorner(found, size, 0);

            for (int i = 0; i < cutResult.EdgeBoxes.Length; i++ )
            {
                RectangleF edgeBox = cutResult.EdgeBoxes[i];
                if (!IsTooSmall(edgeBox))
                {
                    m_EmptyBoxes.Add(edgeBox);
                }
            }
            return cutResult.Middle;
        }

        protected virtual int FindFirstFitIndex(SizeF size)
        {
            for (int i = 0; i < m_EmptyBoxes.Count; i++)
            {
                RectangleF emptyBox = m_EmptyBoxes[i];
                if (Fits(size, emptyBox))
                {
                    return i;
                }
            }
            return -1;
        }

        public static Tuple<RectangleF, RectangleF, RectangleF> CutAndSplitHorizontally(RectangleF original, SizeF splitPoint)
        {
            RectangleF topLeft = new RectangleF(original.X, original.Y, splitPoint.Width, splitPoint.Height);
            RectangleF topRight = new RectangleF(original.X + splitPoint.Width, original.Y, original.Width - splitPoint.Width, splitPoint.Height);
            RectangleF down = new RectangleF(original.X, original.Y + splitPoint.Height, original.Width, original.Height - splitPoint.Height);
            return new Tuple<RectangleF, RectangleF, RectangleF>(topLeft, topRight, down);
        }

        public static Tuple<RectangleF, RectangleF, RectangleF> CutAndSplitVertically(RectangleF original, SizeF splitPoint)
        {
            RectangleF topLeft = new RectangleF(original.X, original.Y, splitPoint.Width, splitPoint.Height);
            RectangleF right = new RectangleF(original.X + splitPoint.Width, original.Y, original.Width - splitPoint.Width, original.Height);
            RectangleF downLeft = new RectangleF(original.X, original.Y + splitPoint.Height, original.Width - splitPoint.Width, original.Height - splitPoint.Height);
            return new Tuple<RectangleF, RectangleF, RectangleF>(topLeft, right, downLeft);
        }


        public static bool Fits(SizeF size, RectangleF target)
        {
            return target.Size.Width >= size.Width && target.Size.Height >= size.Height;
        }

    }
}
