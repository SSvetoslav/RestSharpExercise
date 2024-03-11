# RestSharpExercise
GitHub Issues provides the standard RESTful API endpoints, which you can access with HTTP client from https://api.github.com:

· GET endpoints – respond with JSON object as result.

o GET /repos/{user}/{repo}/issues – returns the issues in given GitHub repo.

o GET /repos/{user}/{repo}/issues/{num} – returns the specified issue.

o GET /repos/{user}/{repo}/issues/{num}/comments – returns the comments for an issue.

o GET /repos/{user}/{repo}/issues/comments/{id} – returns the specified comment.

· POST / PATCH / DELETE endpoints – all of them need authentication.

o POST /repos/{user}/{repo}/issues – creates a new issue.

o PATCH /repos/{user}/{repo}/issues/{num} – modifies the specified issue.

o POST /repos/{user}/{repo}/issues/{num}/comments – creates a new comment for certain issue.

o PATCH /repos/{user}/{repo}/issues/comments/{id} – modifies existing comment.

o DELETE /repos/{user}/{repo}/issues/comments/{id} – deletes existing comment.
