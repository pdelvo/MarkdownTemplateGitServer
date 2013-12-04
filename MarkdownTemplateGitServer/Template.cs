using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GitSharp;

namespace MarkdownTemplateGitServer
{
    public static class Template
    {
        public static string PerformTemplate(string path, string content, Dictionary<string, string> sections, IEnumerable<KeyValuePair<string,string>> template , string repository = "Backend.git", string branch = "template")
        {
            var resultPath = Path.ChangeExtension(path, ".html");

            var data = GetTemplate(resultPath, repository, branch) ?? GetTemplate("default.html", repository, branch);

            if (data == null) return content;

            var sectionTemplate = GetTemplate(Path.GetFileNameWithoutExtension(path) + "_section.html", repository, branch) ??
                                  GetTemplate("_section.html", repository, branch);

            var sectionBuilder = new StringBuilder();

            if (sectionTemplate != null)
            {
                foreach (var section in sections)
                {
                    sectionBuilder.Append(sectionTemplate.Replace("[id]", section.Key).Replace("[name]", section.Value));
                }
            }

            var text = data
                .Replace("[content]", content)
                .Replace("[nav]", sectionBuilder.ToString());

            return template.Aggregate(text, (current, keyValuePair) => current.Replace("[" + keyValuePair.Key + "]", keyValuePair.Value));
        }

        public static string GetTemplate(string path, string repository = "Backend.git", string branch = "template")
        {
            var repo = new Repository(repository);

            var commit = repo.Get<Commit>(branch);
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
