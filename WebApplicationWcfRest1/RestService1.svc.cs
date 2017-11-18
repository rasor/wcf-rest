using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
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
        }

        public void DeleteBook(string id)
        {
            Debug.WriteLine(id);
        }

        public Book GetBookById(string id)
        {
            return new Book() {Id = 1, Name= "The incredible stamp" };
        }

        public Book[] GetBooksList()
        {
            return new Book[] {new Book() {Id = 1, Name= "The incredible stamp" }};
        }

        public void UpdateBook(Book book)
        {
            Debug.WriteLine(book.Name);
        }
    }
}
