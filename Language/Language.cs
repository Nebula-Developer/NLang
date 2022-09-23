using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace NLang.Language;


public static class NLanguage {
    public static string[] Run(String path) {
        String data = File.ReadAllText(path);
        
        void RegexReplace(String regex, String replace) {
            data = Regex.Replace(data, regex, replace);
        }

        List<(string, string)> Predefs = new List<(string, string)>();
        Predefs.Add(("print", "printf"));
        Predefs.Add(("string", "char*"));

        foreach ((string, string) predef in Predefs) {
            data = "#define " + predef.Item1 + " " + predef.Item2 + "\n" + data;
        }

        // Local CImport:
        //  cimport "file.h";
        // C:
        //  #include "file.h"
        RegexReplace(@"cimport\s+""([a-zA-Z0-9_\.]+)"";", "#include \"$1\"");

        // CImport:
        //  cimport stdio.h;
        // C:
        //  #include <stdio.h>
        RegexReplace(@"cimport\s+([a-zA-Z0-9_\.]+);", "#include <$1>");

        // NLang array:
        //  (arr)int myIntArray = [ 1, 2, 3, 4 ]
        // C:
        //  int myIntArray[] = { 1, 2, 3, 4 }
        RegexReplace(@"\((arr)\)([a-zA-Z0-9_]+)\s+([a-zA-Z0-9_]+)\s*=\s*\[(.*)\]", "$2 $3[] = {$4}");

        // NLang array in argument:
        //  (arr)int myIntArray
        // C:
        //  int myIntArray[]
        RegexReplace(@"\((arr)\)([a-zA-Z0-9_]+)\s+([a-zA-Z0-9_]+)", "$2 **$3");

        // NLang Function:
        //  func <name>(<args>) -> <return type> { ... }
        // C:
        //  <return type> <name>(<args>) { ... }
        // Example:
        //  func main(int argc, (arr)string argv) -> int { ... }
        //  int main(int argc, char* argv[]) { ... }
        // Ignore spaces between ( and ) and between [ and ] and -> etc.
        RegexReplace(@"func\s+([a-zA-Z0-9_]+)\s*\((.*)\)\s*->\s*([a-zA-Z0-9_]+)\s*{", "$3 $1($2) {");

        // NLang foreach:
        // foreach (char curChar in arr) { ... } // Where arr is an **char
        // for (int i = 0; i < sizeof(arr)/sizeof(arr[0]); i++) { char curChar = arr[i]; ... }
        RegexReplace(@"foreach\s*\(\s*([a-zA-Z0-9_]+)\s+([a-zA-Z0-9_]+)\s+in\s+([a-zA-Z0-9_]+)\s*\)\s*{", "for (int i = 0; i < sizeof($3)/sizeof($3[0]); i++) { $1 $2 = $3[i];");

        // NLang bodyless foreach:
        // foreach (char curChar in arr) printf("%c", curChar);
        // for (int i = 0; i < sizeof(arr)/sizeof(arr[0]); i++) printf("%c", arr[i]);
        RegexReplace(@"foreach\s*\(\s*([a-zA-Z0-9_]+)\s+([a-zA-Z0-9_]+)\s+in\s+([a-zA-Z0-9_]+)\s*\)\s*([a-zA-Z0-9_]+)\s*\(", "for (int i = 0; i < sizeof($3)/sizeof($3[0]); i++) $4($2 = $3[i], ");

        // NLang pointer:
        //  (ptr)int myInt = 5;
        // C:
        //  int* myInt = 5;
        RegexReplace(@"\((ptr)\)([a-zA-Z0-9_]+)\s+([a-zA-Z0-9_]+)\s*=\s*([a-zA-Z0-9_]+)", "$2* $3 = $4");

        // NLang pointer in argument:
        //  (ptr)int myInt
        // C:
        //  int* myInt
        RegexReplace(@"\((ptr)\)([a-zA-Z0-9_]+)\s+([a-zA-Z0-9_]+)", "$2* $3");

        // NLang class:
        //  class <name> { ... }
        // C:
        //  typedef struct { ... } <name>;
        RegexReplace(@"class\s+([a-zA-Z0-9_]+)\s*{", "typedef struct {");

        

        return data.Split("\r\n");
    }
}