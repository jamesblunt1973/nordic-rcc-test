namespace WordCounter;

public interface IFileReader
{
	IAsyncEnumerable<string> ReadLinesAsync(string filePath);
}

public class FileReader(Func<string, IStreamReader> streamReaderFactory) : IFileReader
{
	public async IAsyncEnumerable<string> ReadLinesAsync(string filePath)
	{
		using IStreamReader streamReader = streamReaderFactory(filePath);
		List<string?> lines = [];
		try
		{
			await foreach (var line in streamReader.ReadLineAsync())
			{
				if (line == null)
					break;
				lines.Add(line);
			}
		}
		catch (IOException ex)
		{
			Console.WriteLine($"Error reading file '{filePath}': {ex.Message}");
		}
		catch (UnauthorizedAccessException ex)
		{
			Console.WriteLine($"Access denied to file '{filePath}': {ex.Message}");
		}
		finally
		{
			streamReader.Dispose();
		}
		foreach (var line in lines)
		{
			yield return line!;
		}
	}
}
