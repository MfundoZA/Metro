using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Persistance
{
    public class TextFileReader
    {
        public StreamReader? StreamReader { get; set; }

        public static string ReadAll(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex) when (ex is FileNotFoundException)
            {
                throw new FileNotFoundException(ex.Message);
            }
        }
    }
}
