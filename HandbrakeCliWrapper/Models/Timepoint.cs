using HandbrakeCliWrapper.Enums;

namespace HandbrakeCliWrapper.Models {
    public class Timepoint {
        public TimepointType TimepointType { get; set; }
        public int Point { get; set; }
    }
}