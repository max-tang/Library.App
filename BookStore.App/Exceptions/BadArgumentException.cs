using System;

namespace BookStore.App
{

    public class BadArgumentException : ApplicationException
    {
        public BadArgumentException(string message) : base(message) { }
    }
}
