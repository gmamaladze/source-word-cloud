using System.Collections.Generic;
using System.Drawing;
using Gma.CodeCloud.Base.TextAnalyses.Processing;

namespace Gma.CodeCloud.Base.Geometry
{
    public interface ILayout
    {
        int Arrange(IEnumerable<IWord> words, IGraphicEngine graphicEngine);
        IEnumerable<LayoutItem> GetWordsInArea(RectangleF area);
    }
}