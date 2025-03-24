namespace WordCounter;

public interface IStreamReader: IDisposable
{
	IAsyncEnumerable<string?> ReadLineAsync();
}

public class StreamReaderWrapper(string filePath) : IStreamReader
{
	private readonly StreamReader _streamReader = new(filePath);

	public async IAsyncEnumerable<string?> ReadLineAsync()
	{
		while (true)
		{
			var line = await _streamReader.ReadLineAsync();
			if (line == null)
				yield break;

			yield return line;
		}
	}

	public void Dispose()
	{
		_streamReader.Dispose();
	}
}
