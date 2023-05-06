using Metro.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Persistance
{
    public class TextFileWriter
    {
        private static StreamWriter? StreamWriter;

        public static void Write(string text, string path)
        {
            try
            {
                StreamWriter = File.AppendText(path);
                StreamWriter.WriteLine(text);
                StreamWriter.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occured! Please try again. \n" +
                    $"Error message: {exception.Message}");
            }
        }

        public static void Write(object data, string path)
        {
            try
            {
                StreamWriter = File.AppendText(path);
                StreamWriter.WriteLine(data.ToString());
                StreamWriter.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occured! Please try again. \n" +
                    $"Error message: {exception.Message}");
            }
        }
    }
}
