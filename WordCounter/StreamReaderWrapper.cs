namespace WordCounter;

public interface IStreamReader: IDisposable
{
	Task<string?> ReadLineAsync();
}

public class StreamReaderWrapper(string filePath) : IStreamReader
{
	private readonly StreamReader _streamReader = new(filePath);

	public async Task<string?> ReadLineAsync()
	{
		return await _streamReader.ReadLineAsync();
	}

	public void Dispose()
	{
		_streamReader.Dispose();
	}
}
