# OptionIniFile

[Source Documentation](xref:sttz.Trimmer.Options.OptionIniFile)

Load an ini file in the player to change a build's configuration.

The ini file can contain one value per line, in the format:

`OptionName = Value`

Child Options are delimited with a dot:

`Option.Child = Value`

Variant parameters are enclosed in square brackets:

`Option[Parameter] = Value`

Lines starting with `#` or `//` are treated as comments and ignored.

Values and parameters can be enclosed with double quotes, double quotes inside double quoted strings need to be escaped with a backslash. If not enclosed in double quotes, the value will be trimmed.

`Option["My Parameter"] = " My Value "`

> [!NOTE]
> You can export and import configuration in ini format from any Build Profile's gear menu.

### Ini File

The name of the ini file to be loaded. Does not need to have an ini extension (e.g. txt can be easier to open).

### Search Path

The paths that will be searched for the ini file. Paths should be separted by `;` and will be searched in order until a matching file has been found. Only the first file found will be loaded.

The path supports a few variables that will be expanded:
* `%DataPath%`: Will be replaced by [Application.dataPath](https://docs.unity3d.com/ScriptReference/Application-dataPath.html)
* `%PersistentDataPath%`: Will be replaced by [Application.persistentDataPath](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)
* `%Personal%`: Will be replaced by path to `SpecialFolder.Personal` (the users' home directory)
* `%Desktop%`: Will be replaced by path to `SpecialFolder.DesktopDirectory` (the users' desktop directory)
