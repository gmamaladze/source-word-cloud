using System.Collections.Generic;
using Gma.CodeCloud.Base.Portability;
using Gma.CodeCloud.Base.TextAnalyses.Processing;

namespace Gma.CodeCloud.Base.Geometry
{
    public interface ILayout
    {
        int Arrange(IEnumerable<IWord> words, IGraphicEngine graphicEngine);
        IEnumerable<LayoutItem> GetWordsInArea(Rectangle area);
    }
}