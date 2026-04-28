using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace Assignment6
{
    public class Service1 : IService1
    {
        public string WebDownload(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return "Error: URL cannot be null or empty. Please provide a valid URL.";

            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return "Error: URL must start with http:// or https://";

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (CSE445 Weather Planner)");
                return client.DownloadString(url);
            }
            catch (WebException webEx) { return "Error: Unable to download URL. " + webEx.Message; }
            catch (UriFormatException) { return "Error: The provided URL is not in a valid format."; }
            catch (Exception ex) { return "Error: An unexpected error occurred. " + ex.Message; }
        }

        private static readonly HashSet<string> StopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Articles
            "a", "an", "the",
            // Pronouns
            "i", "me", "my", "myself", "we", "our", "ours", "ourselves",
            "you", "your", "yours", "yourself", "yourselves",
            "he", "him", "his", "himself", "she", "her", "hers", "herself",
            "it", "its", "itself", "they", "them", "their", "theirs", "themselves",
            "what", "which", "who", "whom", "this", "that", "these", "those",
            // Verbs (common auxiliary/linking)
            "am", "is", "are", "was", "were", "be", "been", "being",
            "have", "has", "had", "having",
            "do", "does", "did", "doing",
            "will", "would", "shall", "should",
            "can", "could", "may", "might", "must",
            // Prepositions
            "about", "above", "across", "after", "against", "along", "among",
            "around", "at", "before", "behind", "below", "beneath", "beside",
            "between", "beyond", "by", "down", "during", "except", "for",
            "from", "in", "inside", "into", "like", "near", "of", "off",
            "on", "onto", "out", "outside", "over", "past", "since",
            "through", "throughout", "to", "toward", "towards", "under",
            "underneath", "until", "unto", "up", "upon", "with", "within", "without",
            // Conjunctions
            "and", "but", "or", "nor", "for", "yet", "so",
            "although", "because", "if", "once", "since", "than",
            "that", "though", "till", "unless", "until", "when",
            "where", "whether", "while",
            // Other common function words
            "no", "not", "only", "own", "same", "so", "than", "too",
            "very", "just", "also", "even", "still", "already",
            "each", "every", "both", "few", "more", "most", "other",
            "some", "such", "any", "all", "many", "much",
            "here", "there", "how", "why", "again", "further",
            "then", "once", "now", "ever", "never", "always",
            // Contractions (common in web text)
            "dont", "doesnt", "didnt", "isnt", "arent", "wasnt", "werent",
            "wont", "wouldnt", "cant", "couldnt", "shouldnt",
            "hasnt", "havent", "hadnt",
            // Web/HTML specific stop words
            "http", "https", "www", "com", "org", "net", "html", "htm",
            "class", "style", "div", "span", "href", "src", "alt",
            "nbsp", "amp", "lt", "gt", "quot"
        };

        public string WordFilter(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "Error: Input text cannot be null or empty.";

            string noTags = Regex.Replace(text, @"<[^>]+>", " ");

            // decode common HTML entities
            noTags = noTags.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">")
                           .Replace("&quot;", "\"").Replace("&nbsp;", " ").Replace("&#39;", "'");

            // remove remaining numeric entities (e.g. &#123;) and JS/CSS blocks
            noTags = Regex.Replace(noTags, @"&#?\w+;", " ");
            noTags = Regex.Replace(noTags, @"\{[^}]*\}", " ");
            noTags = Regex.Replace(noTags, @"//[^\n]*", " ");

            string[] words = noTags.Split(new char[] { ' ', '\t', '\n', '\r', '\f', '\v' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> contentWords = new List<string>();
            foreach (string rawWord in words)
            {
                string word = rawWord.Trim(
                    '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '[', ']',
                    '{', '}', '-', '_', '/', '\\', '|', '@', '#', '$', '%', '^',
                    '&', '*', '+', '=', '~', '`');

                if (string.IsNullOrWhiteSpace(word)) continue;

                bool allDigits = true;
                foreach (char c in word) { if (!char.IsDigit(c)) { allDigits = false; break; } }
                if (allDigits) continue;

                if (word.Length <= 1 && word.ToUpper() != "I") continue;
                if (StopWords.Contains(word)) continue;

                contentWords.Add(word);
            }

            return contentWords.Count == 0
                ? "(No content words found after filtering)"
                : string.Join(" ", contentWords);
        }
    }
}
