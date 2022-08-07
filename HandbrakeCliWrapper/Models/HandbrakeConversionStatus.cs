using System;

namespace HandbrakeCliWrapper.Models {
    public class HandbrakeConversionStatus {
        /// <summary>
        /// Whether a conversion is going on at the moment
        /// </summary>
        public bool Converting { get; internal set; }
        /// <summary>
        /// The file used as input file for the current conversion
        /// </summary>
        public string InputFile { get; internal set; }
        /// <summary>
        /// The filename used as output filename for the current conversion
        /// </summary>
        public string OutputFile { get; internal set; }
        /// <summary>
        /// How many percentage done the current conversion is
        /// </summary>
        public float Percentage { get; internal set; }
        /// <summary>
        /// The current fps for the current conversion
        /// </summary>
        public float CurrentFps { get; internal set; }
        /// <summary>
        /// The average fps for the current conversion
        /// </summary>
        public float AverageFps { get; internal set; }
        /// <summary>
        /// The estimated time left of the current conversion
        /// </summary>
        public TimeSpan Estimated { get; internal set; }

        public override string ToString() {
            if (!Converting)
                return "Idle";
            return
                $"{InputFile} -> {OutputFile} - {Percentage}%  {CurrentFps} fps.  {AverageFps} fps. avg.  {Estimated} time remaining";
        }
    }
}