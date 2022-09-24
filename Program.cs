using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace NLang;

public static class Program {
    public static void Main(String[] args) {
        Stopwatch main = new Stopwatch();
        main.Start();

        String? output = null;
        String? input = null;
        String gcc = "gcc";
        bool outputC = false;
        bool debug = false;

        void fail() { Environment.Exit(1); return; }

        if (args.Contains("--help") || args.Contains("-h")) {
            Console.WriteLine("Usage: nlang [options] <input file>\n");
            Console.WriteLine("Example:");
            Console.WriteLine("nlang -o ./main ./main.nl\n");
            Console.WriteLine("Options:");
            Console.WriteLine("-o|--output|--out <file>    Specify output file");
            Console.WriteLine("-h|--help                   Show this help message");
            Console.WriteLine("-cc|--ccompile              Compile to a C file");
            Console.WriteLine("-g|--gcc <path>             Specify GCC path");
            Console.WriteLine("-d|--debug                  Enable debug mode");
            Environment.Exit(0);
            return;
        }

        

        for (int i = 0; i < args.Length; i++) {
            String arg = args[i];

            bool hasArgs(params string[] arglist) {
                if (arglist.Contains(arg.ToLower())) return true;
                return false;
            }

            bool nextArgExists = args.Length > i + 1;

            if (hasArgs("-o", "--output", "--out")) {
                if (!nextArgExists) {
                    Console.WriteLine("Error: No output file specified.");
                    fail();
                }

                output = args[i + 1];
                i++;
            }

            else if (hasArgs("-cc", "--ccompile")) outputC = true;
            else if (hasArgs("-d", "--debug")) debug = true;

            else if (hasArgs("-g", "--gcc")) {
                if (!nextArgExists) {
                    Console.WriteLine("Error: No GCC path specified.");
                    fail();
                }

                gcc = args[i + 1];
                i++;
            }

            else {
                if (input != null) {
                    Console.WriteLine("Error: Too many input files.");
                    fail();
                }
                
                input = arg;
            }
        }

        if (input == null) {
            Console.WriteLine("Error: No input file specified.\nUse --help for more information.");
            fail();
            return; // Prevent compiler warning
        }

        if (!File.Exists(input)) {
            Console.WriteLine("Error: Input file does not exist.");
            fail();
        }

        Stopwatch run = new Stopwatch();
        Stopwatch compile = new Stopwatch();

        run.Start();
        String[] data = NLang.Language.NLanguage.Run(input);
        run.Stop();

        compile.Start();
        String name = Path.GetFileNameWithoutExtension(input);

        if (outputC) {
            NLang.Compile.Compiler.CompileC(data, output, gcc);
        } else {
            NLang.Compile.Compiler.Compile(data, name, output, gcc);
        }

        compile.Stop();

        main.Stop();

        if (debug) {
            Console.WriteLine($"Compile time: {compile.ElapsedMilliseconds}ms");
            Console.WriteLine($"Run time: {run.ElapsedMilliseconds}ms");
            Console.WriteLine($"Total time: {main.ElapsedMilliseconds}ms");
        }
    }
}