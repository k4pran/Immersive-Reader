namespace Modules.EReader {
    public class ImageLocation : Content {
        
        private string imageUrl { get; }

        public ImageLocation(string imageUrl) {
            this.imageUrl = imageUrl;
        }
    }
}