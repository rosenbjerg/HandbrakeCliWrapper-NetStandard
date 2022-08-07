using System;

namespace HandbrakeCliWrapper.Models {
    public class HandbrakeTranscodingEventArgs : EventArgs {
        public string InputFilename { get; }

        public HandbrakeTranscodingEventArgs(string inputFilename) {
            InputFilename = inputFilename;
        }
    }
}