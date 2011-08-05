using Gma.CodeCloud.Base.Portability;
using Gma.CodeCloud.Base.TextAnalyses.Processing;

namespace Gma.CodeCloud.Base.Geometry
{
    public class LayoutItem
    {
        public LayoutItem(Rectangle rectangle, IWord word)
        {
            this.Rectangle = rectangle;
            Word = word;
        }

        public Rectangle Rectangle { get; private set; }
        public IWord Word { get; private set; }

        public LayoutItem Clone()
        {
            return new LayoutItem(this.Rectangle, this.Word);
        }
    }
}
