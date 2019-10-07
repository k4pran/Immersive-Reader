namespace Modules.VirtualBook {

    public interface IBookController {

        int CurrentPageNb();

        int Next();

        int Previous();

        int GoTo(int pageNb);

        int Start();

        int End();
    }
}