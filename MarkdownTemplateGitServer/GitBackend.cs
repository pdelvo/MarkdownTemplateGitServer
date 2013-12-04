using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<Change> GetChanges(string path, string firstIdentifier, string secondIdentifier, string repositoryPath = "Backend.git")
        {
            var repo = new Repository(repositoryPath);

            var firstCommit = repo.Get<Commit>(firstIdentifier);
            var secondCommit = repo.Get<Commit>(secondIdentifier);
            if (firstCommit == null || secondCommit == null) return null;

            return secondCommit.CompareAgainst(firstCommit).ToArray();
        }

        public static IEnumerable<Commit> GetHistory(string path, string identifier, string repositoryPath = "Backend.git")
        {
            var repo = new Repository(repositoryPath);

            var commit = repo.Get<Commit>(identifier);
            if (commit == null) yield break;

            while (commit != null)
            {
                if (commit.Tree[path] != null)
                {
                    yield return commit;
                }
                commit = commit.HasParents ? commit.Parent : null;
            }
        }
    }
}
