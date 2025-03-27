using ChristianApi.Data.Interfaces;
using System.Text;

namespace ChristianApi.Data
{
	public class FileManager : IFileManager
	{
		public StreamReader StreamReader(string path, Encoding encoding)
		{
			return new StreamReader(path, encoding);
		}
	}
}
