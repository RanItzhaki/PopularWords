using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PopularWords
{
    // The class manages the scanning of a url for the top 10 appeared words in it.
    public partial class MostPopularWords : System.Web.UI.Page
    {
        // The method scanns the text of the url and presents the top ten words in it with their appearances counters.
        protected void ProcessContentFromUrl(object sender, EventArgs e)
        {
            if (urlInput.Text != string.Empty && urlInput.Text != null)
            {
                List<string> wordsList = new List<string>();
                List<string> extendedWordsList = new List<string>();
                List<string> tenMostPopularWords = null;
                List<int> tenHeightsAppearances = null;
                string[] wordsArray = null;
                string alertMessage = string.Empty;

                try
                {
                    string urlOfUser = urlInput.Text;
                    string urlContent = string.Empty;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlOfUser);
                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

                    // Reads and stores all the content of the url.
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        urlContent = reader.ReadToEnd();
                    }

                    // Collects all the data, which is associated with the 'content' attribute.
                    htmlDoc.LoadHtml(urlContent);
                    foreach (var item in htmlDoc.DocumentNode.SelectNodes("//*[@content]"))
                    {
                        wordsList.Add(item.GetAttributeValue("content", ""));
                    }

                    // Collects all the data, which is associated with the 'title' attribute.
                    foreach (var item in htmlDoc.DocumentNode.SelectNodes("//*[@title]"))
                    {
                        wordsList.Add(item.GetAttributeValue("title", ""));
                    }
                }
                catch (WebException)
                {
                }
                catch (UriFormatException)
                {
                    alertMessage = "alert(\"Illegal Input!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", alertMessage, true);
                }
                catch (NullReferenceException)
                {
                }
                finally
                {
                    if (alertMessage == string.Empty)
                    {
                        foreach (string phrase in wordsList)
                        {
                            wordsArray = Regex.Split(phrase, @"\s");
                            extendedWordsList.AddRange(wordsArray);
                        }

                        // Gets the top 10 popular words in the url and the number of their appearances.
                        tenMostPopularWords = GetTenMostPopularWords(extendedWordsList, out tenHeightsAppearances);

                        // Prints the top 10 popular words in the url and the number of their appearances on the screen.
                        PrintWordsOnScreen(tenMostPopularWords, tenHeightsAppearances);
                    }
                }
            }
            else
            {
                string alertMessage = "alert(\"Empty Input\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", alertMessage, true);
            }
        }

        // Gets the top 10 popular words in the url and the number of their appearances.
        protected List<string> GetTenMostPopularWords(List<string> wordsList, out List<int> countersList)
        {
            List<string> tenMostPopularWords = new List<string>();
            Dictionary<string, int> wordsCounter = new Dictionary<string, int>();
            string currentWord = string.Empty;
            int currentMaxValue = 0;
            int currentCount = 0;
            countersList = new List<int>();

            foreach (string word in wordsList)
            {
                if (word != null && word != string.Empty)
                {
                    if (wordsCounter.ContainsKey(word)) // If the word exists in the dictionary, increases its appearances counter, else adds it.
                    {
                        wordsCounter.TryGetValue(word, out currentCount);
                        currentCount++;
                        wordsCounter.Remove(word);
                        wordsCounter.Add(word, currentCount);
                    }
                    else
                    {
                        wordsCounter.Add(word, 1);
                    }
                }
            }

            // Gets the top 10 popular words in the url and the number of their appearances.
            while (tenMostPopularWords.Count < 10 && wordsCounter.Count > 0)
            {
                currentMaxValue = wordsCounter.Values.Max();
                currentWord = wordsCounter.FirstOrDefault(x => x.Value == currentMaxValue).Key;
                tenMostPopularWords.Add(currentWord);
                countersList.Add(currentMaxValue);
                wordsCounter.Remove(currentWord);
            }

            return tenMostPopularWords;
        }

        // Prints the top 10 popular words in the url and the number of their appearances on the screen.
        protected void PrintWordsOnScreen(List<string> wordsList, List<int> countersList)
        {
            StringBuilder listOfWords = new StringBuilder();
            int currentIndex = 0;

            for (int i = 0; i < wordsList.Count; i++)
            {
                currentIndex = i + 1;
                listOfWords.Append("<strong>" + currentIndex + ") " + wordsList[i] + "</strong><br>");
                listOfWords.Append("<strong>Appearances: " + countersList[i] + "</strong><br><br>");
            }

            top10.InnerHtml = listOfWords.ToString();
        }

        // The method clears the screen.
        protected void ClearScreen(object sender, EventArgs e)
        {
            urlInput.Text = string.Empty;
            top10.InnerHtml = string.Empty;
        }
    }
}