# Legend of Cube
3D platformer using XNA starring a Cube.

## Setting up Development Environment

### Requirements
- __Visual Studio 2013__ or newer
- __XNA 4.0__

### Install XNA
1. Download `XNA Game Studio 4.0.4.zip` from: https://msxna.codeplex.com/releases/view/117230
2. Extract the archive and follow instructions in the contained README.

### Open Project
Find `LegendOfCube.sln` and open in Visual Studio.

## Coding Style
Follow Microsoft's C# Guidelines, see [general rules](https://msdn.microsoft.com/en-us/library/ff926074.aspx) and [about naming](https://msdn.microsoft.com/en-us/library/ms229002(v=vs.110).aspx), but with some exceptions and additional rules described below.

### Constants
It as allowed to use `ALL_UPPERCASE_CONSTANT` over the recommended `UpperCamelCaseConstant` if it would make things clearer.

### Indentation
It's preferred to do the leading indentation to the scope level using tabs, and use spaces for additional indentation. As in:

```c#
void Foo()
{
--->if (true)
--->{
--->--->DoSomething(withThisArgument,
--->--->............andThisOne);
--->}
}
```

(where `--->` is tab and `.` is a space.)

- Due to problems with Visual Studio, it is ok to use tabs for additional indentation.
- To use tabs by default in Visual Studio, enable "Keep tabs" in "Tools > Options... > Text Editor > C# > Tabs".

## Distribution
### Windows
Installation on Windows is done through an installer program. See [separate repo](https://github.com/helmertz/legendofcube-installer-windows) for info about how to create one.

## License
TBD
