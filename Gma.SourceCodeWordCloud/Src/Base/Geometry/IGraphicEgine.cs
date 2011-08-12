using System;
using Gma.CodeCloud.Base.Geometry.Portability;

namespace Gma.CodeCloud.Base.Geometry
{
    public interface IGraphicEngine : IDisposable
    {
        Size Measure(string text, int weight);
        void Draw(LayoutItem layoutItem);
        void DrawEmphasized(LayoutItem layoutItem);
    }
}
