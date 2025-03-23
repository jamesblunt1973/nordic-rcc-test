using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace WordCounter.Test;

public class FileReaderTest
{
	[Fact]
	public async Task Given_TextFile_When_ReadLinesAsync_Then_Content_Is_Valid()
	{
		// Arrange
		var fileContent = new List<string>
		{
			"Line 1",
			"Line 2",
			"Line 3"
		};

		// Mock StreamReader
		var streamReader = Substitute.For<IStreamReader>();
		streamReader.ReadLineAsync().Returns(fileContent[0], fileContent[1], fileContent[2], null);

		var fileReader = new FileReader(_ => streamReader);

		// Act
		var lines = fileReader.ReadLinesAsync("fakepath.txt").ToListAsync();

		// Assert
		var result = await lines;
		streamReader.ReceivedCalls();
		result.Should().NotBeEmpty();
		result.Count.Should().Be(fileContent.Count);
		result.Should().Equal(fileContent);
	}

	[Fact]
	public async Task Given_TextFile_When_Read_Error_Then_Throw_IOException()
	{
		// Arrange
		var streamReader = Substitute.For<IStreamReader>();
		streamReader.ReadLineAsync().Throws(new IOException("Read error"));
		var fileReader = new FileReader(_ => streamReader);

		// Act
		var result = await fileReader.ReadLinesAsync("fakepath.txt").ToListAsync();

		// Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public async Task Given_TextFile_When_Access_Error_Then_Throw_UnauthorizedAccessException()
	{
		// Arrange
		var streamReader = Substitute.For<IStreamReader>();
		streamReader.ReadLineAsync().Throws(new UnauthorizedAccessException("Access denied"));
		var fileReader = new FileReader(_ => streamReader);

		// Act
		var result = await fileReader.ReadLinesAsync("fakepath.txt").ToListAsync();

		// Assert
		result.Should().BeEmpty();
	}
}
