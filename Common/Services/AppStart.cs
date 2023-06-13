namespace Common.Services;

public class AppStart: IAppStart
{
    private readonly IBookService _bookservice;
    public AppStart(IBookService bookService)
    {
        _bookservice = bookService;
    }

    public async Task Run()
    {
        try{
            
            List<Book> initBookList = await _bookservice.GetAllBooks();
            List<List<Book>> filteredBookList = await _bookservice.FilterBooks(initBookList);
            if (filteredBookList.Any())
            {
                string bookset = string.Empty;
                foreach (var bookList in filteredBookList) 
                {
                    bookset += bookList.FirstOrDefault().parent_name + Environment.NewLine;
                    foreach (var book in bookList)
                    {
                        bookset += book.display_name + " " + String.Join(",", book.meta.states) + Environment.NewLine;
                    }
                }
                _bookservice.SaveBooks(bookset);
            }
            Console.WriteLine("All done! Check project folder for books.txt for data on books");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        await Task.CompletedTask;
    }
}