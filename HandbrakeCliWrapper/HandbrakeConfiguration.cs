using HandbrakeCliWrapper.Enums;
using HandbrakeCliWrapper.Models;
using HandbrakeCliWrapper.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandbrakeCliWrapper {
    /// <summary>
    /// Encoding configuration for HandbrakeCLI
    /// The ToString method is overridden to return a string that can be used as argument to HandbrakeCLI
    /// Reference and comments taken from Handbrake documentation 
    /// https://handbrake.fr/docs/en/1.5.0/cli/command-line-reference.html
    /// </summary>
    public class HandbrakeConfiguration {
        #region source options
        /// <summary>
        /// Select a title to encode (0 to scan all titles only, default: 1)
        /// </summary>
        public int? Title { get; set; }
        /// <summary>
        /// Set the minimum title duration (in seconds). Shorter titles will be ignored(default: 10).
        /// </summary>
        public int? MinDurationSeconds { get; set; }
        /// <summary>
        /// Scan selected title only.
        /// </summary>
        public bool ScanSelectedTitleOnly { get; set; }
        /// <summary>
        /// Detect and select the main feature title.
        /// </summary>
        public bool MainFeature { get; set; }
        /// <summary>
        /// Select chapters (e.g. "1-3" for chapters 1 to 3 or "3" for chapter 3 only, default: all chapters)
        /// </summary>
        public IList<string> Chapters { get; set; }
        /// <summary>
        /// Select the video angle (DVD or Blu-ray only)
        /// </summary>
        public int? Angle { get; set; }
        /// <summary>
        /// Select how many preview images are generated, and whether to store to disk(0 or 1). (default: 10:0)
        /// </summary>
        public Preview Previews { get; set; }
        /// <summary>
        /// Start encoding at a given preview.
        /// </summary>
        public int? StartAtPreview { get; set; }
        /// <summary>
        /// Start encoding at a given offset in seconds, frames, or pts(on a 90kHz clock) (e.g.seconds:10, frames:300, pts:900000). Units must match --stop-at units, if specified.
        /// </summary>
        public Timepoint StartAt { get; set; }
        /// <summary>
        /// Stop encoding after a given duration in seconds, frames, or pts(on a 90kHz clock) has passed (e.g.seconds:10, frames:300, pts:900000). Duration is relative to --start-at, if specified. Units must match --start-at units, if specified.
        /// </summary>
        public Timepoint StopAt { get; set; }
        #endregion

        #region destination options
        /// <summary>
        /// Select container format (default: auto-detected from destination file name))
        /// </summary>
        public Format Format { get; set; }
        /// <summary>
        /// Add chapter markers
        /// </summary>
        public bool? Markers { get; set; }
        /// <summary>
        /// Disable preset chapter markers
        /// </summary>
        public bool? NoMarkers { get; set; }
        /// <summary>
        /// Optimize MP4 files for HTTP streaming (fast start, s.s.rewrite file to place MOOV atom at beginning)
        /// </summary>
        public bool? WebOptimize { get; set; }
        /// <summary>
        /// Disable preset 'optimize'
        /// </summary>
        public bool? NoWebOptimize { get; set; }
        /// <summary>
        /// Add iPod 5G compatibility atom to MP4 container
        /// </summary>
        public bool? IpodAtom { get; set; }
        /// <summary>
        /// Disable iPod 5G atom
        /// </summary>
        public bool? NoIpodAtom { get; set; }
        /// <summary>
        /// Add audio silence or black video frames to start of streams so that all streams start at exactly the same time
        /// </summary>
        public bool? AlignAv { get; set; }
        /// <summary>
        /// Create adaptive streaming compatible output. Inserts parameter sets(SPS and PPS) inline in the video stream before each IDR.
        /// </summary>
        public bool? InlineParameterSets { get; set; }
        #endregion

        #region video options
        /// <summary>
        /// Select video encoder
        /// </summary>
        public VideoEncoder Encoder { get; set; }
        /// <summary>
        /// Adjust video encoding settings for a particular speed/efficiency tradeoff (encoder-specific)
        /// </summary>
        public string EncoderPreset { get; set; }
        /// <summary>
        /// List supported --encoder-preset values for the specified video encoder
        /// </summary>
        public IList<string> EncoderPresets { get; set; }
        /// <summary>
        /// Adjust video encoding settings for a particular type of source or situation(encoder-specific)
        /// </summary>
        public string EncoderTune { get; set; }
        /// <summary>
        /// List supported --encoder-tune values for the specified video encoder
        /// </summary>
        public IList<string> EncoderTunes { get; set; }
        /// <summary>
        /// Specify advanced encoding options in the same style as mencoder(all encoders except theora) : option1=value1:option2=value2
        /// </summary>
        public IList<Encopt> Encopts { get; set; }
        /// <summary>
        /// Ensure compliance with the requested codec profile(encoder-specific)
        /// </summary>
        public string EncoderProfile { get; set; }
        /// <summary>
        /// List supported --encoder-profile values for the specified video encoder
        /// </summary>
        public IList<string> EncoderProfiles { get; set; }
        /// <summary>
        /// Ensures compliance with the requested codec level(encoder-specific)
        /// </summary>
        public EncoderLevel EncoderLevel { get; set; }
        /// <summary>
        /// List supported --encoder-level values for the specified video encoder
        /// </summary>
        public IList<EncoderLevel> EncoderLevels { get; set; }
        /// <summary>
        /// Set video quality (e.g. 22.0)
        /// </summary>
        public float VideoQuality { get; set; }
        /// <summary>
        /// Set video bitrate in kbit/s (default: 1000)
        /// </summary>
        public int VideoBitrate { get; set; }
        /// <summary>
        /// Use two-pass mode
        /// </summary>
        public bool? TwoPass { get; set; }
        /// <summary>
        /// Disable two-pass mode
        /// </summary>
        public bool? NoTwoPass { get; set; }
        /// <summary>
        /// When using 2-pass use "turbo" options on the first pass to improve speed (works with x264 and x265)
        /// </summary>
        public bool? Turbo { get; set; }
        /// <summary>
        /// Disable 2-pass mode's "turbo" first pass
        /// </summary>
        public bool? NoTurbo { get; set; }
        /// <summary>
        /// Set video framerate
        ///  (5/10/12/15/20/23.976/24/25/29.97/ 30/48/50/59.94/60/72/75/90/100/120 or a number between 1 and 1000).
        ///  Be aware that not specifying a framerate lets HandBrake preserve a source's time stamps, potentially creating variable framerate video
        /// </summary>
        public float FrameRate { get; set; }
        /// <summary>
        /// Frame-rate setting for the output video
        /// Select variable, constant or peak-limited frame rate control.VFR preserves the source timing.
        /// CFR makes the output constant rate at the rate given by the -r flag(or the source's  average rate if no -r is given).
        /// PFR doesn't allow the rate to go over the rate specified with the -r flag but won't change the source timing if it's below that rate.
        /// If none of these flags are given, the default is --pfr when -r is given and --vfr otherwise
        /// </summary>
        public FrameRateSetting FrameRateSetting { get; set; }
        #endregion

        #region audio options
        /// <summary>
        /// Specify a comma separated list of audio languages you would like to select from the source title.By default, the first audio matching each language will be added to your output. Provide the language's ISO 639-2 code
        ///  (e.g.fre, eng, spa, dut, et cetera) Use code 'und' (Unknown) to match all languages.
        /// </summary>
        public IList<string> AudioLanguages { get; set; }
        /// <summary>
        /// Which audio tracks to process and keep in the output video (combines parameters --all-audio and --first-audio
        /// All Audio: Select all audio tracks matching languages in the specified language list(--audio-lang-list). Any language if list is not specified.
        /// First Audio: Select first audio track matching languages in the specified language list(--audio-lang-list). Any language if list is not specified.
        /// </summary>
        public AudioTracks AudioTracks { get; set; }
        /// <summary>
        /// Select audio track(s), separated by commas ("none" for no audio, "1,2,3" for multiple tracks, default: first one). Multiple output tracks can be used for one input.
        /// </summary>
        public IList<string> Audios { get; set; }
        /// <summary>
        /// Select audio encoder
        ///  "copy:<type>" will pass through the corresponding audio track without modification, if pass through is supported for the audio type.
        /// </summary>
        public IList<AudioEncoder> AudioEncoders { get; set; }
        /// <summary>
        /// Set audio codecs that are permitted when the "copy" audio encoder option is specified
        /// The list of audio codecs to copy rather than transcode
        /// </summary>
        public IList<AudioCopyMask> AudioCopyMasks { get; set; }
        /// <summary>
        /// Set audio codec to use when it is not possible to copy an audio track without re-encoding.
        /// </summary>
        public AudioEncoder AudioEncoderFallback { get; set; }
        /// <summary>
        /// Set audio track bitrate(s) in kbit/s. (default: determined by the selected codec, mixdown, and samplerate combination). Separate tracks by commas.
        /// </summary>
        public IList<int> AudioBitrates { get; set; }
        /// <summary>
        /// Set audio quality metric. Separate tracks by commas.
        /// </summary>
        public IList<float> AudioQualities { get; set; }
        /// <summary>
        /// Set audio compression metric. (available depending on selected codec) Separate tracks by commas.
        /// </summary>
        public IList<float> AudioCompressions { get; set; }
        /// <summary>
        /// Format(s) for audio downmixing/upmixing
        /// </summary>
        public IList<Mixdown> Mixdowns { get; set; }
        /// <summary>
        ///  Normalize audio mix levels to prevent clipping. Separate tracks by commas. 0 = Disable Normalization (default) 1 = Enable Normalization
        /// </summary>
        public IList<string> NormalizeMixes { get; set; }
        /// <summary>
        /// Set audio samplerate(s) (8/11.025/12/16/22.05/24/32/44.1/48 kHz) or "auto". Separate tracks by commas.
        /// </summary>
        public IList<AudioSampleRate> AudioSampleRates { get; set; }
        /// <summary>
        /// Apply extra dynamic range compression to the audio, making soft sounds louder.Range is 1.0 to 4.0 (too loud), with 1.5 - 2.5 being a useful range. Separate tracks by commas.
        /// </summary>
        public IList<float> DynamicRangeCompressions { get; set; }
        /// <summary>
        /// Amplify or attenuate audio before encoding.  Does NOT work with audio passthru(copy). Values are in dB.Negative values attenuate, positive values amplify.A 1 dB difference is barely audible.
        /// </summary>
        public float? AudioGain { get; set; }
        /// <summary>
        /// Select dithering to apply before encoding audio
        /// </summary>
        public IList<AudioDither> AudioDithers { get; set; }
        /// <summary>
        /// Set audio track name(s). Separate tracks by commas.
        /// </summary>
        public IList<string> AudioTrackNames { get; set; }
        #endregion

        #region picture options
        /// <summary>
        /// Set storage width in pixels
        /// </summary>
        public int? Width { get; set; }
        /// <summary>
        /// Set storage height in pixels
        /// </summary>
        public int? Height { get; set; }
        /// <summary>
        /// <top:bottom:left:right> Set picture cropping in pixels (default: automatically remove black bars)
        /// </summary>
        public string Crop { get; set; }
        /// <summary>
        /// Always crop to a multiple of the modulus
        /// </summary>
        public bool? LooseCrop { get; set; }
        /// <summary>
        /// Disable preset 'loose-crop'
        /// </summary>
        public bool? NoLooseCrop { get; set; }
        /// <summary>
        /// Set maximum height in pixels
        /// </summary>
        public int? MaxHeight { get; set; }
        /// <summary>
        /// Set maximum width in pixels
        /// </summary>
        public int? MaxWidth { get; set; }
        /// <summary>
        /// Anamorphic setting for the output video (combines parameters --non-anamorphic -auto-anamorphic --loose-anamorphic
        /// Non Anamorphic: Set pixel aspect ratio to 1:1
        /// Auto Anamorphic: Store pixel aspect ratio that maximizes storage resolution
        /// Loose Anamorphic: Store pixel aspect ratio that is as close as possible to the source video pixel aspect ratio
        /// </summary>
        public Anamorphic Anamorphic { get; set; }
        /// <summary>
        /// Set display width in pixels, for custom anamorphic.This determines the display aspect during playback, which may differ from the storage aspect.
        /// </summary>
        public int? DisplayWidth { get; set; }
        /// <summary>
        /// Preserve the source's display aspect ratio when using custom anamorphic
        /// </summary>
        public bool? KeepDisplayAspect { get; set; }
        /// <summary>
        /// Disable preset 'keep-display-aspect'
        /// </summary>
        public bool? NoKeepDisplayAspect { get; set; }
        /// <summary>
        /// <par_x:par_y> Set pixel aspect for custom anamorphic (--display-width and --pixel-aspect are mutually exclusive.
        /// </summary>
        public string PixelAspect { get; set; }
        /// <summary>
        /// Use wider ITU pixel aspect values for loose and custom anamorphic, useful with underscanned sources
        /// </summary>
        public bool? ItuPar { get; set; }
        /// <summary>
        /// Disable preset 'itu-par'
        /// </summary>
        public bool? NoItuPar { get; set; }
        /// <summary>
        /// Set storage width and height modulus Dimensions will be made divisible by this number. (default: set by preset, typically 2)
        /// </summary>
        public int? Modulus { get; set; }
        /// <summary>
        /// Set the color space signaled by the output
        /// </summary>
        public ColorMatrix ColorMatrix { get; set; }
        #endregion

        #region filters options
        /// <summary>
        /// Detect interlace artifacts in frames. 
        /// If not accompanied by the decomb or deinterlace filters, this filter only logs the interlaced frame count to the activity log.
        /// If accompanied by the decomb or deinterlace filters, it causes these filters to selectively deinterlace only those frames where interlacing is detected.
        /// </summary>
        public string CombDetect { get; set; }
        /// <summary>
        /// Disable preset comb-detect filter
        /// </summary>
        public bool? NoCombDetect { get; set; }
        /// <summary>
        /// Deinterlace video using FFmpeg yadif.
        /// </summary>
        public string Deinterlace { get; set; }
        /// <summary>
        /// Disable preset deinterlace filter
        /// </summary>
        public bool? NoDeinterlace { get; set; }
        /// <summary>
        /// Deinterlace video using a combination of yadif, blend, cubic, or EEDI2 interpolation.
        /// </summary>
        public string Decomb { get; set; }
        /// <summary>
        /// Disable preset decomb filter
        /// </summary>
        public bool NoDecomb { get; set; }
        /// <summary>
        /// Detelecine(ivtc) video with pullup filter
        /// Note: this filter drops duplicate frames to restore the pre-telecine framerate, unless you specify a constant framerate(--rate 29.97 --cfr)
        /// </summary>
        public string Detelecine { get; set; }
        /// <summary>
        /// Disable preset detelecine filter
        /// </summary>
        public bool? NoDetelecine { get; set; }
        /// <summary>
        /// Denoise video with hqdn3d filter
        /// </summary>
        public string Hqdn3d { get; set; }
        /// <summary>
        /// Disable preset hqdn3d filter
        /// </summary>
        public bool? NoHqdn3d { get; set; }
        /// <summary>
        /// Denoise video with NLMeans filter
        /// </summary>
        public string Nlmeans { get; set; }
        /// <summary>
        /// Disable preset NLMeans filter
        /// </summary>
        public bool? NoNlmeans { get; set; }
        /// <summary>
        /// Tune NLMeans filter to content type
        /// </summary>
        public NlmeansTune NlmeansTune { get; set; }
        /// <summary>
        /// Sharpen video with chroma smooth filter
        /// </summary>
        public string ChromaSmooth { get; set; }
        /// <summary>
        /// Disable preset chroma smooth filter
        /// </summary>
        public bool? NoChromaSmooth { get; set; }
        /// <summary>
        /// Tune chroma smooth filter
        /// </summary>
        public ChromaSmoothTune ChromaSmoothTune { get; set; }
        /// <summary>
        /// Sharpen video with unsharp filter
        /// </summary>
        public string Unsharp { get; set; }
        /// <summary>
        /// Disable preset unsharp filter
        /// </summary>
        public bool? NoUnsharp { get; set; }
        /// <summary>
        /// Tune unsharp filter
        /// </summary>
        public UnsharpTune UnsharpTune { get; set; }
        /// <summary>
        /// Sharpen video with lapsharp filter
        /// </summary>
        public string Lapsharp { get; set; }
        /// <summary>
        /// Disable preset lapsharp filter
        /// </summary>
        public bool? NoLapsharp { get; set; }
        /// <summary>
        /// Tune lapsharp filter
        /// </summary>
        public LapsharpTune LapsharpTune { get; set; }
        /// <summary>
        /// Deblock video with avfilter deblock
        /// </summary>
        public string Deblock { get; set; }
        /// <summary>
        /// Disable preset deblock filter
        /// </summary>
        public bool? NoDeblock { get; set; }
        /// <summary>
        /// Tune deblock filter
        /// </summary>
        public DeblockTune DeblockTune { get; set; }
        /// <summary>
        /// Rotate image or flip its axes.angle rotates clockwise, can be one of: 0, 90, 180, 270 hflip= 1 flips the image on the x axis (horizontally).
        /// </summary>
        public string Rotate { get; set; }
        /// <summary>
        /// Pad image with borders(e.g.letterbox).
        /// </summary>
        public string Pad { get; set; }
        /// <summary>
        /// Convert colorspace, transfer characteristics or color primaries.
        /// </summary>
        public string Colorspace { get; set; }
        /// <summary>
        /// Grayscale encoding
        /// </summary>
        public bool? Grayscale { get; set; }
        /// <summary>
        /// Disable preset 'grayscale'
        /// </summary>
        public bool? NoGrayscale { get; set; }
        #endregion

        #region subtitles options
        /// <summary>
        /// Specify a comma separated list of subtitle languages you would like to select from the source title.
        /// By default, the first subtitle matching each language will be added to your output.Provide the language's ISO 639-2 code
        /// </summary>
        public IList<string> SubtitleLanguages { get; set; }
        /// <summary>
        /// Select all subtitle tracks matching languages in the specified language list
        /// </summary>
        public bool? AllSubtitles { get; set; }
        /// <summary>
        /// Select first subtitle track matching languages in the specified language list
        /// </summary>
        public bool? FirstSubtitle { get; set; }
        /// <summary>
        /// Select subtitle track(s), separated by commas More than one output track can be used for one input. "none" for no subtitles. Example: "1,2,3" for multiple tracks. A special track name "scan" adds an extra first pass.This extra pass scans subtitles matching the language of the first audio or the language selected by --native-language.The one that's only used 10 percent of the time or less is selected. This should locate subtitles for short foreign language segments.
        /// </summary>
        public IList<string> Subtitles { get; set; }
        /// <summary>
        /// Set subtitle track name(s).
        /// </summary>
        public IList<string> SubtitleNames { get; set; }
        /// <summary>
        /// Only display subtitles from the selected stream if the subtitle has the forced flag set.The values in 'string' are indexes into the subtitle list specified with '--subtitle'. Separate tracks by commas.Example: "1,2,3" for multiple tracks.
        /// </summary>
        public IList<string> SubtitlesForced { get; set; }
        /// <summary>
        /// "Burn" the selected subtitle into the video track.If "subtitle" is omitted, the first track is burned. "subtitle" is an index into the subtitle list specified with '--subtitle' or "native" to burn the subtitle track that may be added by the 'native-language' option.
        /// </summary>
        public string SubtitleBurned { get; set; }
        /// <summary>
        /// Flag the selected subtitle as the default subtitle to be displayed upon playback.Setting no default means no subtitle will be displayed automatically. 'number' is an index into the subtitle list specified with '--subtitle'.
        /// </summary>
        public string SubtitleDefault { get; set; }
        /// <summary>
        /// Specify your language preference.When the first audio track does not match your native language then select the first subtitle that does.When used in conjunction with --native-dub the audio track is changed in preference to subtitles.
        /// </summary>
        public string NativeLanguage { get; set; }
        /// <summary>
        /// Used in conjunction with --native-language requests that if no audio tracks are selected the default selected audio track will be the first one that matches the --native-language.If there are no matching audio tracks then the first matching subtitle track is used instead.
        /// </summary>
        public bool? NativeDub { get; set; }
        /// <summary>
        /// SubRip SRT filename(s), separated by commas.
        /// </summary>
        public IList<string> SrtFiles { get; set; }
        /// <summary>
        /// Character codeset(s) that the SRT file(s) are encoded as, separated by commas.
        /// </summary>
        public IList<string> SrtCodesets { get; set; }
        /// <summary>
        /// Offset(in milliseconds) to apply to the SRT file(s), separated by commas.If not specified, zero is assumed.Offsets may be negative.
        /// </summary>
        public IList<string> SrtOffsets { get; set; }
        /// <summary>
        /// SRT track language as an ISO 639-2 code
        /// </summary>
        public IList<string> SrtLanguages { get; set; }
        /// <summary>
        /// Flag the selected SRT as the default subtitle to be displayed during playback.Setting no default means no subtitle will be automatically displayed.If 'number' is omitted, the first SRT is the default. 'number' is a 1-based index into the 'srt-file' list
        /// </summary>
        public int? SrtDefault { get; set; }
        /// <summary>
        /// "Burn" the selected SRT subtitle into the video track.If 'number' is omitted, the first SRT is burned. 'number' is a 1-based index into the 'srt-file' list
        /// </summary>
        public int? SrtBurn { get; set; }
        /// <summary>
        /// SubStationAlpha SSA filename(s), separated by commas.
        /// </summary>
        public IList<string> SsaFiles { get; set; }
        /// <summary>
        /// Offset(in milliseconds) to apply to the SSA file(s), separated by commas.If not specified, zero is assumed.Offsets may be negative.
        /// </summary>
        public IList<string> SsaOffsets { get; set; }
        /// <summary>
        /// SSA track language as an ISO 639-2 code(e.g.fre, eng, spa, dut, et cetera) If not specified, then 'und' is used.Separate by commas.
        /// </summary>
        public IList<string> SsaLanguages { get; set; }
        /// <summary>
        /// Flag the selected SSA as the default subtitle to be displayed during playback.Setting no default means no subtitle will be automatically displayed.If 'number' is omitted, the first SSA is the default. 'number' is a 1-based index into the 'ssa-file' list
        /// </summary>
        public int? SsaDefault { get; set; }
        /// <summary>
        /// "Burn" the selected SSA subtitle into the video track.If 'number' is omitted, the first SSA is burned. 'number' is a 1-based index into the 'ssa-file' list
        /// </summary>
        public int? SsaBurn { get; set; }
        #endregion

        public HandbrakeConfiguration(bool omitDefaults = false) {
            // initialize all lists
            Chapters = new List<string>();
            EncoderPresets = new List<string>();
            EncoderTunes = new List<string>();
            Encopts = new List<Encopt>();
            EncoderProfiles = new List<string>();
            EncoderLevels = new List<EncoderLevel>();
            AudioLanguages = new List<string>();
            Audios = new List<string>();
            AudioEncoders = new List<AudioEncoder>();
            AudioCopyMasks = new List<AudioCopyMask>();
            AudioBitrates = new List<int>();
            AudioQualities = new List<float>();
            AudioCompressions = new List<float>();
            Mixdowns = new List<Mixdown>();
            NormalizeMixes = new List<string>();
            AudioSampleRates = new List<AudioSampleRate>();
            DynamicRangeCompressions = new List<float>();
            AudioDithers = new List<AudioDither>();
            AudioTrackNames = new List<string>();
            SubtitleLanguages = new List<string>();
            Subtitles = new List<string>();
            SubtitleNames = new List<string>();
            SubtitlesForced = new List<string>();
            SrtFiles = new List<string>();
            SrtCodesets = new List<string>();
            SrtOffsets = new List<string>();
            SrtLanguages = new List<string>();
            SsaFiles = new List<string>();
            SsaOffsets = new List<string>();
            SsaLanguages = new List<string>();

            // initialize default options
            if (!omitDefaults) {
                Format = Format.av_mp4;
                Encoder = VideoEncoder.x264;
                EncoderLevel = EncoderLevel._4_0;
                VideoQuality = 21;
                VideoBitrate = 1000;
                FrameRate = 30;
                FrameRateSetting = FrameRateSetting.pfr;
                AudioTracks = AudioTracks.first_audio;
                AudioEncoders.Add(AudioEncoder.copy__aac);
                AudioCopyMasks.Add(AudioCopyMask.aac);
                AudioEncoderFallback = AudioEncoder.copy__aac;
                AudioBitrates.Add(320);
                Mixdowns.Add(Mixdown._5point1);
                AudioSampleRates.Add(AudioSampleRate.Auto);
                AudioGain = 0;
                MaxHeight = -1;
                MaxWidth = -1;
                Anamorphic = Anamorphic.loose_anamorphic;
                Modulus = 2;
            }
        }

        /// <summary>
        /// Builds an argument string from the settings
        /// </summary>
        /// <returns>A string to be used as argument to Handbrake CLI</returns>
        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            // source options
            AddParameter(builder, "title", Title);
            AddParameter(builder, "min-duration", MinDurationSeconds);
            AddParameterFlag(builder, "scan", ScanSelectedTitleOnly);
            AddParameterFlag(builder, "main-feature", MainFeature);
            AddParameter(builder, "chapters", Chapters);
            AddParameter(builder, "angle", Angle);
            AddParameter(builder, "previews", Previews?.ToString());
            AddParameter(builder, "start-at-preview", StartAtPreview);
            AddParameter(builder, "start-at", StartAt?.ToString());
            AddParameter(builder, "stop-at", StopAt?.ToString());

            // destination options
            AddParameter(builder, "format", Format);
            AddParameterFlag(builder, "markers", Markers);
            AddParameterFlag(builder, "no-markers", NoMarkers);
            AddParameterFlag(builder, "optimize", WebOptimize);
            AddParameterFlag(builder, "no-optimize", NoWebOptimize);
            AddParameterFlag(builder, "ipod-atom", IpodAtom);
            AddParameterFlag(builder, "no-ipod-atom", NoIpodAtom);
            AddParameterFlag(builder, "align-av", AlignAv);
            AddParameterFlag(builder, "inline-parameter-sets", InlineParameterSets);

            // video options
            AddParameter(builder, "encoder", Encoder);
            AddParameter(builder, "encoder-preset", EncoderPreset);
            AddParameter(builder, "encoder-preset-list", EncoderPresets);
            AddParameter(builder, "encoder-tune", EncoderTune);
            AddParameter(builder, "encoder-tune-list", EncoderTunes);
            AddParameter(builder, "encopts", Encopts, ":");
            AddParameter(builder, "encoder-profile", EncoderProfile);
            AddParameter(builder, "encoder-profile-list", EncoderProfiles);
            AddParameter(builder, "encoder-level", EncoderLevel);
            AddParameter(builder, "encoder-level-list", EncoderLevels);
            AddParameter(builder, "quality", VideoQuality);
            AddParameter(builder, "vb", VideoBitrate);
            AddParameterFlag(builder, "two-pass", TwoPass);
            AddParameterFlag(builder, "no-two-pass", NoTwoPass);
            AddParameterFlag(builder, "turbo", Turbo);
            AddParameterFlag(builder, "no-turbo", NoTurbo);
            AddParameter(builder, "rate", FrameRate);
            AddParameterFlag(builder, FrameRateSetting.ToString(), true);

            // audio options
            AddParameter(builder, "audio-lang-list", AudioLanguages);
            AddParameterFlag(builder, AudioTracks.ToString(), true);
            AddParameter(builder, "audio", Audios);
            AddParameter(builder, "aencoder", AudioEncoders);
            AddParameter(builder, "audio-copy-mask", AudioCopyMasks);
            AddParameter(builder, "audio-fallback", AudioEncoderFallback);
            AddParameter(builder, "ab", AudioBitrates);
            AddParameter(builder, "aq", AudioQualities);
            AddParameter(builder, "ac", AudioCompressions);
            AddParameter(builder, "mixdown", Mixdowns);
            AddParameter(builder, "normalize-mix", NormalizeMixes);
            AddParameter(builder, "arate", AudioSampleRates);
            AddParameter(builder, "drc", DynamicRangeCompressions);
            AddParameter(builder, "gain", AudioGain);
            AddParameter(builder, "adither", AudioDithers);
            AddParameter(builder, "aname", AudioTrackNames);

            // picture options
            AddParameter(builder, "width", Width);
            AddParameter(builder, "height", Height);
            AddParameter(builder, "crop", Crop);
            AddParameterFlag(builder, "loose-crop", LooseCrop);
            AddParameterFlag(builder, "no-loose-crop", NoLooseCrop);
            AddParameter(builder, "maxHeight", MaxHeight);
            AddParameter(builder, "maxWidth", MaxWidth);
            AddParameterFlag(builder, Anamorphic.ToString(), true);
            AddParameter(builder, "display-width", DisplayWidth);
            AddParameterFlag(builder, "keep-display-aspect", KeepDisplayAspect);
            AddParameterFlag(builder, "no-keep-display-aspect", NoKeepDisplayAspect);
            AddParameter(builder, "pixel-aspect", PixelAspect);
            AddParameterFlag(builder, "itu-par", ItuPar);
            AddParameterFlag(builder, "no-itu-par", NoItuPar);
            AddParameter(builder, "modulus", Modulus);
            AddParameter(builder, "color-matrix", ColorMatrix);

            // filters options
            AddParameter(builder, "comb-detect", CombDetect, "=");
            AddParameterFlag(builder, "no-comb-detect", NoCombDetect);
            AddParameter(builder, "deinterlace", Deinterlace, "=");
            AddParameterFlag(builder, "no-deinterlace", NoDeinterlace);
            AddParameter(builder, "decomb", Decomb, "=");
            AddParameterFlag(builder, "no-decomb", NoDecomb);
            AddParameter(builder, "detelecine", Detelecine, "=");
            AddParameterFlag(builder, "no-detelecine", NoDetelecine);
            AddParameter(builder, "hqdn3d", Hqdn3d, "=");
            AddParameterFlag(builder, "no-hqdn3d", NoHqdn3d);
            AddParameter(builder, "nlmeans", Nlmeans, "=");
            AddParameterFlag(builder, "no-nlmeans", NoNlmeans);
            AddParameter(builder, "nlmeans-tune", NlmeansTune);
            AddParameter(builder, "chroma-smooth", ChromaSmooth, "=");
            AddParameterFlag(builder, "no-chroma-smooth", NoChromaSmooth);
            AddParameter(builder, "chroma-smooth-tune", ChromaSmoothTune);
            AddParameter(builder, "unsharp", Unsharp, "=");
            AddParameterFlag(builder, "no-unsharp", NoUnsharp);
            AddParameter(builder, "unsharp-tune", UnsharpTune);
            AddParameter(builder, "lapsharp", Lapsharp, "=");
            AddParameterFlag(builder, "no-lapsharp", NoLapsharp);
            AddParameter(builder, "lapsharp-tune", LapsharpTune);
            AddParameter(builder, "deblock", Deblock, "=");
            AddParameterFlag(builder, "no-deblock", NoDeblock);
            AddParameter(builder, "deblock-tune", DeblockTune);
            AddParameter(builder, "rotate", Rotate, "=");
            AddParameter(builder, "pad", Pad);
            AddParameter(builder, "colorspace", Colorspace);
            AddParameterFlag(builder, "grayscale", Grayscale);
            AddParameterFlag(builder, "no-grayscale", NoGrayscale);

            // subtitles options
            AddParameter(builder, "subtitle-lang-list", SubtitleLanguages);
            AddParameterFlag(builder, "all-subtitles", AllSubtitles);
            AddParameterFlag(builder, "first-subtitle", FirstSubtitle);
            AddParameter(builder, "subtitle", Subtitles);
            AddParameter(builder, "subname", SubtitleNames);
            AddParameter(builder, "subtitle-forced", SubtitlesForced, "=");
            AddParameter(builder, "subtitle-burned", SubtitleBurned, "=");
            AddParameter(builder, "subtitle-default", SubtitleDefault, "=");
            AddParameter(builder, "native-language", NativeLanguage);
            AddParameterFlag(builder, "native-dub", NativeDub);
            AddParameter(builder, "srt-file", SrtFiles);
            AddParameter(builder, "srt-codeset", SrtCodesets);
            AddParameter(builder, "srt-offset", SrtOffsets);
            AddParameter(builder, "srt-lang", SrtLanguages);
            AddParameter(builder, "srt-default", SrtDefault, "=");
            AddParameter(builder, "srt-burn", SrtBurn, "=");
            AddParameter(builder, "ssa-file", SsaFiles);
            AddParameter(builder, "ssa-offset", SsaOffsets);
            AddParameter(builder, "ssa-lang", SsaLanguages);
            AddParameter(builder, "ssa-default", SsaDefault, "=");
            AddParameter(builder, "ssa-burn", SsaBurn, "=");

            return builder.ToString().TrimEnd(' ');
        }

        private static void AddParameter(StringBuilder builder, string parameter, float? value, string optionSpacing = " ") {
            if (value != null)
                AddParameter(builder, parameter, value.ToString(), optionSpacing);
        }
        private static void AddParameter(StringBuilder builder, string parameter, int? value, string optionSpacing = " ") {
            if (value != null)
                AddParameter(builder, parameter, value.ToString(), optionSpacing);
        }
        private static void AddParameter<T>(StringBuilder builder, string parameter, IEnumerable<T> values, string seperator = ",", string optionSpacing = " ") {
            if (values != null && values.Count() > 0)
                AddParameter(builder, parameter, values.Select(x => x.ToString()), seperator, optionSpacing);
        }
        private static void AddParameter(StringBuilder builder, string parameter, IEnumerable<string> values, string seperator = ",", string optionSpacing = " ") {
            if (values != null && values.Count() > 0)
                AddParameter(builder, parameter, String.Join(seperator, values), optionSpacing);
        }
        private static void AddParameter(StringBuilder builder, string parameter, Enum value, string optionSpacing = " ") {
            if (value == null)
                return;

            AddParameter(builder, parameter, EnumHelper.Format(value), optionSpacing);
        }
        private static void AddParameter(StringBuilder builder, string parameter, string value, string optionSpacing = " ") {
            if (!String.IsNullOrWhiteSpace(value))
                builder.Append($"--{parameter}{optionSpacing}{value} ");
        }
        private static void AddParameterFlag(StringBuilder builder, string parameter, bool? value) {
            if (value == true)
                builder.Append($"--{parameter} ");
        }
    }
}