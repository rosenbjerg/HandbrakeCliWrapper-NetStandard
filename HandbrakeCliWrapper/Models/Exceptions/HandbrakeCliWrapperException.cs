using System;

namespace HandbrakeCliWrapper.Models.Exceptions {
    public class HandbrakeCliWrapperException : Exception {
        public HandbrakeCliWrapperException(string msg)
            : base(msg) {
        }
        public HandbrakeCliWrapperException(string msg, Exception inner)
            : base(msg, inner) {
        }
    }
}