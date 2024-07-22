using htmlPracticod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;

 static void addAtributes(string line,HtmlElement current)
{
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var at in attributes)
    {
        string s = at.ToString();
        if (s.IndexOf("id") != -1)
        {
            s = s.Substring(s.IndexOf("=") + 2);
            current.Id = s.Substring(0, s.Length - 1);
        }
        else if (s.IndexOf("class") != -1)
        {
            s = s.Substring(s.IndexOf("=") + 2);
            s = s.Substring(0, s.Length - 1);
            var classes = new Regex(" ").Split(s);
            foreach (var c in classes)
            {
                current.Classes.Add(c);
            }
        }
        else
        {
            current.Attributes.Add(s);
        }

    }
}
static HtmlElement Serialize(string [] htmlLines)
{
   string[] htmlVoidTags = HtmlHelper.Instance.HtmlVoidTags;
   string[] htmlTags = HtmlHelper.Instance.HtmlTags;
    string name;
    HtmlElement parent = new HtmlElement();
    HtmlElement current = new HtmlElement();
    foreach (var line in htmlLines)
    {
        if (line.StartsWith("/html"))
        {
            return current;
        }
        if (line.IndexOf(" ") != -1 && line.IndexOf(" ") != 0)
        {
            name = line.Substring(0, line.IndexOf(" "));
        }
        else
        {
            name = line;
        }
        if (line.StartsWith("/"+current.Name))
        {

            current = current.Parents[0];
            continue;

        }
       
       
        if (htmlVoidTags.Contains(name) )
        {
            parent = current;
            current = new HtmlElement();
            current.Name = name;
            parent.Children.Add(current);
            current.Parents.Add(parent);
            addAtributes(line, current);
            current = current.Parents[0];
        }
        else if (htmlTags.Contains(name))
        {
            parent = current;
            current = new HtmlElement();
            current.Name = name;
            parent.Children.Add(current);
            current.Parents.Add(parent);
            addAtributes(line, current);
        }
        else
        {
            current.InnerHtml += line;
        }

    }

    return current;
   
}


var html1 = await Load("https://moodle.malkabruk.co.il/");
//var html1 = await Load2("seed/calander.html");
var cleanHtml = new Regex("\\s").Replace(html1, " ");
var htmlLines=new Regex("<(.*?)>").Split(cleanHtml).Where(s => !string.IsNullOrWhiteSpace(s));

HtmlElement current = Serialize(htmlLines.ToArray());


Selector s= Selector.TidySelector("li.nav-item");
//("div .drag-container")
//("li.nav-item")
List<HtmlElement> listElements = new List<HtmlElement>();
List<HtmlElement> list = HtmlElement.FindElements(current, s, listElements);
var greatList=new HashSet<HtmlElement>(list);
foreach (var element in greatList)
{
    Console.WriteLine(element.ToString());
}

async Task<string> Load(string url)
{

    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
        
    