using System.Text;

namespace ChristianApi.Data.Interfaces
{
	public interface IFileManager
	{
		StreamReader StreamReader(string path, Encoding encoding);
	}

	
}
