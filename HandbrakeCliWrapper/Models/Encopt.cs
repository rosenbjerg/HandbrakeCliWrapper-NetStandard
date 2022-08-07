namespace HandbrakeCliWrapper.Models {
    public class Encopt {
        public string OptionName { get; set; }
        public string Value { get; set; }

        public override string ToString() {
            return $"{OptionName}={Value}";
        }
    }
}