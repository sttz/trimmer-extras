# Creating Options
This article explains the basic concepts needed to create your own options.

Options represent configuration values and receive many different callbacks to apply their configuration while playing or during the build process.

An Option's value is based on a single line string. To represent more complex set of values, use [child](#child-options) and [variant](#variant-options) options.

Trimmer comes [bundled](bundled_options.md) with a collection of Options, which can serve as a starting point and reference for implementing your own.

## Basic Option

The simplest Option you can create looks like this:
```` cs
// Trimmer creates a conditional compilation symbol to 
// only compile the Option when set in the Build Profile. 
// Always wrap Options in an #if condition like this:
#if TR_OptionSimple || UNITY_EDITOR

using UnityEngine;
using sttz.Trimmer.BaseOptions;

// The convention is to prefix your Option class with 
// "Option". The Option will be referred to in the editor, 
// ini files and the prompt without the "Option" prefix 
// (just "Simple" in this case):
public class OptionSimple : OptionToggle
{
    // All necessary configuration should be done in the 
    // Configure method. Do not override the constructor. 
    // Most properties should only be set once in Configure 
    // and not changed afterwards.
    protected override void Configure()
    {
        Category = "Debug";
    }

    // Apply is called once when the game is played in the 
    // editor or in a build and whenever the Option's Value 
    // is changed.
    public override void Apply()
    {
        base.Apply();
        Debug.Log("Apply OptionSimple with " + Value);
    }
}
#endif
````

Extend the base class that matches the value type you need:
* [OptionAsset](xref:sttz.Trimmer.BaseOptions.OptionAsset`1)
* [OptionEnum](xref:sttz.Trimmer.BaseOptions.OptionEnum`1)
* [OptionFloat](xref:sttz.Trimmer.BaseOptions.OptionFloat)
* [OptionInt](xref:sttz.Trimmer.BaseOptions.OptionInt)
* [OptionString](xref:sttz.Trimmer.BaseOptions.OptionString)
* [OptionToggle](xref:sttz.Trimmer.BaseOptions.OptionToggle)
* [OptionContainer](xref:sttz.Trimmer.BaseOptions.OptionContainer)

Use these classes as references as to how to implement your own if you need another value type. Each Option receives a string to parse in [Load](xref:sttz.Trimmer.Option.Load*), needs to serialize it back in [Save](xref:sttz.Trimmer.Option.Save) and can display a custom edit GUI in [EditGUI](xref:sttz.Trimmer.Option.EditGUI*).

The [Apply](xref:sttz.Trimmer.Option.Apply) method is called when you need to act on your Option's [Value](xref:sttz.Trimmer.Option`1.Value). This is whenever the project is played in the editor, a built player is opened or an Option's value is changed during playback.

Your Option's Apply method can e.g. find a script in your scene and change one of its fields or inject a new script into the active scene. If you need to act on scenes being loaded/unloaded during playback, use Unity's `SceneManager` API.

## Child Options
An Option can only have a single value. If you need to group together a number of different values, you can use child Options.

```` cs
#if TR_OptionSimple || UNITY_EDITOR

using UnityEngine;
using sttz.Trimmer.BaseOptions;

public class OptionSimple : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    public override void Apply()
    {
        base.Apply();
        var childValue = GetChild<OptionSimpleChild>().Value;
        Debug.Log("Apply OptionSimple with " + Value + " / " + childValue);
    }

    // Simply nest Option classes inside your main Option.
    // No addditional setup or configuration required.
    public class OptionSimpleChild : OptionString { }
}
#endif
````

An Option can have any number of children and each child can contain children as well. The nested Option classes are detected automatically and there's no additional set up required.

The main Option and its children are always applied together, so if the main Option's or its children's value changes, [Apply](xref:sttz.Trimmer.Option.Apply) is called on all of them.

Only a limited set of configuration properties are available for child Options. Check the documentation to see which properties and methods of the Option class apply to child Options.

## Variant Options
From the main and child Options, only a single instance is ever created. Sometimes you want to have a single Option class but have multiple instances of it.

Trimmer uses variant Options for this case. Variant options have a parameter in addition to the value and Trimmer automatically creates instances for each different parameter.

```` cs
#if TR_OptionSimple || UNITY_EDITOR

using UnityEngine;
using sttz.Trimmer.BaseOptions;

public class OptionSimple : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
        
        // Setting Variance is all that's required to make 
        // your Option variant. You can also set it to
        // OptionVariance.Array if you don't care about the
        // parameter or need the variants to be ordered 
        // explicitly.
        Variance = OptionVariance.Dictionary;

        // In case you set Variance to Dictionary, it's best
        // to also set the default parameter to something
        // different than "Default".
        VariantDefaultParameter = "Master";
    }

    public override void Apply()
    {
        base.Apply();
        Debug.Log("Apply OptionSimple with [" + VariantParameter + "] = " + Value);
    }
}
#endif
````

