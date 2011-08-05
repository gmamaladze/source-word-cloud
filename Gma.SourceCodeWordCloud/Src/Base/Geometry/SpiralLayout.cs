using System;
using Gma.CodeCloud.Base.Portability;

namespace Gma.CodeCloud.Base.Geometry
{
    public class SpiralLayout : BaseLayout
    {
        public SpiralLayout(Size size)
            : base(size)
        {
        }

        public override bool TryFindFreeRectangle(Size size, out Rectangle foundRectangle)
        {
            foundRectangle = Rectangle.Empty;
            double alpha = GetPseudoRandomStartAngle(size);
            const double stepAlpha = Math.PI / 60;

            const double pointsOnSpital = 500;


            Math.Min(Center.Y, Center.X);
            for (int pointIndex = 0; pointIndex < pointsOnSpital; pointIndex++)
            {
                double dX = pointIndex / pointsOnSpital * Math.Sin(alpha) * Center.X;
                double dY = pointIndex / pointsOnSpital * Math.Cos(alpha) * Center.Y;
                foundRectangle = new Rectangle(Center.X + dX - size.Width / 2, Center.Y + dY - size.Height / 2, size.Width, size.Height);

                alpha += stepAlpha;
                if (!IsInsideSurface(foundRectangle))
                {
                    return false;
                }

                if (!QuadTree.HasContent(foundRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        private static double GetPseudoRandomStartAngle(Size size)
        {
            return size.Height*size.Width;
        }
    }
}