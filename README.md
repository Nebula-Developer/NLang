# NLang, the C we all want
NLang is a simple and easy to use programming language that compiles to C.<br />
It is designed to simplify and restructure the syntax of C, while keeping it safe, efficient, and still related.

<br />

## Why NLang?
Switching languages can be hard - and why should you have to?<br />
Each feature that I ended up adding was thought through fully, and I believe that NLang is a great language for beginners and experts alike, being powerful enough to be used for complex programs, and simple enough to be used for small programs.<br />
Due to C being such a popular language, change can be hard - especially for things as big as programming languages.<br />
But guess what? In NLang, you are able to write fully native C code, and also NLang side by side. (Stack overflow answers will still be useful, and you can use any C library you want!)

<br />

## Build
To build NLang, simply run:
```make detect```<br />
This will build NLang, and then (if linux/mac) copy it to your path.*

<br />

If you would like to build NLang for a specific OS, run:

```make build-<OS>``` - (OS is either linux, mac, or windows)

<br />

And, if you would like to copy it to your path, run:

```make install-<OS>``` *

<br />

Path locations:

Linux - /usr/bin/nlang<br/>
Mac - /usr/local/bin/nlang

<br />

<small>*Windows install is not implemented yet. Please copy the executable to your PATH manually.</small>

<br />

## Usage
NLang is a command line tool, and can be used like this:

```nlang [options] [file]```

Lets say we want to compile a simple hello world program, we can do this:<br />
### hello.nl:

```
cimport stdio.h;

func main(int args, (arr)string argv) -> int {
    printf"Hello, world!\n");
    return 0;
}
```
Then we can compile it with:

```nlang -o hello hello.nl```

Now we have our executable:

```./hello```

<br />

## Documentation
Documentation is currently being worked on, and will be available soon. <br />
For now, you can check out the examples folder for some examples of NLang code.

<br />

## Contributing
Contributions are welcome! Please read the [contributing guidelines](CONTRIBUTING.md) before contributing.