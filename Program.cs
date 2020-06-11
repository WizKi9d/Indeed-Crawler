using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Xml;
using HtmlAgilityPack;

namespace webcrawlermedium
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            bool On = true;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Cyan;
            string spiderDrop = "                                            | \n";

            for (int i = 0; i < 10; i++) {
                Console.Clear();
                Console.WriteLine(@"
██╗███╗░░██╗██████╗░███████╗███████╗██████╗░  ░█████╗░██████╗░░█████╗░░██╗░░░░░░░██╗██╗░░░░░███████╗██████╗░
██║████╗░██║██╔══██╗██╔════╝██╔════╝██╔══██╗  ██╔══██╗██╔══██╗██╔══██╗░██║░░██╗░░██║██║░░░░░██╔════╝██╔══██╗
██║██╔██╗██║██║░░██║█████╗░░█████╗░░██║░░██║  ██║░░╚═╝██████╔╝███████║░╚██╗████╗██╔╝██║░░░░░█████╗░░██████╔╝
██║██║╚████║██║░░██║██╔══╝░░██╔══╝░░██║░░██║  ██║░░██╗██╔══██╗██╔══██║░░████╔═████║░██║░░░░░██╔══╝░░██╔══██╗
██║██║░╚███║██████╔╝███████╗███████╗██████╔╝  ╚█████╔╝██║░░██║██║░░██║░░╚██╔╝░╚██╔╝░███████╗███████╗██║░░██║
╚═╝╚═╝░░╚══╝╚═════╝░╚══════╝╚══════╝╚═════╝░  ░╚════╝░╚═╝░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░╚═╝░░╚══════╝╚══════╝╚═╝░░╚═╝");
                Console.WriteLine(spiderDrop);
                Console.CursorTop--;
                Console.WriteLine(@"                                            .
                                           / \
                                       \  / _ \  /
                                        \_\(_)/_/
                                       ___//o\\___
                                          /   \    
                                         /     \");
                spiderDrop = spiderDrop + "                                            | \n";
                Thread.Sleep(120);
            }

            Thread.Sleep(350);
            Console.WriteLine("Made by: Alex Wzorek");
            Thread.Sleep(500);

            while (On)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.CursorVisible = true;
                Console.WriteLine("Enter search term: ");
                string searchTerm = Console.ReadLine();
                string transformSearch = TransformSearch(searchTerm);

                // List of links
                List<String> hrefList = new List<string>();

                // Get web page html 
                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.indeed.co.uk/jobs?q=" + transformSearch + "&l=hp10+9ex");


                try
                {
                    // Get all headers of jobs available
                    //var HeaderNames = doc.DocumentNode
                    //                     .SelectNodes("//h2[@class='title']").ToList();
                    // Get all links to jobs available
                    var ArticleLinks = doc.DocumentNode
                                          .SelectNodes("//a[@href]").ToList();

                    foreach (var item in ArticleLinks) {
                        string linkItem = item.Attributes["href"].Value;
                        //Console.WriteLine(linkItem);

                        char[] linkArray = linkItem.Take(8).ToArray();
                        string linkSubstring = new string(linkArray);
                        if(linkSubstring == "/pagead/" || linkSubstring == "/company" || linkSubstring == "/rc/clk?") {
                            // Add links to list
                            hrefList.Add("https://indeed.co.uk" + linkItem);
                        }
                    }
                    /*
                    foreach(string itemTwo in hrefList) {
                        Console.WriteLine(itemTwo);
                    }*/

                    List<String> usefulLinks = new List<string>();

                    int listLen = hrefList.Count;
                    for (int i = 0; i < listLen; i++)
                    {
                        string getLink = hrefList[i];
                        HtmlAgilityPack.HtmlDocument docTwo = web.Load(getLink);

                        var PageText = docTwo.DocumentNode
                                             .SelectNodes("//p");
                        if(PageText != null) {
                            PageText.ToList();
                        } else {
                            break;
                        }
                        //var checkForNodes = docTwo.DocumentNode
                        //                          .Descendants("li")
                        //                          .Any();
                        //Console.WriteLine(checkForNodes);
                        //Console.ReadLine();

                        string completeArticle = "".ToLower();
                        foreach (var item in PageText)
                        {
                            completeArticle = completeArticle + item.InnerText;
                        }
                        // Add the li's to the completeArticle
                        var PageTextExtra = docTwo.DocumentNode
                                            .SelectNodes("//li").ToList(); 
                        foreach (var item in PageTextExtra)
                        {
                            completeArticle = completeArticle + item.InnerText;
                        }
                        
                        if (completeArticle.Contains("degree")) {
                            Console.WriteLine("Included!");
                        } else {
                            Console.WriteLine("Not Included!");
                            usefulLinks.Add(getLink);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n Here are the links to listings that will interest you: \n");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    foreach(var item in usefulLinks) {
                        Console.WriteLine(item + "\n");
                    }
                }
                // Catch error if links aren't found
                catch (Exception e)
                {
                    Console.WriteLine("ERROR" + "\n" + e);
                }
            }

        }
        // Transform the search to make sure it's a real link
        public static string TransformSearch(string searchTerm)
        {
            string newString = "";
            foreach (char letter in searchTerm) {
                if(Convert.ToString(letter) == " ") {
                    newString = newString + "%20";
                } else {
                    newString = newString + Convert.ToString(letter);
                }
            }
            return (newString);
        }
    }
}
