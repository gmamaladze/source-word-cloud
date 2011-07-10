using System.Collections.Generic;
using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public interface ILayout
    {
        void Arrange(KeyValuePair<string, int>[] words, IGraphicEngine graphicEngine);
        IEnumerable<LayoutItem> GetWordsInArea(RectangleF area);
    }
}