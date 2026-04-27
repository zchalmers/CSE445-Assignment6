// Service1.svc.cs - Service Implementation
// Project: WebAndFilterService (WCF Service Application)
// Author: Andrew Courter
// Course: CSE445 - Distributed Software Development
//
// Description: Implements all WCF service operations defined in IService1.cs:
//   - WebDownload: Downloads webpage content from a given URL
//   - WordFilter:  Strips stop words and HTML/XML tags from text


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Assignment6
{
    
    public class Service1 : IService1
    {
         
        // REQUIRED SERVICE 1: WebDownload Implementation

       
        /// <param name="url">A valid HTTP/HTTPS URL string</param>
        /// <returns>The page content as a string, or an error message</returns>
        public string WebDownload(string url)
        {
            // Input validation: check for null or empty URL
            if (string.IsNullOrWhiteSpace(url))
            {
                return "Error: URL cannot be null or empty. Please provide a valid URL.";
            }

            // Input validation: ensure URL starts with http:// or https://
            
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return "Error: URL must start with http:// or https://";
            }

            try
            {
                // Create a WebClient to perform the HTTP GET request
                
                WebClient client = new WebClient();

                // Set a user-agent header so servers don't reject the request
               
                client.Headers.Add("User-Agent",
                    "Mozilla/5.0 (CSE445 Assignment3 WebDownload Service)");

                // DownloadString performs HTTP GET and returns the response body
                // as a string.
                string content = client.DownloadString(url);

                // Return the downloaded content
                return content;
            }
            catch (WebException webEx)
            {
                // WebException covers HTTP errors 
                return "Error: Unable to download URL. " + webEx.Message;
            }
            catch (UriFormatException)
            {
                // UriFormatException means the URL string is malformed
                return "Error: The provided URL is not in a valid format.";
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                return "Error: An unexpected error occurred. " + ex.Message;
            }
        }

        
        // REQUIRED SERVICE 2: WordFilter Implementation
        

      
        private static readonly HashSet<string> StopWords = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
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

      
        /// <param name="text">The input text to filter</param>
        /// <returns>Filtered text with only content words remaining</returns>
        public string WordFilter(string text)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(text))
            {
                return "Error: Input text cannot be null or empty.";
            }

            // Remove all HTML/XML tags using regex
            
            string noTags = Regex.Replace(text, @"<[^>]+>", " ");

            // Decode common HTML entities to their text equivalents
            
            noTags = noTags.Replace("&amp;", "&");
            noTags = noTags.Replace("&lt;", "<");
            noTags = noTags.Replace("&gt;", ">");
            noTags = noTags.Replace("&quot;", "\"");
            noTags = noTags.Replace("&nbsp;", " ");
            noTags = noTags.Replace("&#39;", "'");

            // Remove any remaining HTML numeric entities (e.g., &#123;)
            noTags = Regex.Replace(noTags, @"&#?\w+;", " ");

            // Remove JavaScript and CSS content that might remain
            
            noTags = Regex.Replace(noTags, @"\{[^}]*\}", " "); 
            noTags = Regex.Replace(noTags, @"//[^\n]*", " ");   

            // Split text into words using whitespace and common delimiters
            
            char[] delimiters = { ' ', '\t', '\n', '\r', '\f', '\v' };
            string[] words = noTags.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // Filter out stop words and clean up each word
            List<string> contentWords = new List<string>();

            foreach (string rawWord in words)
            {
                // Strip leading and trailing punctuation from each word
                
                string word = rawWord.Trim(
                    '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '[', ']',
                    '{', '}', '-', '_', '/', '\\', '|', '@', '#', '$', '%', '^',
                    '&', '*', '+', '=', '~', '`');

                // Skip empty strings that result from stripping punctuation
                if (string.IsNullOrWhiteSpace(word))
                {
                    continue;
                }

                // Skip words that are purely numeric
                bool allDigits = true;
                foreach (char c in word)
                {
                    if (!char.IsDigit(c))
                    {
                        allDigits = false;
                        break;
                    }
                }
                if (allDigits && word.Length > 0)
                {
                    continue;
                }

                // Skip very short words 
                if (word.Length <= 1 && word.ToUpper() != "I")
                {
                    continue;
                }

                // Check if the word is a stop word 
                if (StopWords.Contains(word))
                {
                    continue; 
                }

                // This word passed all filters - it is a content word
                contentWords.Add(word);
            }

            // Join content words with single spaces and return
            if (contentWords.Count == 0)
            {
                return "(No content words found after filtering)";
            }

            return string.Join(" ", contentWords);
        }
    }
}
