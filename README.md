# CodeSmellFinder
*Searching bad smells!..*
## Introduction

CodeSmellFinder is a C# project that offers detect and report JavaScript *custom* smells. We says custom cause you writes the rule for that. Check it out!

## Compatibility

CodeSmellFinder library target version:
 - dotnet core 3.1

CodeSmellFinder tool (console app):
 - dotnet core 3.1

## Usage

```bash
# Easy! just run..
dotnet SmellFinderTool.dll
```

## Screenshots

 - Selector folder to analize files
 
![image](https://user-images.githubusercontent.com/19495643/177678875-2d44b25b-c0d6-466e-986a-f7885996690b.png)

 - Analizing .js files
 
![image](https://user-images.githubusercontent.com/19495643/177678949-609a43ee-b595-49fd-a235-39ef090914f3.png)

 - Report file JSON generated
 
![image](https://user-images.githubusercontent.com/19495643/177678990-22328775-1e85-4560-8954-b4e6374d0981.png)

![image](https://user-images.githubusercontent.com/19495643/177679697-14216aae-c585-4b63-bc41-a89e58eaf910.png)

## Adding new visitors
New visitor extensions are welcome! Check this guide to add visitors

## Contributing

Want to help develop CodeSmellFinder project? Check out our [contribution guide](/CONTRIBUTING.md).

Issues should be issued at https://github.com/jmsolar/CodeSmellFinder/tree/testing-issue

## Aditional librarys used
*  Antlr4 tool https://github.com/antlr/antlr4
*  Antlr4 gramars https://github.com/antlr/grammars-v4/tree/master/javascript/javascript/CSharp
*  Spectro https://github.com/spectreconsole/spectre.console 
