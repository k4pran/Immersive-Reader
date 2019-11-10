namespace Modules.VirtualBook {

    public interface ICoreInfo {

        string BookId();

        string BookTitle();

        void InitBookIdAs(string bookId);

        void InitTitleAs(string title);
    }
}