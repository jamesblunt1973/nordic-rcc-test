using System.Collections.Concurrent;

namespace WordCounter;

public interface IWordCounter
{
	IDictionary<string, int> CountWords(IEnumerable<string> lines);
}

public class WordCounter(char[] separators) : IWordCounter
{
	public IDictionary<string, int> CountWords(IEnumerable<string> lines)
	{
		var wordCounts = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
		foreach (var line in lines)
		{
			var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			foreach (var word in words)
			{
				wordCounts.AddOrUpdate(word, 1, (_, count) => count + 1);
			}
		}
		return wordCounts;
	}
}
