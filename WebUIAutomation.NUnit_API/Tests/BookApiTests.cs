using AventStack.ExtentReports;
using System.Text;
using System.Text.Json;
using WebUIAutomation.NUnit_API.BaseTests;
using WebUIAutomation.NUnit_Tests.Report;

namespace WebUIAutomation.NUnit_API.Tests;

public class BookApiTests : BaseTest
{
	private ExtentTest _test;
	protected List<string> CreatedBookIds;

	[SetUp]
	public async Task TestSetup()
	{
		CreatedBookIds = [];
		await PrepopulateTestDataAsync();
		_test = ReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
	}

	[TearDown]
	public async Task TestTeardown()
	{
		await CleanupTestDataAsync();
		var status = TestContext.CurrentContext.Result.Outcome.Status;
		var message = TestContext.CurrentContext.Result.Message;

		if (status == NUnit.Framework.Interfaces.TestStatus.Passed)
		{
			_test.Pass("Test passed");
		}
		else if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
		{
			_test.Fail($"Test failed: {message}");
		}
	}

	[OneTimeTearDown]
	public void GenerateReport()
	{
		ReportManager.Flush();
	}

	[Test]
	public async Task CreateBook_ShouldReturn201AndMatchInput()
	{
		_test.Info("Starting CreateBook test");

		var book = new
		{
			title = "War and Peace",
			author = "Leo Tolstoy",
			isbn = "978-0119232633",
			publishedDate = "1869-01-01T00:00:00Z"
		};

		var content = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");
		var response = await Client.PostAsync(Configuration["Endpoints:CreateBook"], content);

		Assert.AreEqual(201, (int)response.StatusCode, "Book creation should return status 201.");
		_test.Pass("Book creation returned status 201.");

		var responseBody = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());

		Assert.IsTrue(responseBody.TryGetProperty("title", out var titleElement), "Response should contain 'title'.");
		Assert.AreEqual(book.title, titleElement.GetString(), "Book title should match.");
		_test.Pass("Book title matches input.");

		Assert.IsTrue(responseBody.TryGetProperty("author", out var authorElement), "Response should contain 'author'.");
		Assert.AreEqual(book.author, authorElement.GetString(), "Book author should match.");
		_test.Pass("Book author matches input.");

		Assert.IsTrue(responseBody.TryGetProperty("isbn", out var isbnElement), "Response should contain 'isbn'.");
		Assert.AreEqual(book.isbn, isbnElement.GetString(), "Book ISBN should match.");
		_test.Pass("Book ISBN matches input.");

		Assert.IsTrue(responseBody.TryGetProperty("publishedDate", out var publishedDateElement), "Response should contain 'publishedDate'.");
		Assert.AreEqual(book.publishedDate, publishedDateElement.GetString(), "Book published date should match.");
		_test.Pass("Book published date matches input.");

