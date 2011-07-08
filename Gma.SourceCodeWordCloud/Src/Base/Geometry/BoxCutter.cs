using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gma.CodeCloud.Base.Geometry
{
    public class BoxCutter
    {

        private readonly Random m_Randomizer;

        public BoxCutter(Random randomizer)
        {
            m_Randomizer = randomizer;
        }

        public CutResult CutFromRandomCorner(RectangleF original, SizeF toCut)
        {
            int edgeNumber = m_Randomizer.Next(0, 3);
            return CutFromCorner(original, toCut, edgeNumber);
        }


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

        public CutResult CutFromCorner(RectangleF original, SizeF toCut, int cornerNumber)
        {
            switch (cornerNumber)
            {
                case 0:
                    return CutFromTopLeft(original, toCut);
                case 1:
                    return CutFromTopRight(original, toCut);
                case 2:
                    return CutFromBottomRight(original, toCut);
                case 3:
                    return CutFromBottomLeft(original, toCut);
                default:
                    throw new ArgumentOutOfRangeException("cornerNumber", cornerNumber, "Expected values are 0,1,2,3.");
            }
        } 


        private static CutResult CutFromTopLeft(RectangleF original, SizeF toCut)
        {
            RectangleF topLeftBox = new RectangleF(new PointF(original.X, original.Y), toCut);
            return CutAnyFromMiddle(original, topLeftBox);
        }

        private static CutResult CutFromTopRight(RectangleF original, SizeF toCut)
        {
            RectangleF topLeftBox = new RectangleF(new PointF(original.X + original.Width - toCut.Width, original.Y), toCut);
            return CutAnyFromMiddle(original, topLeftBox);
        }

        private static CutResult CutFromBottomRight(RectangleF original, SizeF toCut)
        {
            RectangleF topLeftBox = new RectangleF(new PointF(original.X + original.Width - toCut.Width, original.Y + original.Height - toCut.Height), toCut);
            return CutAnyFromMiddle(original, topLeftBox);
        }

        private static CutResult CutFromBottomLeft(RectangleF original, SizeF toCut)
        {
            RectangleF topLeftBox = new RectangleF(new PointF(original.X, original.Y + original.Height - toCut.Height), toCut);
            return CutAnyFromMiddle(original, topLeftBox);
        }

        public static CutResult CutAnyFromMiddle(RectangleF original, RectangleF middleBox)
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

            float column1Width = middleBox.X - original.X;
            float column2Width = middleBox.Width;
            float column3Width = original.Width - column1Width - middleBox.Width;

            float row1Height = middleBox.Y - original.Y;
            float row2Heigth = middleBox.Height;
            float row3Height = original.Height - row1Height - middleBox.Height;

            RectangleF leftTop = new RectangleF(original.X, original.Y, column1Width, row1Height + row2Heigth);
            RectangleF rightTop = new RectangleF(original.X + column1Width, original.Y, column2Width + column3Width, row1Height);
            RectangleF rightBottom = new RectangleF(original.X + column1Width + column2Width, original.Y + row1Height, column3Width, row2Heigth);
            RectangleF leftBottom = new RectangleF(original.X, original.Y + row1Height + row2Heigth, column1Width + column2Width + column3Width, row3Height);

            return new CutResult(middleBox, new[] {leftTop, rightTop, rightBottom, leftBottom});
        }
    }
}
