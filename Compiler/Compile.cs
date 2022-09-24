using System.Diagnostics;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace NLang.Compile;

public static class Compiler {
    public static string TrimWhitespace(string str) {
        Regex regex = new Regex(@"\s+");
        return regex.Replace(str, " ");
    }

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
        Console.WriteLine(TrimWhitespace(p.StandardOutput.ReadToEnd()));
        Console.WriteLine(TrimWhitespace(p.StandardError.ReadToEnd()));
    }

    public static void CompileC(String[] data, String? c_output) {
        c_output = c_output ?? "main.c";
        if (!c_output.EndsWith(".c"))
            c_output += ".c";
        
        File.WriteAllLines(c_output, data);

        Process p = new Process();
        p.StartInfo.FileName = "/usr/local/bin/gcc-12";
        p.StartInfo.Arguments = $"-o {c_output.Replace(".c", "")} {c_output}";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.Start();

        p.WaitForExit();
        Console.WriteLine(TrimWhitespace(p.StandardOutput.ReadToEnd()));
        Console.WriteLine(TrimWhitespace(p.StandardError.ReadToEnd()));
    }
}