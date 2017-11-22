# OptionPrompt

[Source Documentation](xref:sttz.Trimmer.Options.OptionPrompt)

The prompt is a simple text-based GUI interface to edit Options in the player.

The interface mimics a terminal with a `>` showing up and letting the user input commands.

The prompt supports Option name and value expansion. Start entering the name of an Option and press tab to expand the current prefix. Use arrow up and down to cycle through the Options that match the current prefix. The expanded Options will include their values and typing will start editing the value. Enter saves the value and clears the prompt.

The syntax is the same as in ini files, see [OptionIniFile](ini_file.md) for more details.

### Prompt

Enable or disable the prompt.

### Prompt Font Size

Set the font size of the prompt. `0` uses Uniy's default font size.

### Prompt Position

The position of the prompt on the screen.

### Prompt Activation

The activation sequence to open the prompt. The user needs to press the keys in the sequence in order without pressing any other key in-between. The keys are separted by a dash and the key names are taken from Unity's [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html) enumeration.