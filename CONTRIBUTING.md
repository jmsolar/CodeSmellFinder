# Contributing to CodeSmellFinder proyect
This is the contribution guide for CodeSmellFinder. Great to have you here! Here are a few ways you can help make this project better.

## Creating issues
Do you have an idea for a feature or have you found a bug? Please create an issue so we can talk about it!

## Adding new visitors
New features are welcome! Either as requests or proposals.

* Please create an issue first, so we know what to expect.
* Create a fork on your github account.
* Add a new class similar like *TernaryEvaluationVisitor* (SmellFinder/Visitors/TernaryEvaluationVisitor.cs)
* Sertup a decorator attribute like this

Example: 

```c#
[Visitor("MyNewVisitorExample", Description = "Any description message")]
```

* The new class must be a subclass from *BaseVisitor*.
* Implements the logical for search a bad smell in .js files

#### Steps to run SmellFinderTool on a local project
*  Clone the repository `https://github.com/jmsolar/CodeSmellFinder.git`
*  Open `CodeSmellFinder.sln`
*  Setup as startup project `SmellFinderTool`
*  Run the program
