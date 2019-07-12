using CommandLine;
using System;
using System.IO;

namespace SFB_Profile_Converter
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file name.")]
        public string Input { get; set; }
        [Option('o', "output", Required = false, HelpText = "Output file name. Same as -i if not set but then requires -f.")]
        public string Output { get; set; }
        [Option('s', "src-dir", Required = false, HelpText = "Source directory path.")]
        public string SrcDir { get; set; }
        [Option('d', "dest-dir", Required = false, HelpText = "Destination directory path.")]
        public string DestDir { get; set; }
        [Option('q', "Quiet", Required = false, HelpText = "Run silently.", Default = false)]
        public bool Quiet { get; set; }
        [Option('f', "Force", Required = false, HelpText = "Force overwriting input file.", Default = false)]
        public bool Force { get; set; }

        public Options parseOpts(string[] args)
        {
            Options opts = null;
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(r => { opts = r; });

            if (opts.Output == null)
            {
                opts.Output = opts.Input;
                if (string.IsNullOrEmpty(opts.SrcDir) ? string.IsNullOrEmpty(opts.DestDir) : (opts.SrcDir == opts.DestDir) &&
                    !opts.Force)
                {
                    Console.Write("Output file name not set. Add '-f/--force' to overwrite input file.");
                    Environment.Exit(1);
                }
            }

            if (opts.SrcDir == null)
            {
                opts.SrcDir = Directory.GetCurrentDirectory();
            }
            else
            {
                if (!Directory.Exists(opts.SrcDir))
                {
                    Console.WriteLine($"Source directory '{opts.SrcDir}' does not exist! Exiting.");
                    Environment.Exit(1);
                }
            }

            if (string.IsNullOrEmpty(opts.DestDir))
            {
                Globals.co.write("Dest Dir is empty");
                opts.DestDir = Directory.GetCurrentDirectory();
                Globals.co.write($"Dest dir = '{opts.DestDir}'");
            }
            else
            {
                if (!Directory.Exists(opts.DestDir))
                {
                    Console.WriteLine($"Destination directory '{opts.DestDir}' does not exist! Exiting.");
                    Environment.Exit(1);
                }
            }

            var fp = Path.Combine(opts.SrcDir, opts.Input);
            if (!File.Exists(fp))
            {
                Console.WriteLine($"Input file path '{fp}' does not exist! Existing.");
                Environment.Exit(1);
            }

            return opts;
        }
    }

}