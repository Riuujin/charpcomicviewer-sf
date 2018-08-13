using System.Threading.Tasks;

namespace CSharpComicViewerLib.Comic
{
    public interface IComic
    {
        string getBookmarkName(int pageNumber);

        string[] GetFilePaths();

        Task<int> GetNumberOfPages();

        Task<byte[]> GetPage(int pageNumber);

        Task<string> GetInformationText();
    }
}