		responseBody.TryGetProperty("id", out var id);
		await DeleteBookAsync(id.GetString()!);
	}

	[Test]
	public async Task CreateDuplicateBook_ShouldReturn409()
	{
		_test.Info("Starting CreateDuplicateBook test");

		var book = new
		{
			title = "Duplicate Book",
			author = "Author",
			isbn = "978-0000004000",
			publishedDate = "2000-01-01T00:00:00Z"
		};

		var bookId = await CreateBookAsync(book.title, book.author, book.isbn, book.publishedDate);
		_test.Pass($"Initial book created with ID: {bookId}");

		var content = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");

		var response = await Client.PostAsync(Configuration["Endpoints:CreateBook"], content);
		Assert.AreEqual(409, (int)response.StatusCode, "Duplicate book creation should return status 400.");
		_test.Pass("Duplicate book creation returned status 409 as expected.");
	}

	[Test]
	public async Task GetAllBooks_ShouldReturnList()
	{
		_test.Info("Starting GetAllBooks_ShouldReturnList test");
		var response = await Client.GetAsync(Configuration["Endpoints:GetAllBooks"]);

		Assert.AreEqual(200, (int)response.StatusCode, "Getting all books should return status 200.");
		_test.Pass("Getting all books returned status 200.");

		var responseBody = JsonSerializer.Deserialize<List<dynamic>>(await response.Content.ReadAsStringAsync());
		Assert.IsNotEmpty(responseBody, "Books list should not be empty.");
		_test.Pass("Books list is not empty.");
	}

	[Test]
	public async Task GetAllBooks_ShouldContainRequiredFields()
	{
		_test.Info("Starting GetAllBooks_ShouldContainRequiredFields test");

		var response = await Client.GetAsync(Configuration["Endpoints:GetAllBooks"]);
		Assert.AreEqual(200, (int)response.StatusCode, "Getting all books should return status 200.");
		_test.Pass("Getting all books returned status 200.");

		var responseBody = await response.Content.ReadAsStringAsync();
		var books = JsonSerializer.Deserialize<List<JsonElement>>(responseBody);

		Assert.IsNotEmpty(books, "Books list should not be empty.");
		_test.Pass("books list is not empty.");

		foreach (var book in books)
		{
			Assert.IsTrue(book.TryGetProperty("title", out _), "Response should contain 'title'.");
			_test.Pass("books list contains 'title'.");

			Assert.IsTrue(book.TryGetProperty("author", out _), "Response should contain 'author'.");
			_test.Pass("books list contains 'author'.");

			Assert.IsTrue(book.TryGetProperty("publishedDate", out _), "Response should contain 'publishedDate'.");
			_test.Pass("books list contains 'publishedDate'.");
		}
	}

	[Test]
	public async Task GetBookById_ShouldReturnBook()
	{
		_test.Info("Starting GetBookById test");

		var bookId = await CreateBookAsync("Jane Eyre", "Charlotte Bronte", "978-0121341146", "1847-10-16T00:00:00Z");
		_test.Pass($"Test book created with ID: {bookId}");

		var response = await Client.GetAsync(Configuration["Endpoints:GetBookById"]!.Replace("{id}", bookId));

		Assert.AreEqual(200, (int)response.StatusCode, "Getting a book by ID should return status 200.");
		_test.Pass("Fetching book by ID returned status 200.");

		var responseBody = await response.Content.ReadAsStringAsync();
		var book = JsonSerializer.Deserialize<JsonElement>(responseBody);

		Assert.IsTrue(book.TryGetProperty("id", out var idElement), "Response should contain 'id'.");
		_test.Pass("Response contains 'id'.");

		Assert.AreEqual(bookId, idElement.GetString(), "Returned book ID should match the requested ID.");
		_test.Pass("Returned book ID matches the requested ID.");

		Assert.IsTrue(book.TryGetProperty("title", out var titleElement), "Response should contain 'title'.");
		Assert.AreEqual("Jane Eyre", titleElement.GetString(), "Returned book title should match.");
		_test.Pass("Book title matches expected value.");

		Assert.IsTrue(book.TryGetProperty("isbn", out var isbnElement), "Response should contain 'isbn'.");
		Assert.AreEqual("978-0121341146", isbnElement.GetString(), "Returned book isbn should match.");
		_test.Pass("Book isbn matches expected value.");

		book.TryGetProperty("id", out var id);
		await DeleteBookAsync(id.GetString()!);
	}

	[Test]
	public async Task GetBookByNonExistentId_ShouldReturn404()
	{
		_test.Info("Starting GetBookByNonExistentId test");

		var bookId = "c5b7b183-59c0-4d71-9b55-b0d15291d9e3";
		await DeleteBookAsync(bookId);

		var response = await Client.GetAsync(Configuration["Endpoints:GetBookById"]!.Replace("{id}", bookId));

		Assert.AreEqual(404, (int)response.StatusCode, "Fetching a non-existent book should return status 404.");
		_test.Pass("Fetching a non-existent book returns status 404.");
	}

	[Test]
	public async Task GetBookByInvalidIdFormat_ShouldReturn400()
	{
		_test.Info("Starting GetBookByInvalidIdFormat test");
		var response = await Client.GetAsync(Configuration["Endpoints:GetBookById"]!.Replace("{id}", "invalid-id-format"));
		Assert.AreEqual(400, (int)response.StatusCode, "Fetching a book with invalid ID format should return status 400.");
		_test.Pass("Fetching a a book with invalid ID format returns status 400.");
	}

	[Test]
	public async Task UpdateBook_ShouldUpdateAndValidateData()
	{
		_test.Info("Starting UpdateBook test");

		var bookId = await CreateBookAsync("Old Title", "Old Author", "978-0000100001", "2000-01-01T00:00:00Z");
		_test.Pass($"Test book created with ID: {bookId}");

		var updatedBook = new
		{
			title = "Updated Title",
			author = "Updated Author",
			publishedDate = "2023-01-01T00:00:00Z"
		};

		var content = new StringContent(JsonSerializer.Serialize(updatedBook), Encoding.UTF8, "application/json");
		var response = await Client.PutAsync(Configuration["Endpoints:UpdateBook"].Replace("{id}", bookId), content);

		Assert.AreEqual(204, (int)response.StatusCode, "Updating a book should return status 204.");
		_test.Pass("Book updated successfully with status 204.");

		// Fetch updated book
		var getResponse = await Client.GetAsync($"/Books/{bookId}");
		Assert.AreEqual(200, (int)getResponse.StatusCode, "Fetching updated book should return status 200.");
		_test.Pass("Fetching updated book returns status 200.");

		var responseBody = JsonSerializer.Deserialize<JsonElement>(await getResponse.Content.ReadAsStringAsync());
		Assert.IsTrue(responseBody.TryGetProperty("title", out var titleElement), "Response should contain 'title'.");
		Assert.AreEqual(updatedBook.title, titleElement.GetString(), "Updated title should match.");
		_test.Pass("Book title matches expected value.");

		Assert.IsTrue(responseBody.TryGetProperty("author", out var authorElement), "Response should contain 'author'.");
		Assert.AreEqual(updatedBook.author, authorElement.GetString(), "Updated author should match.");
		_test.Pass("Book author matches expected value.");

		responseBody.TryGetProperty("id", out var id);
		await DeleteBookAsync(id.GetString()!);
	}


	[Test]
	public async Task UpdateBookWithNonExistentId_ShouldReturn404()
	{
		_test.Info("Starting UpdateBookWithNonExistentId test");

		var bookId = "c5b7b183-59c0-4d71-9b55-b0d15291d9e3";
		await DeleteBookAsync(bookId);

		var updatedBook = new
		{
			title = "Updated Title",
			author = "Updated Author",
			publishedDate = "2023-01-01T00:00:00Z"
		};

		var content = new StringContent(JsonSerializer.Serialize(updatedBook), Encoding.UTF8, "application/json");
		var response = await Client.PutAsync(Configuration["Endpoints:UpdateBook"]!.Replace("{id}", bookId), content);
		Assert.AreEqual(404, (int)response.StatusCode, "Updating a non-existent book should return status 404.");
		_test.Pass("Updating a non-existent book returns status 404.");
	}

	[Test]
	public async Task UpdateBookWithInvalidIdFormat_ShouldReturn400()
	{
		_test.Info("Starting UpdateBookWithInvalidIdFormat test");
		var content = new StringContent("{\"title\": \"Test\"}", Encoding.UTF8, "application/json");
		var response = await Client.PutAsync(Configuration["Endpoints:UpdateBook"]!.Replace("{id}", "invalid-id-format"), content);
		Assert.AreEqual(400, (int)response.StatusCode, "Updating a book with invalid ID format should return status 400.");
		_test.Pass("Updating a a book with invalid ID format returns status 400.");
	}

	[Test]
	public async Task DeleteBook_ShouldReturn204()
	{
		_test.Info("Starting DeleteBook test");

		var bookId = await CreateBookAsync("Book to Delete", "Author", "978-9999999999", "2023-01-01T00:00:00Z");
		_test.Pass($"Test book created with ID: {bookId}");

		var response = await Client.DeleteAsync(Configuration["Endpoints:DeleteBook"].Replace("{id}", bookId));
		Assert.AreEqual(204, (int)response.StatusCode, "Deleting a book should return status 204.");

		var getResponse = await Client.GetAsync($"/Books/{bookId}");
		Assert.AreEqual(404, (int)getResponse.StatusCode, "Fetching a deleted book should return status 404.");
		_test.Pass("Book deleted successfully with status 204.");
	}

	[Test]
	public async Task DeleteNonExistentBook_ShouldReturn404()
	{
		_test.Info("Starting DeleteNonExistentBook test");
		var bookId = "c5b7b183-59c0-4d71-9b55-b0d15291d9e3";
		await DeleteBookAsync(bookId);

		var response = await Client.DeleteAsync(Configuration["Endpoints:DeleteBook"]!.Replace("{id}", bookId));
		Assert.AreEqual(404, (int)response.StatusCode, "Deleting a non-existent book should return status 404.");
		_test.Pass("Deleting a non-existent book returns status 404.");

		_test.Info("Starting UpdateBookWithNonExistentId test");
	}

	[Test]
	public async Task DeleteBookWithInvalidIdFormat_ShouldReturn400()
	{
		_test.Info("Starting UpdateBookWithInvalidIdFormat test");
		var response = await Client.DeleteAsync(Configuration["Endpoints:DeleteBook"]!.Replace("{id}", "invalid-id-format"));
		Assert.AreEqual(400, (int)response.StatusCode, "Deleting a book with invalid ID format should return status 400.");
		_test.Pass("Deleting a book with invalid ID format returns status 400.");

	}

	protected async Task<string> CreateBookAsync(string title, string author, string isbn, string publishedDate)
	{
		var book = new { title, author, isbn, publishedDate };
		var content = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");

		var response = await Client.PostAsync(Configuration["Endpoints:CreateBook"], content);
		response.EnsureSuccessStatusCode();

		var responseBody = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
		var bookId = responseBody.GetProperty("id").GetString();
		if (bookId != null) CreatedBookIds.Add(bookId);

		return bookId!;
	}

	protected async Task DeleteBookAsync(string bookId)
	{
		var response = await Client.DeleteAsync(Configuration["Endpoints:DeleteBook"].Replace("{id}", bookId));
		if (response.IsSuccessStatusCode)
		{
			CreatedBookIds.Remove(bookId);
		}
	}

	private async Task PrepopulateTestDataAsync()
	{
		await CreateBookAsync("Test Book", "Test Author", "123-4567890123", "2023-01-01T00:00:00Z");
	}

	private async Task CleanupTestDataAsync()
	{
		var idsToRemove = CreatedBookIds.ToArray();
		foreach (var bookId in idsToRemove)
		{
			await DeleteBookAsync(bookId);
		}
	}
}
