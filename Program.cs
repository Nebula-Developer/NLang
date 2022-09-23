using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.IO;

namespace NLang;

public static class Program {
    public static void Main(String[] args) {
        String[] data = NLang.Language.NLanguage.Run("test.nl");
        File.WriteAllLines("nlbin/c/test.c", data);
        NLang.Compile.Compiler.Compile(data, "test");
    }
}