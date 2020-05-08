using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class BookShelfIterator : Iterator
    {
        private BookShelf bookShelf;
        private int index;
        public BookShelfIterator(BookShelf bookShelf)
        {
            this.bookShelf = bookShelf;
            index = 0;
        }
        public bool hasNext()
        {
            if (index < bookShelf.GetLength())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object Next()
        {
            Book book=bookShelf.GetBookAt(index++);
            return book;
        }
    }
}
