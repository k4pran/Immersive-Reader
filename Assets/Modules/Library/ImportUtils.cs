namespace Modules.Common {
    
    public class ImportUtils {

        private ImportUtils() {}

        public static FileType determineFileType(string filePath) {
            string fileExt = FileUtils.getFileExt(filePath).ToLower();
            
            switch (fileExt) {
                
                case ".txt":
                    return FileType.TXT;
                
                case ".pdf":
                    return FileType.PDF;
                
                default:
                    throw new UnsupportFileFormatException();
            }
        }
    }
}