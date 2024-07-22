using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace htmlPracticod
{
    internal class Selector
    {
        public string Id  { get; set; }
        public string TagName { get; set; }
        public List<string> Class { get; set; }  = new List<string>();
        public Selector Father { get; set; }
        public Selector Child { get; set; }

        public static void AddClass(Selector s, string clas)
        {       
            while (clas.Length > 0)
            {
                string temp = clas;
                if (clas.IndexOf(".") != -1)
                {
                    s.Class.Add(clas.Substring(0, clas.IndexOf(".")));
                    temp = temp.Substring(temp.IndexOf(".") + 1);
                    clas = temp;
                }
                else
                {
                    s.Class.Add(clas);
                    return;
                }
            }
        }
        public static void HelpTree(string line,Selector s)
        {
            string[] htmlTags = HtmlHelper.Instance.HtmlTags;
            string name, id;
            if (line.IndexOf("#") != -1)
            {
                name = line.Substring(0, line.IndexOf("#"));
                id = line.Substring(line.IndexOf("#") + 1);
                s.Id = id;
                if (id.IndexOf(".") != -1)
                {
                    id = id.Substring(0, id.IndexOf("."));
                    s.Id = id;
                    AddClass(s, line.Substring(line.IndexOf(".") + 1));
                }
            }
            else if (line.IndexOf(".") != -1)
            {
                name = line.Substring(0, line.IndexOf("."));
                AddClass(s, line.Substring(line.IndexOf(".") + 1));
            }
            else name = line;
                    if (htmlTags.Contains(name) && name != "")
                    {
                        s.TagName = name;
                    }    
        }
       public static Selector TidySelector(string query )
        {   
            Selector p = new Selector();
            var selector = new Regex(" ").Split(query);
            if(selector.Length > 1)
            {
                for(int i = 0; i < selector.Length-1; i++)
                {
                    if (i == 0)
                    {
                        HelpTree(selector[i], p);
                    }
                    Selector s = new Selector();
                    p.Child = s;
                    s.Father = p;
                    HelpTree(selector[i+1], s);
                    p = s;                
                }
            }
            else
            {
                HelpTree(query, p);
            }
            while(p.Father != null)
            {
                p = p.Father;
            }
            return p;      
       }
    }
}
