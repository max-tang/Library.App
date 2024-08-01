

namespace Library.App
{
    class IsbnFormatInfo
    {
        public int[] Weights { get; }
        public int NumberOfDigits { get; }

        public int CheckDivider { get; }

        public IsbnFormatInfo(int[] weigts, int numberOfDigits, int checkDivider)
        {
            Weights = weigts;
            NumberOfDigits = numberOfDigits;
            CheckDivider = checkDivider;
        }
    }

    public static class IsbnValidator
    {
        private static readonly IsbnFormatInfo[] IsbnFormats;

        static IsbnValidator()
        {
            IsbnFormats = new IsbnFormatInfo[] {
                new(new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 10, 11), // ISBN 10
                new(new int[] { 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1 }, 13, 10), // ISBN 13
            };
        }

        /// <summary>
        /// Implemtents the ISBN check for both <see href="https://freeisbn.com/10-digit-isbn"/>ISBN 10</see> 
        /// and <see href="https://freeisbn.com/13-digit-isbn"/>ISBN 13</see>.
        /// </summary>
        /// <param name="normalizedIsbn"></param>
        /// <param name="isbnFormat"></param>
        /// <returns></returns>
        public static bool IsValid(string isbn)
        {
            if (isbn == null)
            {
                return false;
            }

            string normalized = isbn.Replace("-", string.Empty).Replace(" ", string.Empty);

            foreach (IsbnFormatInfo isbnFormat in IsbnFormats)
            {
                if (IsValidISBN(normalized, isbnFormat))
                {
                    return true;
                }
            }

            return false;
        }


        private static bool IsValidISBN(string normalizedIsbn, IsbnFormatInfo isbnFormat)
        {
            if (normalizedIsbn.Length != isbnFormat.NumberOfDigits)
            {
                return false;
            }

            int sum = 0;

            for (int i = 0; i < isbnFormat.NumberOfDigits - 1; ++i)
            {
                if (!char.IsDigit(normalizedIsbn[i]))
                {
                    return false;
                }
                sum += isbnFormat.Weights[i] * Convert.ToInt32(normalizedIsbn[i]);
            }

            if (normalizedIsbn[isbnFormat.NumberOfDigits - 1] == 'X')
            {
                sum += 10 * isbnFormat.Weights[isbnFormat.NumberOfDigits - 1];
            }
            else
            {
                sum += Convert.ToInt32(normalizedIsbn[isbnFormat.NumberOfDigits - 1]) * isbnFormat.Weights[isbnFormat.NumberOfDigits - 1];
            }

            return sum % isbnFormat.CheckDivider == 0;
        }
    }
}
