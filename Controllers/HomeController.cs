using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordDictionary.Models;
using HtmlAgilityPack;
using System.Text;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections;
using System.Text.RegularExpressions;

namespace WordDictionary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string RemoveHtmlTags(string html)
        {
            string htmlRemoved = Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>|<[^>]+>| ", " ").Trim();
            string normalised = Regex.Replace(htmlRemoved, @"\s{2,}", " ");
            return normalised;
        }

        [HttpPost]
        public ActionResult getResults(Dictionary model)
        {

       


            var wordList = new Dictionary();
            


            Debug.WriteLine(model.Url);

            var Url = model.Url;

            //string[] results = getAll(Url);

            //Debug.WriteLine(getAll(Url));

            string _allText = " ";


            try
            {
                HtmlWeb web = new HtmlWeb();

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                var htmlDoc = web.Load(Url);

                var root = htmlDoc.DocumentNode;

                var sb = new StringBuilder();
                var nodes = root.DescendantsAndSelf();

                foreach (var node in nodes)
                {
                    if (!node.HasChildNodes)
                    {
                        string text = node.InnerText;
                        if (!string.IsNullOrEmpty(text))
                            sb.AppendLine(text.Trim());
                    }
                }

                _allText = sb.ToString();

            }
            catch (Exception)
            {
            }

            _allText = System.Web.HttpUtility.HtmlDecode(_allText);


            string[] _texts = _allText.Split(' ');


            int numberOfrecords = 100;
            var results = _texts
            .GroupBy(s => s)
            .Where(g => g.Count() > 1)
            .OrderByDescending(g => g.Count())
            .Take(numberOfrecords)
            .Select(g => g.Key);


            List<string> _wordStrings = new List<string>();

            foreach (var node in results)
            {
                Debug.WriteLine(node);
                _wordStrings.Add(node);            }


            ViewBag.WordStrings = _wordStrings;


            string[] weekDays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };


            return View("Words");
        }

       
      


    }
}
