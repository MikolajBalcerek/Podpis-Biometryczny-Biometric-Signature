using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    class AuthorController
    {
        public List<Author> authors = new List<Author>();

        public AuthorController() { }

        public void addAuthor(String name)
        {
            this.authors.Add(new Author(authors.Count, name));
        }
        public List<String> getAuthorsNames()
        {
            List<String> names = new List<String>();

            foreach(var author in authors) { names.Add(author.getName()); }

            return names;
        }

        public Author getAuthor(String name)
        {
            foreach(var author in authors)
            {
                if (author.getName().Equals(name))
                {
                    return author;
                }
            }
            return null;
        }

        public bool contains(String name)
        {
            foreach(var author in authors)
            {
                if (author.getName().Equals(name)) { return true; }
            }
            return false;
        }
    }
}