All you have to do is set [Variance](xref:sttz.Trimmer.Option.Variance) to either [Dictionary](xref:sttz.Trimmer.OptionVariance.Dictionary) or [Array](xref:sttz.Trimmer.OptionVariance.Array). In the profile editor you can then create additional variants.

Dictionary variants can have their parameter freely configured in the editor, while Array variants have their parameter automatically assigned to an index.

Variant Options always have at least one instance created, even if there aren't any variants configured, so that the Option can always do some basic setup or interact with the build process. This instance will use the [VariantDefaultParameter](xref:sttz.Trimmer.Option.VariantDefaultParameter) and you cannot remove this variant in the editor.

## Build Processing
Options can hook into different stages of the build process:

```` cs
#if TR_OptionSimple || UNITY_EDITOR

using UnityEngine;
using sttz.Trimmer.BaseOptions;

public class OptionSimple : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    // Method called for profile builds but not regular Unity builds.
    // This callback gives Options the chance to edit the
    // BuildPlayerOptions that will be used to start the build.
    // E.g. set options.locationPathName to where you want 
    // your build to be saved and not be asked in the editor
    // or set additional build flags in options.options.
    public override BuildPlayerOptions PrepareBuild(BuildPlayerOptions options, OptionInclusion inclusion)
    {
        return options;
    }

    // Method called before a build starts.
    // Here you can modify assets or do other preparatory
    // steps before the build starts.
    // (This method is equivalent to Unity's IPreprocessBuild)
    public override void PreprocessBuild(BuildTarget target, string path, OptionInclusion inclusion)
    {
        // Actions before the build starts
    }

    // Method called for every scene beind built.
    // Use this callback to modify scenes as they are built.
    // You can e.g. delete some game objects / scripts or
    // inject new scripts.
    // (This method is equivalent to Unity's IProcessScene 
    // but is not called when playing in the editor)
    public override void PostprocessScene(Scene scene, bool isBuild, OptionInclusion inclusion)
    {
        // Edit a scene during build
    }

    // Method called after the build has finished.
    // Use this to edit the built project or do some cleanup
    // after the build.
    // (This method is equivalent to Unity's IPostprocessBuild)
    public override void PostprocessBuild(BuildTarget target, string path, OptionInclusion inclusion)
    {
        // Actions after build finished
    }
}
#endif
````

You only need to implement the events that you need.

Unlike Unity's `IProcessScene` callback, [PostprocessScene](sttz.Trimmer.Option.PostprocessScene*) is *not* called when playing in the editor. Use [Apply](xref:sttz.Trimmer.Option.Apply) instead, which is called when the game starts playing in the editor or in a build.

## Option Lifecycle
Options instances are created in the background at different times in the editor and player. You should not rely on Options instances being created at specific times and you should not place code in their constructors or destructors (use [Configure](xref:sttz.Trimmer.Option.Configure) instead).

Option instances are also created at edit-time to check their configuration. It's therefore critical that most of its methods are side-effect free. Especially [Load](xref:sttz.Trimmer.Option.Load*), [Save](xref:sttz.Trimmer.Option.Save) and [EditGUI](xref:sttz.Trimmer.Option.EditGUI*) can be can be called at any time and you should be careful when relying on external state.

At edit-time, only one instance is created per Option type, meaning the same instance will be used to edit all variants of an Option.

## Associated Feature
For some Options you want to configure a feature that can be included or removed from the build independent of the Option.

E.g. a Steam integration Option that sets the App ID and injects a script that handles the Steam integration. For debug builds, you might want to include the Option so that you can change the App ID in the player. For release builds however, you only want to include the feature and not make the App ID configurable. And when you make a build for another platform, you don't want to include the Steam integration code at all.

