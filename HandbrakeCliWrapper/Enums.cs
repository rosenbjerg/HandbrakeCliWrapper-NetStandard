namespace HandbrakeCLIwrapper
{
    public enum EncoderLevel
    {
        _1_0,
        _1b,
        _1_1,
        _1_2,
        _1_3,
        _2_0,
        _2_1,
        _2_2,
        _3_0,
        _3_1,
        _3_2,
        _4_0,
        _4_1,
        _4_2,
        _5_0,
        _5_1,
        _5_2
    }

    public enum AudioSampleRate
    {
        Auto,
        _8,
        _11_025,
        _12,
        _16,
        _22_05,
        _24,
        _32,
        _44_1,
        _48
    }

    public enum Mixdown
    {
        mono,
        left_only,
        right_only,
        stereo,
        dpl1,
        dpl2,
        _5point1,
        _6point1,
        _7point1,
        _5__2__lfe
    }

    public enum AudioTracks
    {
        first_audio,
        all_audio
    }

    public enum AudioCopyMask
    {
        aac,
        ac3,
        eac3,
        truehd,
        dts,
        dtshd,
        mp3,
        flac
    }

    public enum AudioEncoder
    {
        none,
        av_aac,
        ca_aac,
        ca_haac,
        copy__aac,
        ac3,
        copy__ac3,
        eac3,
        copy__eac3,
        copy__truehd,
        copy__dts,
        copy__dtshd,
        mp3,
        copy__mp3,
        vorbis,
        flac16,
        flac24,
        copy__flac,
        opus,
        copy
    }

    public enum FrameRateSetting
    {
        /// <summary>
        ///     Variable frame rate
        /// </summary>
        vfr,

        /// <summary>
        ///     Constant frame rate
        /// </summary>
        cfr,

        /// <summary>
        ///     Max frame rate
        /// </summary>
        pfr
    }

    public enum Format
    {
        av_mp4,
        av_mkv
    }

    public enum Anamorphic
    {
        non_anamorphic,
        auto_anamorphic,
        loose_anamorphic
    }

    public enum Encoder
    {
        x264,
        x264_10bit,
        vce_h264,
        nvenc_h264,
        x265,
        x265_10bit,
        x265_12bit,
        nvenc_h265,
        mpeg4,
        mpeg2,
        VP8,
        VP9,
        theora
    }
}