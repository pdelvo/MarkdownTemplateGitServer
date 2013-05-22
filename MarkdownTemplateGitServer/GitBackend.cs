using GitSharp;

namespace MarkdownTemplateGitServer
{
    public static class GitBackend
    {
        public static Blob GetData(string path, string identifier, string repositoryPath = "Backend.git")
        {
            var repo = new Repository(repositoryPath);

            var commit = repo.Get<Commit>(identifier);
            if (commit == null) return null;
            var blob = commit.Tree[path];

            if (blob == null || !blob.IsBlob)
            {
                return null;
            }

            return new Blob(repo, blob.Hash);
        }
    }
}
