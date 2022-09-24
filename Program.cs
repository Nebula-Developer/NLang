using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.IO;

namespace NLang;

public static class Program {
    public static void Main(String[] args) {
        String output = "main.nlout";
        String input = "";

        List<string> outputArgs = new List<string> { "-o", "--output", "--out" };

        if (args.Contains("--help") || args.Contains("-h")) {
            Console.WriteLine("Usage: nlang [options] <input file>\n");
            Console.WriteLine("Example:");
            Console.WriteLine("nlang -o ./main ./main.nl\n");
            Console.WriteLine("Options:");
            Console.WriteLine("-o|--output|--out <file>    Specify output file");
            Console.WriteLine("-h|--help                   Show this help message");
            return;
        }

        for (int i = 0; i < args.Length; i++) {
            String arg = args[i];
            if (outputArgs.Contains(arg)) {
                if (args.Length > i + 1) {
                    output = args[i + 1];
                    i++;
                } else {
                    Console.WriteLine("Error: No output file specified.");
                    Environment.Exit(1);
                }
            } else {
                if (input == "") {
                    input = arg;
                } else {
                    Console.WriteLine("Error: Too many input files.");
                    Environment.Exit(1);
                }
            }
        }

        if (input == "") {
            Console.WriteLine("Error: No input file specified.\nUse --help for more information.");
            Environment.Exit(1);
        }

        if (!File.Exists(input)) {
            Console.WriteLine("Error: Input file does not exist.");
            Environment.Exit(1);
        }

        String[] data = NLang.Language.NLanguage.Run(input);
        String name = Path.GetFileNameWithoutExtension(input);

        NLang.Compile.Compiler.Compile(data, name, output);
    }
}