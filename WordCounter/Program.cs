using System.Collections.Concurrent;
using WordCounter;

IFileReader fileReader = new FileReader(filePath => new StreamReaderWrapper(filePath)); 
IWordCounter wordCounter = new WordCounter.WordCounter([' ', ',', '.', ':', ';']);

var files = Directory.GetFiles("Data");
var output = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

await Parallel.ForEachAsync(files, async (file, _) =>
{
	var lines = fileReader.ReadLinesAsync(file);
	var wordCounts = wordCounter.CountWords(await lines.ToListAsync());

	foreach (var kvp in wordCounts)
	{
		output.AddOrUpdate(kvp.Key, kvp.Value, (_, count) => count + kvp.Value);
	}
});

foreach (var item in output.AsParallel().OrderBy(a => a.Value))
{
	Console.WriteLine($"{item.Key}: {item.Value}");
}
