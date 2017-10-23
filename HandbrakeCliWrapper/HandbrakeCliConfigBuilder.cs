using System.Collections.Generic;
using System.Text;

namespace HandbrakeCLIwrapper
{
    
    /// <summary>
    /// Configuration builder for the Handbrake CLI
    /// </summary>
    public class HandbrakeCliConfigBuilder
    {
        /// <summary>
        /// Output container format
        /// </summary>
        public Format Format { get; set; } = Format.av_mp4;
        /// <summary>
        /// Max height of the output video
        /// </summary>
        public int MaxHeight { get; set; } = -1;
        /// <summary>
        /// Max width of the output video
        /// </summary>
        public int MaxWidth { get; set; } = -1;
        /// <summary>
        /// Whether to web optimize the output video
        /// </summary>
        public bool WebOptimize { get; set; } = true;
        /// <summary>
        /// The modulus of the output video 
        /// </summary>
        public int Modulus { get; set; } = 2;
        /// <summary>
        /// Anamorphic setting for the output video
        /// </summary>
        public Anamorphic Anamorphic { get; set; } = Anamorphic.loose_anamorphic;
        /// <summary>
        /// Video encoder to use 
        /// </summary>
        public Encoder Encoder { get; set; } = Encoder.x264;
        /// <summary>
        /// Relative video quality for the output video
        /// </summary>
        public float VideoQuality { get; set; } = 21;
        /// <summary>
        /// Frame-rate setting for the output video
        /// </summary>
        public FrameRateSetting FrameRateSetting { get; set; } = FrameRateSetting.pfr;
        /// <summary>
        /// Frame-rate for the output video
        /// </summary>
        public float FrameRate { get; set; } = 30;
        /// <summary>
        /// Audio encoder to use for the output video
        /// </summary>
        public AudioEncoder AudioEncoder { get; set; } = AudioEncoder.copy__aac;
        /// <summary>
        /// The copy mask to apply when using the 'copy' setting for the audio encoder.
        /// The list of audio codecs to copy rather than transcode
        /// </summary>
        public List<AudioCopyMask> AudioCopyMask { get; set; } =
            new List<AudioCopyMask> {HandbrakeCLIwrapper.AudioCopyMask.aac};
        /// <summary>
        /// The fallback audio encoder.
        /// The encoder to use for audio tracks that needs to be transcoded when using the 'copy' settings for the audio encoder.
        /// </summary>
        public AudioEncoder AudioEncoderFallback { get; set; } = AudioEncoder.av_aac;
        /// <summary>
        /// The maximum bit rate of the audio of the output video
        /// </summary>
        public int AudioBitrate { get; set; } = 320;
        /// <summary>
        /// Which audio tracks to process and keep in the output video
        /// </summary>
        public AudioTracks AudioTracks { get; set; } = AudioTracks.first_audio;
        /// <summary>
        /// The audio gain to be applied to the output video
        /// </summary>
        public float AudioGain { get; set; } = 0;
        /// <summary>
        /// Whether wwo-pass encoding is enabled 
        /// </summary>
        public bool TwoPass { get; set; } = true;
        /// <summary>
        /// Whether to enable turbo first pass when using two-pass
        /// </summary>
        public bool TurboTwoPass { get; set; } = true;
        /// <summary>
        /// The audio mixdown to apply to the output video
        /// </summary>
        public Mixdown Mixdown { get; set; } = Mixdown._5point1;
        /// <summary>
        /// The sample rate for the audio of the output video
        /// </summary>
        public AudioSampleRate AudioSampleRate { get; set; } = AudioSampleRate.Auto;
        /// <summary>
        /// The encoder level used for the output video
        /// </summary>
        public EncoderLevel EncoderLevel { get; set; } = EncoderLevel._4_0;

        /// <summary>
        /// Builds an argument string from the settings
        /// </summary>
        /// <returns>A string to be used as argument to Handbrake CLI</returns>
        public string Build()
        {
            var sb = new StringBuilder();
            sb.Append($"--format {Format} ");
            if (MaxHeight > 0)
                sb.Append($"--maxHeight {MaxHeight} ");
            if (MaxWidth > 0)
                sb.Append($"--maxWidth {MaxWidth} ");
            if (WebOptimize)
                sb.Append("--optimize ");

            sb.Append($"--modulus {Modulus} ");
            sb.Append($"--{Anamorphic.Formatted()} ");
            sb.Append($"--encoder {Encoder} ");
            sb.Append($"--quality {VideoQuality} ");
            sb.Append($"--{FrameRateSetting} ");
            sb.Append($"--rate {FrameRate} ");
            sb.Append($"--aencoder {AudioEncoder.Formatted()} ");
            if (AudioEncoder == AudioEncoder.copy)
                sb.Append($"--audio-copy-mask {AudioCopyMask.Formatted()} ");
            if (AudioEncoder.ToString().StartsWith("copy"))
                sb.Append($"--audio-fallback {AudioEncoderFallback.Formatted()} ");
            sb.Append($"--mixdown {Mixdown.Formatted()} ");
            sb.Append($"--arate {AudioSampleRate.Formatted()} ");
            sb.Append($"--ab {AudioBitrate} ");
            sb.Append($"--{AudioTracks.Formatted()} ");
            sb.Append($"--gain {AudioGain} ");
            sb.Append($"--subtitle none ");
            if (TwoPass)
            {
                sb.Append("--two-pass ");
                if (TurboTwoPass && (Encoder == Encoder.x264 || Encoder == Encoder.x265))
                    sb.Append("--turbo ");
            }
            sb.Append($"--encoder-level {EncoderLevel.Formatted()} ");
            sb.Append("--verbose 0 ");
            return sb.ToString();
        }
    }
}