using System.Diagnostics;
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
        Predefs.Add(("bool", "int"));
        Predefs.Add(("true", "1"));
        Predefs.Add(("false", "0"));

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

        // NLang class:
        //  class <name> { ... }
        // C:
        //  typedef struct { ... } <name>;
        RegexReplace(@"class\s+([a-zA-Z0-9_]+)\s*{", "typedef struct {");

        // NLang array modifier:
        //  (arr)int arr = [1, 2, 3];
        // C:
        //  int arr[] = {1, 2, 3};
        // Also:
        //  (arr->100)char myString; // where ->100 is the length of the array
        //  char myString[100];
        RegexReplace(@"\(\s*arr\s*->(\s*[0-9]+)?\s*\)\s*([a-zA-Z0-9_]+)\s*([a-zA-Z0-9_]+)", "$2 $3[$1]");
        RegexReplace(@"\(\s*arr\s*(\s*[0-9]+)?\s*\)\s*([a-zA-Z0-9_]+)\s*([a-zA-Z0-9_]+)", "$2 $3[]");
        RegexReplace(@"\(\s*array\s*->(\s*[0-9]+)?\s*\)\s*([a-zA-Z0-9_]+)\s*([a-zA-Z0-9_]+)", "$2 $3[$1]");
        RegexReplace(@"\(\s*array\s*(\s*[0-9]+)?\s*\)\s*([a-zA-Z0-9_]+)\s*([a-zA-Z0-9_]+)", "$2 $3[]");

        // NLang enum:
        //  enum <name> { ... }
        // C:
        //  typedef enum { ... } <name>;
        RegexReplace(@"enum\s+([a-zA-Z0-9_]+)\s*{", "typedef enum {");

        // NLang if quick:
        //  a == b ? { ... } : { ... }
        // C:
        //  if (a == b) { ... } else { ... }
        // Example:
        //  p == s[0] ? {
        //     printf("p == s[0]\n");
        //  } : {
        //     printf("p != s[0]\n");
        //  }
        //  if (p == s[0]) {
        //     printf("p == s[0]\n");
        //  } else {
        //     printf("p != s[0]\n");
        //  }
        List<string> operators = new List<string>() {
            "==", "!=", "<", ">", "<=", ">="
        };

        foreach (string op in operators) {
            RegexReplace(@"\s*(\S+)\s*" + op + @"\s*(\S+)\s*\?\s*{", "\n\tif ($1 " + op + " $2) {");
        }

        // :
        RegexReplace(@"}\s*:\s*{", "} else {");

        // NLang string variable equality:
        //  (a s== b)
        // C:
        //  (strcmp(a, b) == 0)
        // Example:
        //  if (a s== b && c s== d) { ... }
        //  if (strcmp(a, b) == 0 && strcmp(c, d) == 0) { ... }
        RegexReplace(@"\(\s*(\S+)\s*s==\s*(\S+)\s*", "(strcmp($1, $2) == 0 ");
        RegexReplace(@"\s*(\|\||&&)\s*(\S+)\s*s==\s*(\S+)\s*\)", " $1 strcmp($2, $3) == 0)");
        RegexReplace(@"\s*(\|\||&&)\s*(\S+)\s*s==\s*(\S+)\s*", " $1 strcmp($2, $3) == 0 ");

        // NLang jump:
        //  jump <label>;
        // C:
        //  goto <label>;
        RegexReplace(@"jump\s+(.+);", "goto $1;");

        // NLang zero:
        //  zero <var>;
        // C:
        //  memset(&<var>, 0, sizeof(<var>));
        RegexReplace(@"zero\s+(.+);", "memset(&$1, 0, sizeof($1));");



        return data.Split("\r\n");
    }
}