using Newtonsoft.Json;
using PodpisBio.Src.Author;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PodpisBio.Src.Service
{
    class AuthorService : RestService
    {
        public AuthorService() { getObjectAsync<Object>(""); }

        public Author.Author getAuthor(int id)
        {
            return getObjectAsync<Author.Author>("Authors/"+id);
        }

        public List<Author.Author> getAuthors()
        {
            return getObjectAsync<List<Author.Author>>("Authors/");
        }

        public Author.Author postAuthor(Author.Author author)
        {
            return postObjectAsync<Author.Author>("Authors/", author);
        }
    }
}
