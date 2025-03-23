# Nordic RCC Word Counter
## A show case for what it can be
### .NET 9 Console Application


#### Considerations:
+ SOLID
	- Single Responsibility Principle
		1. Separating reading, processing, and output.
		1. DI helps ensure that classes have a single responsibility by delegating object creation and management to a DI container.
		1. Each class is focused on one responsibility.
	- Open/Closed Principle
		1. By depending on abstractions (interfaces) rather than concrete implementations.
		1. We can introduce new implementations without changing the existing code.
	- Liskov Substitution Principle
		1. Using interfaces ensure that derived classes or implementations can be used interchangeably without altering the correctness of the program.
	- Interface Segregation
		1. Defining separate, small interfaces for different responsibilities so that clients won't have to implement the interfaces which they don't use.
	- Dependency Inversion Principle
		1. High-level modules are not depend on low-level modules but on abstractions.
		1. DI promotes this principle by injecting dependencies through interfaces rather than instantiating concrete classes directly.
+ Clean Code
	1. Use descriptive names.
	1. Avoid hardcoding strings, values, ….
	1. Minimize nested loops by moving functionality into classes.
	1. Use LINQ as it improves readability.
	1. Exception handling when dealing with files.
+ Testability
	1. Make use of interfaces to mock file reading and counting.
	1. Use dependency injection.
+ Performance
	1. Use `ConcurrentDictionary`
	1. Use built in options `RemoveEmptyEntries` and `OrdinalIgnoreCase` instead of `Trim` and `ToLowerCase` call.
	1. Use `Parallel` and `AsParallel` to utilize multi thrad parallel processing.
	1. Use `ForEachAsync` to utilize synchronous programming and avoid blocking I/O.
+ Optimization
	1. Avoid loading entire files into memory at once.
	1. Use `StreamReader` to process line by line instead of `File.OpenText`.

#### Used packages:
+ `System.Linq.Async` to use `ToListAsync` from `QueryableExtensions` class.
+ `FluentAssertion`
+ `NSubstitute` to mock `StreamReader` service

### Further improvements:
+ Async StreamReader:

Instead of calling `ReadLineAsync()` in a loop, we can leverage `await foreach` with an `IAsyncEnumerable<string>` in the consumer method for even better performance and responsiveness.

+ Improved Exception Handling:

Currently, the exceptions are caught and logged to the console. We could implement a centralized logging mechanism or allow the calling code to handle the exception.

Returning an empty enumerable might mask errors. Consider using a **custom result object** to communicate failures.

+ Dependency Injection for Separators:

The separators are currently hard-coded in the WordCounter class. It might be more flexible to inject them via configuration.

+ Improving Performance with Batching:

Instead of processing lines one by one, we could batch process lines to reduce the overhead of individual line processing.

+ Better Parallelization:

Instead of parallelizing file reading only, we could also parallelize word counting within each file.

This would take better advantage of multi-core systems.

+ Sorting Optimization:

The final sorting of results using .OrderBy(a => a.Value) could be optimized. Using a min-heap or priority queue might be more efficient for large datasets.

+ Separation of Concerns:

Consider separating file reading, word counting, and result aggregation into distinct services. This would make the code more modular and testable.

+ Configurable Output Formatting:

Instead of directly printing to the console, consider using an IOutputWriter interface to make output formatting and destination more flexible.