using System.Diagnostics;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace NLang.Compile;

public static class Compiler {
    public static bool IsOnlyspaces(String str) {
        return str.Trim().Length == 0;
    }

    public static string TrimWhitespace(string str) {
        if (IsOnlyspaces(str)) return "";
        return str.TrimStart().TrimEnd() + "\n";
    }

    public static void Compile(String[] data, String name, String? output, String gcc = "gcc") {
        output = output ?? "main.nlout";
        Process p = new Process();
        p.StartInfo.FileName = gcc;
        Directory.CreateDirectory("nlbin");
        File.WriteAllLines("nlbin/" + name + ".c", data);
        p.StartInfo.Arguments = $"-o {output} nlbin/{name}.c";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        
        try {
            p.Start();
        } catch {
            Console.WriteLine("Failed to execute GCC.");
            Console.WriteLine("Please define the GCC (or other compiler) path with -g <path>");
            Environment.Exit(1);
            return;
        }

        p.WaitForExit();
        Console.Write(TrimWhitespace(p.StandardOutput.ReadToEnd()));
        Console.Write(TrimWhitespace(p.StandardError.ReadToEnd()));
    }

    public static void CompileC(String[] data, String? c_output, String gcc = "gcc") {
        c_output = c_output ?? "main.c";
        if (!c_output.EndsWith(".c"))
            c_output += ".c";
        
        File.WriteAllLines(c_output, data);

        Process p = new Process();
        p.StartInfo.FileName = gcc;
        p.StartInfo.Arguments = $"-o {c_output.Replace(".c", "")} {c_output}";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;

        try {
            p.Start();
        } catch {
            Console.WriteLine("Failed to execute GCC.");
            Console.WriteLine("Please define the GCC (or other compiler) path with -g <path>");
            Environment.Exit(1);
            return;
        }

        p.WaitForExit();
        Console.Write(TrimWhitespace(p.StandardOutput.ReadToEnd()));
        Console.Write(TrimWhitespace(p.StandardError.ReadToEnd()));
    }
}