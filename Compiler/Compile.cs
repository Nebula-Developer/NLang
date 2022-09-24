using System.Diagnostics;
using System.IO;
using System;

namespace NLang.Compile;

public static class Compiler {
    public static void Compile(String[] data, String name, String? output) {
        output = output ?? "main.nlout";
        Process p = new Process();
        p.StartInfo.FileName = "/usr/local/bin/gcc-12";
        Directory.CreateDirectory("nlbin");
        File.WriteAllLines("nlbin/" + name + ".c", data);
        p.StartInfo.Arguments = $"-o {output} nlbin/{name}.c";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.Start();
        p.WaitForExit();
        Console.WriteLine(p.StandardOutput.ReadToEnd());
        Console.WriteLine(p.StandardError.ReadToEnd());
    }

    public static void CompileC(String[] data, String name, String? output) {
        output = output ?? "main.c";
        Directory.CreateDirectory("nlbin");
        File.WriteAllLines(output, data);
        Process p = new Process();
        p.StartInfo.FileName = "/usr/local/bin/gcc-12";
        p.StartInfo.Arguments = $"-o nlbin/c_{name} {output}";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.Start();
        p.WaitForExit();
        Console.WriteLine(p.StandardOutput.ReadToEnd());
        String err = p.StandardError.ReadToEnd();
        Console.WriteLine(err);

        if (err.Contains("error")) {
            Console.WriteLine("Error: Failed to compile C code.");
            Environment.Exit(1);
        }
    }
}