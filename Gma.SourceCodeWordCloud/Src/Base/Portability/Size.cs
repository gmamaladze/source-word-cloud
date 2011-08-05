namespace Gma.CodeCloud.Base.Portability
{
    public struct Size
    {
        public Size(double width, double height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public double Width { get; set; }
        public double Height { get; set; }
    }
}
