namespace HandbrakeCliWrapper.Models {
    public class Preview {
        public int NumberOfPreviews { get; set; }
        public bool StoreToDisk { get; set; }

        public override string ToString() {
            return $"{NumberOfPreviews}:{StoreToDisk}";
        }
    }
}