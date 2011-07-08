using System.Collections.Generic;
using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public sealed class CutResult
    {
        public RectangleF Middle { get; set; }
        private readonly RectangleF[] m_EdgeBoxes;


        public CutResult(RectangleF middle, RectangleF[] edgeBoxes)
        {
            Middle = middle;
            m_EdgeBoxes = edgeBoxes;
        }

        public RectangleF[] EdgeBoxes
        {
            get { return m_EdgeBoxes; }
        }
    }
}
