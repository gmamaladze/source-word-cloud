using System.Collections.Generic;
using System.IO;

namespace Gma.CodeCloud.Base
{
    internal class TextFileBlacklist : CommonBlacklist
    {
        public TextFileBlacklist(string fileName) : this(new FileInfo(fileName))
        {
        }

        public TextFileBlacklist(FileInfo fileInfo) : this(fileInfo.OpenText())
        {
        }

        public TextFileBlacklist(StreamReader reader)
        {
            using (reader)
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    Add(line);
                    line = reader.ReadLine();
                }
            }
        }
    }
}