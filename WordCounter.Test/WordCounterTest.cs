namespace WordCounter.Test
{
	public class WordCounterTest
	{
		private readonly char[] separators = [' ', ',', '.', ':', ';'];

		[Fact]
		public void Given_WordCounter_When_CountWords_Then_Count_Occurrences()
		{
			// Arrange
			var wordCounter = new WordCounter(separators);
			var lines = new List<string>
			{
				"apple, banana apple",
				"apple orange, banana"
			};

			// Act
			var result = wordCounter.CountWords(lines);

			// Assert
			Assert.Equal(3, result["apple"]);
			Assert.Equal(2, result["banana"]);
			Assert.Equal(1, result["orange"]);
		}

		[Fact]
		public void Given_WordCounter_When_CountWords_Then_Should_Ignore_Case()
		{
			// Arrange
			var wordCounter = new WordCounter(separators);
			var lines = new List<string>
			{
				"Apple apple",
				"Banana Banana"
			};

			// Act
			var result = wordCounter.CountWords(lines);

			// Assert
			Assert.Equal(2, result["apple"]);
			Assert.Equal(2, result["banana"]);
		}

		[Fact]
		public void Given_WordCounter_When_CountWords_Then_Should_Trim_White_Space()
		{
			// Arrange
			var wordCounter = new WordCounter(separators);
			var lines = new List<string>
			{
				"   apple   , banana  ,   apple    ....",
				"   apple    ,   orange;banana.        ;:"
			};

			// Act
			var result = wordCounter.CountWords(lines);

			// Assert
			Assert.Equal(3, result["apple"]);
			Assert.Equal(2, result["banana"]);
			Assert.Equal(1, result["orange"]);
		}
	}
}
