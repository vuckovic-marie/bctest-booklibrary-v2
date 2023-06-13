namespace Common.Services;

public class BookService : IBookService
{
    private readonly string _bookUrl = "https://api.actionnetwork.com/web/v1/books";
    //private readonly HttpClient _httpClient = new HttpClient();

    public async Task<List<Book>> GetAllBooks()
    {
        List<Book> allBooks = new List<Book>();
        try
        {
            var response = await GetBooksApi(_bookUrl);
            try
            {
                var jsonBooks = System.Text.Json.JsonSerializer.Deserialize<BookObj>(response);
                allBooks.AddRange(jsonBooks.books);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        await Task.CompletedTask;
        return allBooks;
    }

    private async Task<string> GetBooksApi(string url)
    {
        var response = await new HttpClient().GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<List<Book>>> FilterBooks(List<Book> bookList)
    {
        List<List<Book>> groupedBooks = new List<List<Book>>();
        if (bookList.Any())
        {
            var books = bookList.Where(x => x.parent_name != null && !x.meta.states.Contains("NJ") && !x.meta.states.Contains("CO"))
                .GroupBy(x => x.parent_name).OrderBy(x => x.Key);
            foreach (var list in books)
            {
                groupedBooks.Add(list.ToList());
            }
        }
        
        await Task.CompletedTask;
        return groupedBooks;
    }

    public void SaveBooks(string Bookset)
    {
        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        string fileUrl = projectDirectory + "/books.txt";
        
        if (File.Exists(fileUrl)) { File.Delete(fileUrl); }
        using(StreamWriter sw = File.CreateText(fileUrl)) {
            sw.WriteLine(Bookset);
        }
    }
}