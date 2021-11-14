using System;
using System.IO;
using System.Text;

namespace FileEncodingChanger
{
    class Program
    {
        static void Main()
        {
            string sourceDirectory = @"D:\test";
            string sourcePattern = "*.sql";

            RecodeFiles(sourceDirectory, sourcePattern);

            Console.ReadLine();
        }

        private static async void RecodeFiles(string sourceDirectory, string sourcePattern)
        {
            int filesCount = 0;

            try
            {
                var files = Directory.GetFiles(sourceDirectory, sourcePattern, SearchOption.AllDirectories);

                if (files == null)
                {
                    throw new ArgumentNullException();
                }

                var filesTotal = files.Length;
                Console.WriteLine($"Found {filesTotal} files.");

                foreach (var file in files)
                {
                    using StreamReader sr = new StreamReader(file);
                    var data = await sr.ReadToEndAsync();
                    sr.Close();

                    using StreamWriter sw = new StreamWriter(file, append: false, Encoding.UTF8);
                    await sw.WriteAsync(data);
                    sw.Close();

                    filesCount++;

                    PrintPercentOfWork(filesCount, filesTotal);
                }

                Console.WriteLine($"Rewrited {filesCount} files.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void PrintPercentOfWork(int filesCount, int filesTotal)
        {
            var percent = filesCount * 100 / filesTotal;
            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"Completed {percent}%");
        }
    }
}
