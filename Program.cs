using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace NLang;

public static class Program {
    public static Version version = new Version(0, 1, 1);

    public static void Main(String[] args) {
        Stopwatch main = new Stopwatch();
        main.Start();

        String? output = null;
        String? input = null;
        String gcc = "gcc";
        bool outputC = false;
        bool debug = false;

        void fail() { Environment.Exit(1); return; }

        if (args.Contains("-n") || args.Contains("--new")) {
            New(args);
            return;
        }

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
            Console.WriteLine("-n|--new                    Create a new project (nlang -n -h for more information)");
            Environment.Exit(0);
            return;
        }

        if (args.Contains("--version") || args.Contains("-v")) {
            Console.WriteLine("NLang version " + version.ToString());
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

    public static void New(String[] args) {
        if (args.Contains("-h") || args.Contains("--help")) {
            Console.WriteLine("Usage: nlang [options]\n");
            Console.WriteLine("Options:");
            Console.WriteLine("-h|--help                   Show this help message");
            Console.WriteLine("-p|--project <name>         Create a new project");
            Console.WriteLine("-cd|--createdir             Create a new directory for the project");
            Console.WriteLine("-m|--makefile               Create a new Makefile for compiling the project");
            Console.WriteLine("-i|--ignore                 Create a new .gitignore file for the project");
            Environment.Exit(0);
            return;
        }

        String? name = null;
        bool isProject = false;
        bool createNewDir = args.Contains("-cd") || args.Contains("--createdir");

        for (int i = 0; i < args.Length; i++) {
            String arg = args[i];

            bool hasArgs(params string[] arglist) {
                if (arglist.Contains(arg.ToLower())) return true;
                return false;
            }

            bool nextArgExists = args.Length > i + 1;

            if (hasArgs("-p", "--project")) {
                if (!nextArgExists) {
                    Console.WriteLine("Error: No project name specified.");
                    Environment.Exit(1);
                    return;
                }

                name = args[i + 1];
                isProject = true;
                i++;
            }
        }

        if (name == null) {
            Console.WriteLine("Error: No project name specified.\nUse -n -h for more information.");
            Environment.Exit(1);
            return;
        }

        if (isProject) {
            if (createNewDir) {
                if (Directory.Exists(name)) {
                    Console.WriteLine("Error: Project already exists.");
                    Environment.Exit(1);
                    return;
                }

                Directory.CreateDirectory(name);
            }

            String path = createNewDir ? $"{name}/" : "";
            
            String[] data = new String[] {
                "cimport stdio.h;",
                "",
                "func main(int argc, (arr)string argv) -> int {",
                "    printf(\"Hello, world!\\n\");",
                "    return 0;",
                "}"
            };

            String entry = $"{path}{name}.nl";

            if (File.Exists(entry)) {
                Console.WriteLine("Error: Entry file already exists.");
                Environment.Exit(1);
                return;
            }

            File.WriteAllLines(entry, data);

            if (args.Contains("-m") || args.Contains("--makefile")) {
                if (File.Exists($"{path}Makefile")) {
                    Console.WriteLine("Warning: Makefile already exists, ignoring.");
                } else {
                    String[] makefile = new String[] {
                        $"NAME={name}",
                        $"NLANG=nlang",
                        "",
                        $"$(name): $(NAME).nl",
                        $"\t$(NLANG) -o $(NAME) $(NAME).nl"
                    };

                    File.WriteAllLines($"{path}Makefile", makefile);
                }
            }

            if (args.Contains("-i") || args.Contains("--ignore")) {
                if (File.Exists($"{path}.gitignore")) {
                    Console.WriteLine("Warning: .gitignore already exists, ignoring.");
                    Environment.Exit(1);
                    return;
                } else {
                    File.WriteAllText($"{path}.gitignore", "nlbin/");
                }
            }
        }

        else {
            Console.WriteLine("Error: No project type specified.\nUse -n -h for more information.");
            Environment.Exit(1);
            return;
        }
    }
}

public class Version {
    public int Major;
    public int Minor;
    public int Patch;

    public Version(int major, int minor, int patch) {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public Version(String version) {
        String[] data = version.Split('.');

        if (data.Length != 3) {
            throw new Exception("Invalid version string.");
        }

        Major = int.Parse(data[0]);
        Minor = int.Parse(data[1]);
        Patch = int.Parse(data[2]);
    }

    public override String ToString() {
        return $"{Major}.{Minor}.{Patch}";
    }
}