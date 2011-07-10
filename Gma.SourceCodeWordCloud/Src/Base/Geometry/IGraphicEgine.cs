using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gma.CodeCloud.Base.Geometry
{
    public interface IGraphicEngine
    {
        SizeF Measure(string text, int weight);
        void Draw(LayoutItem layoutItem);
    }
}
