# OptionBuildSettings

[Source Documentation](xref:sttz.Trimmer.Options.OptionBuildSettings)

Options can influence the build settings used for a build started from a Build Profile (but not regular Unity builds). OptionBuildSettings provides a basic set of options to set the `BuildOptions` flags, the output path and scenes included in the build.

A profile build starts out with no `BuildOptions` flags, no output location set and with the scenes defined in Unity's Build Settings.

### Build Options

Toggle the flags provided by Unity's `BuildOptions` enumeration. Note that it's only possible to set additional flags, not unsetting flags that other Options might have set.

See [Unity's documentation](https://docs.unity3d.com/ScriptReference/BuildOptions.html) for information on the individual flags.

### Build Path

Set an output path where the build will be placed. Leave this Option empty to show a save dialog to choose the build location.

The Option supports a set of variables in the path that will be expanded:
* `%Target%`: The name of the build target (see [BuildTarget](https://docs.unity3d.com/ScriptReference/BuildTarget.html))
* `%Group%`: The name of the build target group (see [BuildTargetGroup](https://docs.unity3d.com/ScriptReference/BuildTargetGroup.html))
* `%Version%`: The version defined in [PlayerSettings.bundleVersion](https://docs.unity3d.com/ScriptReference/PlayerSettings-bundleVersion.html)
* `%ProductName%`: The product name defined in Unity's Player Settings
* `%CompanyName%`: The company name defined in Unity's Player Settings
* `%UnityVersion%`: The version of Unity
* `%Development%`: "dev" if `BuildOptions.Development` is set or an empty string otherwise

### Scenes

The list of scenes to include in the build. Leave this emtpy to use the default list set in Unity's Build Settings.