using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        private static string repo;
        private static string? USERNAME = Environment.GetEnvironmentVariable("username");
        private static string? TOKEN = Environment.GetEnvironmentVariable("token");
        private static int lastCreatedIssuenumber;
        private static int lastCreatedCommentId;
        
        [SetUp]
        public void Setup()
        {            
            client = new GitHubApiClient("https://api.github.com/repos/testnakov/", USERNAME, TOKEN);
            repo = Environment.GetEnvironmentVariable("repo");
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            var issues = client.GetAllIssues(repo);

            Assert.NotNull(issues, "Issues are null!");
            Assert.That(issues, Has.Count.GreaterThan(0), "There should be more than one issue");

            foreach (Issue issue in issues)
            {
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater by zero");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number should be greater by zero");
                Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty");
            }
        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            int issueNumber = 1;
            var issue = client.GetIssueByNumber(repo, issueNumber);

            Assert.NotNull(issue, "The response should contain issue data.");
            Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater by zero.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), "Issue Numbers are not equal.");
            Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty.");
        }
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            int issueNumber = 7;
            var labels = client.GetAllLabelsForIssue(repo, issueNumber);

            Assert.That(labels.Count, Is.GreaterThan(0));

            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0), "Label ID should be greater by zero.");
                Assert.That(label.Name, Is.Not.Null, "Label name should not be empty.");
                Console.WriteLine($"Label: {label.Id} - Name: {label.Name}");
            }
        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            int issueNumber = 6;
            var comments = client.GetAllCommentsForIssue(repo, issueNumber);

            Assert.That(comments.Count, Is.GreaterThan(0));
            foreach (var comment in comments)
            {
                Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be greater than zero.");
                Assert.That(comment.Body, Is.Not.Null, "Comment body should not be empty.");
                Console.WriteLine($"Comment: {comment.Id} - Body: {comment.Body}");
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            string title = "New issue title 1";
            string body = "New issues body 1";
            var issue = client.CreateIssue(repo, title, body);
            lastCreatedIssuenumber = issue.Number;
            Assert.Multiple(() => 
            {
                Assert.NotNull(issue);
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than zero.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Nubmer should be greater than zero.");
                Assert.That(issue.Title, Is.EqualTo(title), "Issue Title is not empty.");
                Assert.That(issue.Body, Is.EqualTo(body), "Issue body is not empty.");
            });

            lastCreatedIssuenumber = issue.Number;
            Console.WriteLine(lastCreatedIssuenumber);
        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            string body = "New commnet for this issue 5527";
            int issueNumber = lastCreatedIssuenumber;

            var comment = client.CreateCommentOnGitHubIssue(repo, issueNumber, body);
            Assert.That(comment.Body, Is.EqualTo(body), $"Comment Body is not the same as {body}");
            lastCreatedCommentId = comment.Id;
            Console.WriteLine(lastCreatedCommentId);
        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            int commentId = lastCreatedCommentId;

            var comment = client.GetCommentById(repo, commentId);

            Assert.NotNull(comment);
            Assert.That(comment.Id, Is.EqualTo(commentId), $"Comment Id is not {commentId}");

        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            int commentId = lastCreatedCommentId;
            string newBody = "Edited body comment";

            var comment = client.EditCommentOnGitHubIssue(repo, commentId, newBody);

            Assert.NotNull(comment);
            Assert.That(comment.Id, Is.EqualTo(commentId), $"Comment Id is not {commentId}");
            Assert.That(comment.Body, Is.EqualTo(newBody), $"Commend Body is not {newBody}");
        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            int commentId = lastCreatedCommentId;
            var result = client.DeleteCommentOnGitHubIssue(repo, commentId);

            Assert.True(result, $"Comment with id: {commentId} is not deleted");
        }


    }
}

