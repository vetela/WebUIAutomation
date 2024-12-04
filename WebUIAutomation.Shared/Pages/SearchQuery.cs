namespace WebUIAutomation.Shared.Pages;

public class SearchQuery
{
	public string Term { get; private set; }

	private SearchQuery() { }

	public class Builder
	{
		private readonly SearchQuery _query = new();

		public Builder WithTerm(string term)
		{
			_query.Term = term;
			return this;
		}

		public SearchQuery Build() => _query;
	}
}
