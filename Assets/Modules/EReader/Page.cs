namespace EReader {
    
    public abstract class Page<T> {
        
        public abstract string pageName { get; }
        public abstract double pageNb { get; }
        
        public abstract double topMargin { get; }
        public abstract double bottomMargin { get; }
        public abstract double rightMargin { get; }
        public abstract double leftMargin { get; }
        
        public abstract T getContent();
    }
}