# Build

Update [build.bat](..\build.bat)

`if "%1"=="8.#" goto build-8.#`

Add a new section for your version of the ![SMP](images/smp.png) SMP.

```batch
:build-8.#
set build=8.#

set gac=C:\Windows\Microsoft.NET\assembly\GAC_MSIL
set csc=@c:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

set ver1=v4.0_8.#.####.0__d516cb311cfb6e4f
set ver2=v4.0_8.#.####.0__d516cb311cfb6e4f
set ver3=v4.0_8.#.####.0__99b1e4cc0d03f223

goto build
```

Update the version # [SWDExecTrending.cs](..\SWDExecTrending.cs)

```c#
public static readonly int version = 5;
```

Update the [AssemblyInfo.cs](..\AssemblyInfo.cs)

```
[assembly: AssemblyVersion("#.0.0.0")]
[assembly: AssemblyFileVersion("#.0.0.0")]
```

---

### TODO

Use the version number from AI in the main `cs` file instead.

```c#
System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
string version = fvi.FileVersion;
```

Or

```c#
string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion; 
string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
```