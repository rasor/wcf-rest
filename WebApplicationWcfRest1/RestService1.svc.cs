using System.Diagnostics;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using WebApplicationWcfRest1.interfaces;
using WebApplicationWcfRest1.models;

namespace WebApplicationWcfRest1
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestService1 : IBookService
    {

        public void AddBook(Book book)
        {
            Debug.WriteLine(book.Name);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created; // 201
            if (book.Name == "The incredible stamp") { // Book exist
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Conflict; // 409
            }
        }

        public void DeleteBook(string id)
        {
            Debug.WriteLine(id);
        }

        public Book GetBookById(string id)
        {
            if (id == "2") { // Book does not exist - 404
                WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound("Resource not found");
                return null;
            } else {
                return new Book() { Id = 1, Name = "The incredible stamp" };
            }
        }

        public Book[] GetBooksList()
        {
            return new Book[] {new Book() {Id = 1, Name= "The incredible stamp" }};
        }

        public void UpdateBook(Book book)
        {
            Debug.WriteLine(book.Name);
            if (book.Id == 2) { // Book does not exist - 404
                WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound("Resource not found");
            } else if (book.Name == "") { // Invalid request
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.MethodNotAllowed; // 405
            }
        }
    }
}
