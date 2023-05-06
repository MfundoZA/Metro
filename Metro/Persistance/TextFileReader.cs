using Metro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public static List<T>? ReadAllAsList<T>(string path)
        {
                string jsonInput;
                byte[] byteArray;
                MemoryStream inputStream;
                List<T>? list;

                try
                {
                    jsonInput = TextFileReader.ReadAll(path);
                    byteArray = Encoding.UTF8.GetBytes(jsonInput);
                    inputStream = new MemoryStream(byteArray);

                    list = JsonSerializer.DeserializeAsync<List<T>>(inputStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
                }
                catch (Exception ex) when (ex is JsonException || ex is FileNotFoundException)
                {
                    list = null;
                }

                return list;
            }
        }
    }
}
