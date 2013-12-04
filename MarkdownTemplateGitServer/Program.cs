using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using GitSharp;
using MarkdownDeep;

namespace MarkdownTemplateGitServer
{
    class Program
    {
        static void Main(string[] args)
        {

            var server = new WebServer(args[0]);

            server.Open();

            Console.Read();
        }
    }
}
