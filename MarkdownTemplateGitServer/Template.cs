using System.Collections.Generic;
using System.IO;
using System.Text;
using GitSharp;

namespace MarkdownTemplateGitServer
{
    public static class Template
    {
        public static string PerformTemplate(string path, string content, Dictionary<string, string> sections)
        {
            var resultPath = Path.ChangeExtension(path, ".html");

            var data = GetTemplate(resultPath) ?? GetTemplate("default.html");

            if (data == null) return content;

            var sectionTemplate = GetTemplate(Path.GetFileNameWithoutExtension(path) + "_section.html") ??
                                  GetTemplate("_section.html");

            var sectionBuilder = new StringBuilder();

            if (sectionTemplate != null)
            {
                foreach (var section in sections)
                {
                    sectionBuilder.Append(sectionTemplate.Replace("[id]", section.Key).Replace("[name]", section.Value));
                }
            }

            return data
                .Replace("[content]", content)
                .Replace("[nav]", sectionBuilder.ToString())
                .Replace("[title]", Path.GetFileNameWithoutExtension(path));
        }

        private static string GetTemplate(string path)
        {
            var repo = new Repository("Backend.git");

            var commit = repo.Get<Commit>("template");
            if (commit == null) return null;
            var blob = commit.Tree[path];

            if (blob == null || !blob.IsBlob)
            {
                return null;
            }

            return new Blob(repo, blob.Hash).Data;
        }
    }
}
