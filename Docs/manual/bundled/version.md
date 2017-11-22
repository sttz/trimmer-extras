# OptionVersion

[Source Documentation](xref:sttz.Trimmer.Options.OptionVersion)

Version manages the version and build numbers of the project.

Including this Option's feature in the build is equivalent to enabling version handling for that build. Not including the feature will completely disable OptionVersion.

OptionVersion will manage the build number across all build targets. Specifically, it will check all build targets supporting build numbers, choose the highest build number, increment it and save it back to all build targets with a build number.

At runtime, [Version.ProjectVersion](xref:sttz.Trimmer.Version.ProjectVersion) can be accessed to get the version information of the current build.

### Override Version

By default, `PlayerSettings.bundleVersion` bundle version will be used. Enter a version string here to override the version number for this build.

### Override Build

By default, the highest build number across platforms will be incremented and used. Enter a build number here to override the number used for this build.

When a build number is entered here, it will also be incremented on every build.

### Version Control

When enabled, information from the used version control will be included.

Currently only Git is supported and the current commit hash and branch name will be included in the version information.

## Script Settings

There are two additional settings that can be made in the OptionVersion script file for the whole project:

### Share Build Number

Disable sharing of build numbers. When disabled, the build numbers of each platform will not be synchronized and kept separate.

### Increment Build Number

Disable automatic increment of build numbers. When disabled, the build numbers must be incremented manually for every build.