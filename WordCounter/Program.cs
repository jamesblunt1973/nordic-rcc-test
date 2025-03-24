using System.Collections.Concurrent;
using WordCounter;

const int BatchSize = 50;

IFileReader fileReader = new FileReader(filePath => new StreamReaderWrapper(filePath)); 
IWordCounter wordCounter = new WordCounter.WordCounter([' ', ',', '.', ':', ';']);

var files = Directory.GetFiles("Data", "*.txt");
var output = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

//var time = DateTime.Now.Ticks;
await Parallel.ForEachAsync(files, async (file, _) =>
{
	var batch = new List<string>(BatchSize);
	await foreach (var line in fileReader.ReadLinesAsync(file))
	{
		batch.Add(line);

		if (batch.Count >= BatchSize)
		{
			ProcessBatchParallel(batch, wordCounter);
			batch.Clear();
		}
	}

	// Process remaining lines if any
	if (batch.Count > 0)
	{
		ProcessBatchParallel(batch, wordCounter);
	}
});

//var diff = DateTime.Now.Ticks - time;
//Console.WriteLine(diff);

foreach (var item in output.AsParallel().OrderBy(a => a.Value))
{
	Console.WriteLine($"{item.Key}: {item.Value}");
}

void ProcessBatchParallel(List<string> batch, IWordCounter wordCounter)
{
	//var wordCounts = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

	//Parallel.ForEach(batch, line =>
	//{
	//	var localWordCounts = wordCounter.CountWords([line]);
	//	foreach (var kvp in localWordCounts)
	//	{
	//		wordCounts.AddOrUpdate(kvp.Key, kvp.Value, (_, count) => count + kvp.Value);
	//	}
	//});

	var wordCounts = wordCounter.CountWords(batch);

	foreach (var kvp in wordCounts)
	{
		output.AddOrUpdate(kvp.Key, kvp.Value, (_, count) => count + kvp.Value);
	}
}