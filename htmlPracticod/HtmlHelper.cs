using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace htmlPracticod
{
    internal class HtmlHelper
    {
      private static string jsonTags = File.ReadAllText("seed/HtmlTags.json");
      private static string jsonVoidTags = File.ReadAllText("seed/HtmlVoidTags.json");

      private readonly static HtmlHelper _instance = new HtmlHelper(jsonTags, jsonVoidTags);
      public static HtmlHelper Instance => _instance;
      public string[] HtmlTags { get;  }
      public string[] HtmlVoidTags { get;}
        private HtmlHelper(string htmlTags, string htmlVoidTags)
        {                
            HtmlTags = JsonSerializer.Deserialize<string[]>(jsonTags);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(jsonVoidTags);     
        }
    }
}
