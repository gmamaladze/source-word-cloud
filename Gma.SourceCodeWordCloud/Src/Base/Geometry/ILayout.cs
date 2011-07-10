using System.Collections.Generic;
using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public interface ILayout
    {
        void DrawWords(KeyValuePair<string, int>[] words, IGraphicEngine graphicEngine);
    }
}