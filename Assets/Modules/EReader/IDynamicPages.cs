namespace EReader {
    
    public interface IDynamicPages {

        void addPageAt(Page page, int index);
        void removePage(int index);
    }
}