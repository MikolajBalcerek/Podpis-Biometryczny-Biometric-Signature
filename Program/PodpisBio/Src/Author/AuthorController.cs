using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    class AuthorController
    {
        public List<Author> authors;

        public AuthorController() { }

        public void addAuthor(Author author)
        {
            authors.Add(author);
        }
    }
}
