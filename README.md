# PS-Reptile (a MAML generator for binary PowerShell modules)

Unlike script modules, authoring help content for binary modules often involves manually authoring a `.xml` help file in [MAML](https://en.wikipedia.org/wiki/Microsoft_Assistance_Markup_Language) (Microsoft Assistance Markup Language) format.
If, like me, you think this [sounds incredibly tedious](https://msdn.microsoft.com/en-us/library/bb525433.aspx#code-snippet-1), then perhaps this tool will be useful. It examines the primary assembly for a binary module (and, optionally, its XML documentation comments) and generates the corresponding MAML help content.

## Custom attribute example 1 (inline help)

```csharp
[Cmdlet(VerbsCommon.Get, "Foo")]
[OutputType(typeof(FooConnectionProfile))]
[CmdletHelp("Retrieve information about one or more Foo connection profiles.")]
public class GetFooConnection
{
    [Parameter(HelpMessage = "Retrieve all connection profiles")]
    public SwitchParameter All { get; set; }
}
```

## Custom attribute example 1 (inline help)

```csharp
[Cmdlet(VerbsCommon.Get, "Foo")]
[OutputType(typeof(FooConnectionProfile))]
[CmdletHelpFromFile("Help/Connections/GetFooConnection/Cmdlet.txt")]
public class GetFooConnection
{
    [Parameter]
    [ParameterHelpFromFile("Help/Connections/GetFooConnection/All.txt")]
    public SwitchParameter All { get; set; }
}
```

## XML documentation comments example

```csharp
/// <summary>Retrieve information about one or more Foo connection profiles.</summary>
[Cmdlet(VerbsCommon.Get, "Foo")]
[OutputType(typeof(FooConnectionProfile))]
public class GetFooConnection
{
    /// <summary>Retrieve all connection profiles.</summary>
    [Parameter]
    public SwitchParameter All { get; set; }
}
```

## Mix-and-match example

```csharp
/// <summary>Retrieve information about one or more Foo connection profiles.</summary>
[Cmdlet(VerbsCommon.Get, "Foo")]
[OutputType(typeof(FooConnectionProfile))]
public class GetFooConnection
{
    [Parameter(HelpMessage = "Retrieve all connection profiles")]
    public SwitchParameter All { get; set; }
}
```

This project is very much a work-in-progress:

* If you have questions or comments, feel free to raise an issue.
* If you'd like to pitch in, any and all assistence will be greatly appreciated :-)

Eventually, we'll also implement a `dotnet` command-line plugin to invoke this tool.
