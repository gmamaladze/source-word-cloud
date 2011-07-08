using System.Collections.Generic;
using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public interface ILayout
    {
        RectangleF Add(SizeF size);
        IEnumerable<RectangleF> EmptyBoxes { get; }
    }
}