If you set the [HasAssociatedFeature](xref:sttz.Trimmer.OptionCapabilities.HasAssociatedFeature) capability of your Option ([see below](#capabilities)), two things will change:
* The Option will set a `TR_Name` conditional compilation symbol in addition to the `TR_OptionName` symbol. You can use this symbol to conditionally compile the feature independently of its Option.
* In the Build Profile editor, you will be able to include either the Option and feature, only the feature or remove both, which defines the conditional compilation symbols accordingly.

## Capabilities
Options have capabilities that define how an Option interacts with different parts of the Trimmer workflow.

Use the [CapabilitiesAttribute](xref:sttz.Trimmer.CapabilitiesAttribute) and [OptionCapabilities](xref:sttz.Trimmer.OptionCapabilities) enumeration to define the capabilities of your Option.

### Flags

* [HasAssociatedFeature](xref:sttz.Trimmer.OptionCapabilities.HasAssociatedFeature)<br>
  Indicates the Option has an associated feature. This sets an additional conditional compilation symbol and shows additional options in the Build Profile editor. See [Associated Feature](#associated-feature) for more information.

* [CanIncludeOption](xref:sttz.Trimmer.OptionCapabilities.CanIncludeOption)<br>
  Indicates the Option can be included in builds. If this capability is not set, the Option will always be removed from builds (e.g. Options that only work in the editor like a build post-processor Option).

* [ConfiguresBuild](xref:sttz.Trimmer.OptionCapabilities.ConfiguresBuild)<br>
  Indicates the Option will interact with the build process. If this capability is not set, the build callbacks will not be called.

* [CanPlayInEditor](xref:sttz.Trimmer.OptionCapabilities.CanPlayInEditor)<br>
  Indicates the Option should be loaded when playing in the editor. If not set, the Option will not be loaded and it's Apply method will not be called.

* [ExecuteInEditMode](xref:sttz.Trimmer.OptionCapabilities.ExecuteInEditMode)<br>
  Indicates the Option should be loaded when not playing in the editor. This capability is similar to Unity's `ExecuteInEditMode` attribute and allows the Option to interact with the editor while not playing.

The OptionCapabilities enumeration is a bitmask you can combine the flags outline above with the `|` operator, .e.g:

`OptionCapabilities.HasAssociatedFeature | OptionCapabilities.CanIncludeOption`

Depending on the capabilities, Options are not displayed in the Editor or Build Profiles:
* For an Option to be shown in Build Profiles, either [CanIncludeOption](xref:sttz.Trimmer.OptionCapabilities.CanIncludeOption), [HasAssociatedFeature](xref:sttz.Trimmer.OptionCapabilities.HasAssociatedFeature) or [ConfiguresBuild](xref:sttz.Trimmer.OptionCapabilities.ConfiguresBuild) must be included.
* For an Option to be shown in the Editor Profile, either [CanPlayInEditor](xref:sttz.Trimmer.OptionCapabilities.CanPlayInEditor) or [ExecuteInEditMode](xref:sttz.Trimmer.OptionCapabilities.ExecuteInEditMode) must be included.

### Presets
Instead of combining the capabilities, you can also use one of the included presets:

* [PresetDefault](xref:sttz.Trimmer.OptionCapabilities.PresetDefault)<br>
  The default capabilities used when no [CapabilitiesAttribute](xref:sttz.Trimmer.CapabilitiesAttribute) is applied. Consists of [CanIncludeOption](xref:sttz.Trimmer.OptionCapabilities.CanIncludeOption), [ConfiguresBuild](xref:sttz.Trimmer.OptionCapabilities.ConfiguresBuild) and [CanPlayInEditor](xref:sttz.Trimmer.OptionCapabilities.CanPlayInEditor).

* [PresetWithFeature](xref:sttz.Trimmer.OptionCapabilities.PresetWithFeature)<br>
  Same as [PresetDefault](xref:sttz.Trimmer.OptionCapabilities.PresetDefault) but also includes [HasAssociatedFeature](xref:sttz.Trimmer.OptionCapabilities.HasAssociatedFeature).

* [PresetOptionOnly](xref:sttz.Trimmer.OptionCapabilities.PresetOptionOnly)<br>
  A runtime-only Option that does not interact with the build process. Includes [CanIncludeOption](xref:sttz.Trimmer.OptionCapabilities.CanIncludeOption) and [CanPlayInEditor](xref:sttz.Trimmer.OptionCapabilities.CanPlayInEditor).

## Availability
You can limit for which platforms an Option is available by setting its [SupportedTargets](xref:sttz.Trimmer.Option.SupportedTargets) property. If a Build Profile doesn't include any of the Option's supported targets, the Option will be hidden.

In Trimmer's section in Unity's preferences you can toggle if unavailable Options should be displayed. Unavailable Options will be greyed out and always removed from the build.

## Conditional Compilation
Each main Option will define up to two conditional compilation symbols depending on its capabilities.

The Option's class name is used to determine the name of these symbols. The process is as follows:
* The "Option" prefix will be removed from the class name to create the Option's name.
* The symbol prefix (`TR_`) and the Option prefix (`Option`) will be added to the Option's name to create the Option's symbol (e.g. `TR_OptionName`).
* The symbol prefix (`TR_`) will be added to the Option's name to create the feature symbol (e.g. `TR_Name`).

Examples:
* `OptionName` &#x2192; `TR_OptionName` and `TR_Name`
* `MyOption` &#x2192; `TR_OptionMyOption` and `TR_MyOption`

The Option symbol will only be defined if the [CanIncludeOption](xref:sttz.Trimmer.OptionCapabilities.CanIncludeOption) capability is set and the Build Profile includes the Option.

The feature symbol will only be defined if the [HasAssociatedFeature](xref:sttz.Trimmer.OptionCapabilities.HasAssociatedFeature) capability is set and the Build Profile includes the feature.

You can also override [GetScriptingDefineSymbols](xref:sttz.Trimmer.Option.GetScriptingDefineSymbols*) to define additional symbols.
