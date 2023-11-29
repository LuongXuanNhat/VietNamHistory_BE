using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace VNH.Infrastructure.Implement.Common
{
    public static class HandleCommon
    {
        public static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string SanitizeString(string input)
        {
            string withoutDiacritics = RemoveDiacritics(input).Trim().Replace(" ", "-");
            string sanitizedString = Regex.Replace(withoutDiacritics, "[^a-zA-Z0-9-]", "");

            return sanitizedString;
        }
        public static int GenerateRandomNumber()
        {
            Random random = new Random();
            return random.Next(100, 1000);
        }
        public static IEnumerable<string> GenerateSearchPhrases(string[] searchKeywords)
        {
            List<string> searchPhrases = new();
            for (int i = searchKeywords.Length; i >= 1; i--)
            {
                for (int j = 0; j <= searchKeywords.Length - i; j++)
                {
                    string phrase = string.Join(" ", searchKeywords.Skip(j).Take(i));
                    searchPhrases.Add(phrase);
                }
            }
            return searchPhrases;
        }
    }
}
