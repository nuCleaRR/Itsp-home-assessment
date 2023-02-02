# ITSP Home Assessment API exercise

<details>
<summary><b>Exercise description</b></summary>

* Please use C# and .NET framework
* We would prefer if your solution would be shared as a Github repository, with a
README.md file. README should have everything we should know: how to run the
solution, maybe motivation behind your choices, or anything you would like to share with
us about the solution. If you don’t feel comfortable with sharing a repo with us, a zip file
will do.
* Now, the task, finally! :) Everybody likes movies, right? To make things more interesting,
we decided to give you a task of building an API for a movie collection. Few technical
details about your movie collection app:
* It should be a REST API
*  When user authenticates, JWT token should be returned
*  Some logging and role based access would be nice
*  Every collection needs a search feature, right?
*  Every user should be able to see collections of other people, but to be able to
change only their own collection.
*  API endpoints should have unit tests
*  Bonus task (optional)
*  Besides having an API endpoint, let’s provide a user interface for people who
don’t know how to use Postman :)
*  Impress us with your design taste (but don’t worry too much, we have designers)
*  Bonus task 2 (optional)
*  Add a Docker-File if you build a Monolith and a Docker-Composition if you build a
distributed Solution

</details>

Hi ITSP! Let me briefly explain what I did and why.

The main challenge for me was the fact that I switched from Windows to macOS recently and didn't get use for the new environment. My original intention was to did everything using VS Code, but I gave up in the middle and installed Rider. By the way, that's the main reason why I did not choose .NET Framework as you wrote in the exercise.

In addition I decided that's it's too boring to pick up some well known versions of .NET, so I pick the latest .NET 7 and MinimalAPI approach.

# What I did
* .NET 7 + MinimapApi + in memory database
* Integration tests are running on real API
* Swagger is up and running
* Simple authentication/authorization using Bearer token
* Any single cpu cycle of ChatGPT was not harm :)

## How to run
* Application requires Git and [.NET SDK 7](https://dotnet.microsoft.com/en-us/download):
```bash
git clone https://github.com/nuCleaRR/Itsp-home-assessment.git
cd Itsp-home-assessment/Api
dotnet run
open "http://localhost:5025/swagger/index.html"
```
* To run tests
```bash
cd ../Api.IntegrationTests
dotnet test
```

### How to authorize
All endpoints require authorization. There are two roles: admin and user. Admin has an access to all endpoints, user is forbidden to call `get-all` endpoints;

For authorization a new `dotnet user-jwts` command line tool should be used.

For instance:
```bash
dotnet user-jwts create --role "admin" --name "admin"`
```
```bash
dotnet user-jwts create --role "user" --name "Lue_Halvorson67@hotmail.com"
```

There are two bearers created in advance with very long expiration time: 
<details>
<summary><b>Admin</b></summary>
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwic3ViIjoiYWRtaW4iLCJqdGkiOiIyNzZkMTkxNSIsInJvbGUiOiJhZG1pbiIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjM1MzQ1IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNTEiLCJodHRwOi8vbG9jYWxob3N0OjUwMjUiLCJodHRwczovL2xvY2FsaG9zdDo3MTMzIl0sIm5iZiI6MTY3NTI0NDA3MCwiZXhwIjoxNzYxNjQ0MDcwLCJpYXQiOjE2NzUyNDQwNzEsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.pOP9s-QQxYuNiIAl9Tj6RTnE3mxhB3xoc771L9qFvOY
</details>

<details>
<summary><b>User Lue_Halvorson67@hotmail.com with id 29daecd2-986e-d2ae-09e4-4f50b6bf3fcb</b></summary>
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ikx1ZV9IYWx2b3Jzb242N0Bob3RtYWlsLmNvbSIsInN1YiI6Ikx1ZV9IYWx2b3Jzb242N0Bob3RtYWlsLmNvbSIsImp0aSI6Ijg1ZTUzZmRlIiwicm9sZSI6InVzZXIiLCJhdWQiOlsiaHR0cDovL2xvY2FsaG9zdDozNTM0NSIsImh0dHBzOi8vbG9jYWxob3N0OjQ0MzUxIiwiaHR0cDovL2xvY2FsaG9zdDo1MDI1IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEzMyJdLCJuYmYiOjE2NzUyNDc1MjYsImV4cCI6MTc2MTY0NzUyNiwiaWF0IjoxNjc1MjQ3NTI2LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.1qxq1_8vZxHkMgqVcLTqd6ks2gVUnzstGUo38GqbrXk
</details>

To authenticate in Swagger click "Authorize" button in the top right corner, then type "Bearer" + whitespace and paste token.

To authorize using curl:
```bash
curl -i -H "Authorization: Bearer {token}" http://localhost:5025/users/get-all
```

### What I didn't like much aka. points to improve
* Folder structure can be better. I didn't want apply some existing patterns and tried to follow KISS principle, specially having MinimalAPI approach. But still habits forced me to create some familiar layers.
* No CRUD operations on controllers, just omit them for simplicity sake.
* Authorization is very basic. Obviously, hard coding bearer tokens in tests is a bad approach and need to establish automatic token retrieval using user-jwts or any other approach.
* Tests far away from comprehensive coverage and can be further improved by collapsing similar tests with Theory + MemberData approach.
* Versioning is must have, but there are a lot of dancing with Swagger, so I skipped that
* Lack of completed bonus tasks.
