using System.Threading.Tasks;

namespace CSharpComicViewer.Comic
{
    public interface IComic
    {
        string getBookmarkName(int pageNumber);

        string[] GetFilePaths();

        int Pages();

        Task<byte[]> GetPage(int pageNumber);

        Task<string> GetInformationText();
    }
}