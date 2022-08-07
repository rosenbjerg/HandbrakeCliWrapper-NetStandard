# HandbrakeCliWrapper for NetStandard

This is a basic utility that allows easy use of HandBrakeCLI from C# without having to deal with learning the command line interface. It supports the most common functions, and more are easy to add.

## How to use:

Either clone this repo and add it to your project, or download the package from NuGet.
Once you've added it to your project, add the following lines to the using statements:

```
using HandbrakeCliWrapper;
using HandbrakeCliWrapper.Enums;
using HandbrakeCliWrapper.Models;
```

Then depending on what you want to do, you have a couple options:

You can subscribe to some events from the library - `TranscodingStarted, TranscodingCompleted, and Transcoding Error`
Before you can transcode anything, however, you need to build your config file. All options have defaults that work, but for your use case you should finetune them.

The parameters added in this project were taken from the Handbrake CLI documentation
Refer to handbrake documentation for further details
https://handbrake.fr/docs/en/1.5.0/cli/command-line-reference.html

For example, to encode video to be as compatible as possible for PLeX, I use the following settings:

```
HandbrakeCliConfigBuilder config = new HandbrakeCliConfigBuilder();
config.Encoder = HandbrakeCLIwrapper.Encoder.x264;
config.AudioCopyMask.Add(AudioCopyMask.aac);
config.AudioEncoder = AudioEncoder.av_aac;
config.Format = Format.av_mp4;
config.WebOptimize = true;
```

To then convert:

```
HandbrakeCli conv = new HandbrakeCli();
conv.Transcode(config, <input file path>, <output folder>, <optional output filename>, <optional overwrite output>, <optional remove source file after conversion>
```

You can also track your conversion progress by simply checking `conv.Status.Percentage`

`ProgressBar.Value = (int)conv.Status.Percentage;`

This project is licensed under MIT. See `License.MD` for details.
