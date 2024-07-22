using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace htmlPracticod
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }=new List<string>();
        public List<string> Classes { get; set; }=new List<string>();
        public string InnerHtml { get   ; set; } = "";
        public List<HtmlElement> Parents { get; set; }=new List<HtmlElement>();
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

       static IEnumerable<HtmlElement> Descendants(HtmlElement element)
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(element);
            while (queue.Count > 0)
            {
                HtmlElement child = queue.Dequeue();
                if (child.Children.Count > 0)
                {
                    for (int j = 0; j < child.Children.Count; j++)
                    {
                        queue.Enqueue(child.Children[j]);
                    }
                }
                yield return child;
            }

        }
        IEnumerable<HtmlElement> Ancestors(HtmlElement element)
        {
            while (element.Parents.Count > 0)
            {
                element = element.Parents[0];
                yield return element;

            }
        }
        public static bool HelpFindElements(Selector selector,HtmlElement child)
        {
            string tagName = selector.TagName;
            string id = selector.Id;
            var clas = selector.Class;
            bool flag=true;
           foreach(var c in clas)
            {
            if ( !child.Classes.Contains(c))
                {
                    flag = false;
                }
                
            }
            if (clas == null ||flag)

                if (id == null || child.Id == id)
                if (tagName == null || child.Name == tagName)
                    {
                        return true;
                    }
           return false;
        }
       public static List<HtmlElement> FindElements(HtmlElement element, Selector selector, List<HtmlElement> listFounds)
        {
            List<HtmlElement> result = new List<HtmlElement>();
            var listdesce = Descendants(element);
            listFounds = new List<HtmlElement>(listdesce);
            if (selector.Child == null)
            {
                List<HtmlElement> list = new List<HtmlElement>();
                foreach (HtmlElement child in listFounds)
                {
                    bool found = HelpFindElements(selector, child);
                    if (found)
                    {
                        list.Add(child);
                    }
                }
                return list;
            }
           
            foreach (HtmlElement child in listFounds)
            {
              bool found = HelpFindElements(selector,child);
                if (found)
                {
                    listdesce = Descendants(child);
                    listFounds = new List<HtmlElement>(listdesce);
                    listFounds= FindElements(child, selector.Child, listFounds);
                    foreach(var child2 in listFounds)
                    {
                        result.Add(child2);
                    }
                }
            }
            return result;

        }

        public override string? ToString()
        {
            if (Classes.Count > 0)
            {
                string toString = $"<{Name} id= \"{Id}\"  class: \"";

                foreach (var c in Classes)
                {
                    toString += c.ToString() + " ";
                }
                toString = toString.Substring(0,toString.Length - 1);
                toString += "\">";
                return toString;
            }
            return "<" + Name + " id= " + Id + " class: " + ">";

        }
    }
}
