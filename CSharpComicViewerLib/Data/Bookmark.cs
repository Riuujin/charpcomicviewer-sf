using System.Linq;
using System.Text.Json.Serialization;

namespace CSharpComicViewerLib.Data
{
	public class Bookmark
	{
		private bool? allFilesExist;

		public string Name { get; set; }

		public string[] FilePaths { get; set; }

		public int Page { get; set; }

		[JsonIgnore]
		public bool AllFilesExist
		{
			get
			{
				if (allFilesExist == null && FilePaths != null)
				{
					allFilesExist = FilePaths.All(filePath => System.IO.File.Exists(filePath));
				}

				return allFilesExist.GetValueOrDefault(false);
			}
		}
	}
}