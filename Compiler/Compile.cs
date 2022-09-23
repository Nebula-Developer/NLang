using System.Diagnostics;
using System.IO;
using System;

namespace NLang.Compile;

public static class Compiler {
    public static void Compile(String[] data, String name) {
        Process p = new Process();
        p.StartInfo.FileName = "/usr/local/bin/gcc-12";
        Directory.CreateDirectory("nlbin");
        Directory.CreateDirectory("nlbin/c/");
        File.WriteAllLines("nlbin/c/" + name + ".c", data);
        p.StartInfo.Arguments = $"-o ./{name} nlbin/c/{name}.c";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.Start();
        p.WaitForExit();
        Console.WriteLine(p.StandardOutput.ReadToEnd());
        Console.WriteLine(p.StandardError.ReadToEnd());
    }
}