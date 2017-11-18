using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApplicationWcfRest1.models
{
    /// <summary>
    /// Represent a book
    /// </summary>
    [DataContract]
    public class Book
    {
        /// <summary>
        /// Book id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Book name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}