using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace WordDictionary.Models
{
    public class Dictionary
    {
        [Required]
        [Url]
        [Display(Name = "Enter Url")]
        public string Url { get; set; }
       
    }

    public class Dictionaries
    {
        public List<Words> wordsCounts { get; set; }
    }

    public class Words
    {
        public string wordList { get; set; }
    }

}
