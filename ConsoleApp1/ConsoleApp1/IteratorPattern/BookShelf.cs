using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class BookShelf:IAggregate
    {
        private List<Book> books;
        private int last = 0;
        public BookShelf()
        {
            books = new List<Book>(); 
        }
        public Book GetBookAt(int index)
        {
            if (index >= 0 && index < books.Count)
            {
                return books[index];
            }
            return new Book("");
        }
        public void AppendBook(Book book)
        {
            if(book==null)
            {
                return;
            }
            books.Add(book);
            last = books.Count - 1;
        }
        public int GetLength()
        {
            return books.Count;
        }
        public Iterator iterator()
        {
            return new BookShelfIterator(this);
        }
    }
}
