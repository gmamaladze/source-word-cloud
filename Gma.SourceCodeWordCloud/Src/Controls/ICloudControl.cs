using System.Collections.Generic;

namespace Gma.CodeCloud.Controls
{
    public interface ICloudControl
    {
        void Clear();
        void Show(KeyValuePair<string, int>[] words);
    }
}
