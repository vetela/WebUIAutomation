using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;
using System.Text.Json;

namespace WebUIAutomation.NUnit_API.BaseTests;

public class BaseTest
{
	protected HttpClient Client;
	protected IConfiguration Configuration;

	[SetUp]
	public async Task Setup()
	{
		Configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName)
			.AddJsonFile("appsettings.json")
			.AddUserSecrets<BaseTest>()
			.Build();

		string baseUrl = Configuration["ApiBaseUrl"]!;
		Client = new HttpClient { BaseAddress = new(baseUrl) };

		var token = await GetAuthTokenAsync();
		Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
	}

	[TearDown]
	public async Task Teardown()
	{
		Client.Dispose();
	}


	protected async Task<string> GetAuthTokenAsync()
	{
		var tokenUrl = Configuration["Auth:TokenUrl"];
		var clientId = Configuration["Auth:ClientId"];
		var clientSecret = Configuration["Auth:ClientSecret"];
		var scope = Configuration["Auth:Scope"];
		var grantType = Configuration["Auth:GrantType"];

		var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
			{
				Content = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("client_Id", clientId),
				new KeyValuePair<string, string>("client_Secret", clientSecret),
				new KeyValuePair<string, string>("scope", scope),
				new KeyValuePair<string, string>("grant_type", grantType)
			})
		};

		var response = await Client.SendAsync(tokenRequest);
		response.EnsureSuccessStatusCode();

		var responseBody = await response.Content.ReadAsStringAsync();
		var token = JsonSerializer.Deserialize<JsonElement>(responseBody).GetProperty("access_token").GetString();

		return token!;
	}
}
