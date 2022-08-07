using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HandbrakeCliWrapper;

namespace VideoConverter
{
    class Program
    {
        private static int _total;
        private static int _current;

        static async Task Main(string[] args)
        {
            var a= new HandbrakeConfiguration {
                
            };

            var extensions = new []{".mkv", ".avi", }.ToHashSet();
            var input = args[0];
            var files = Directory.GetFiles(input, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(Path.GetExtension(f))).ToList();
            var output = args[1];
            await Run(files, output);
            Console.WriteLine("Done!");
        }

        private static async Task Run(IReadOnlyCollection<string> files, string output)
        {
            var hb = new Handbrake("./HandbrakeCLI.exe");
            _total = files.Count;
            _current = 0;
            PrintStatus(hb);
            var config = new HandbrakeConfiguration();
            foreach (var file in files)
            {
                await hb.Transcode(config, file, output, overwriteExisting: true);
                _current++;
            }
        }

        private static async void PrintStatus(Handbrake hb) 
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
