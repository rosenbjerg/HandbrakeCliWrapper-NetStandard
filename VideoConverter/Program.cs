using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HandbrakeCLIwrapper;

namespace VideoConverter
{
    class Program
    {
        private static int _total;
        private static int _current;

        static void Main(string[] args)
        {
            var exts = new []{".mkv", ".avi", }.ToHashSet();
            var input = args[0];
            var files = Directory.GetFiles(input, "*.*", SearchOption.AllDirectories)
                .Where(f => exts.Contains(Path.GetExtension(f))).ToList();
            var output = args[1];
            Run(files, output);

            Console.ReadLine();
        }

        private static async void Run(List<string> files, string output)
        {
            var hb = new HandbrakeCli("./HandbrakeCLI.exe");
            _total = files.Count;
            _current = 0;
            PrintStatus(hb);
            var config = new HandbrakeCliConfigBuilder();
            foreach (var file in files)
            {
                await hb.Transcode(config, file, output, overwriteExisting: true);
                _current++;
            }
        }
        

        static async void PrintStatus(HandbrakeCli hb) 
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(hb.Status + $" - {_current}/{_total}");
                await Task.Delay(1500);
            }
        }
    }

}
