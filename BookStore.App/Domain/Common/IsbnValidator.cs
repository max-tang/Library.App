

namespace BookStore.App
{
    public static class IsbnValidator
    {
        public static bool IsValid(string isbn)
        {
            string normalized = isbn.Replace("-", string.Empty).Replace(" ", string.Empty);
            if (normalized.Length != 10 && normalized.Length != 13)
            {
                return false;
            }
            return true;
        }
    }
}
