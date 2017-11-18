using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using WebApplicationWcfRest1.models;

namespace WebApplicationWcfRest1.interfaces
{
    /// <summary>
    /// Service crud for book
    /// </summary>
    [ServiceContract]
    public interface IBookService
    {
        /// <summary>
        /// Get list of book
        /// </summary>
        /// <returns>list of book</returns>
        [OperationContract]
        [WebGet(UriTemplate = "Book", 
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Book[] GetBooksList();

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns>book</returns>
        [OperationContract]
        [WebGet(UriTemplate = "Book/{id}", 
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Book GetBookById(string id);

        /// <summary>
        /// Add book
        /// </summary>
        /// <param name="name">name of book</param>
        /// <returns>id of book</returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Book", 
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void AddBook(Book book);

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="id">if of book</param>
        /// <param name="name">name of book</param>
        /// <returns>id of book</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Book",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void UpdateBook(Book book);

        /// <summary>
        /// Delete book
        /// </summary>
        /// <param name="id">id of book</param>
        /// <returns>id of book</returns>
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Book/{id}", 
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteBook(string id);
    }
}