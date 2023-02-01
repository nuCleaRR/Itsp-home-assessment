using System.Net.Http.Headers;

namespace Api.IntegrationTests.Helpers;

public static class HttpClientHelpers
{
    // oh Lord forgive me that hardcoding 
    public const string EmailOfAthentificatedUser = "Lue_Halvorson67@hotmail.com";
    
    public static void AuthorizeAdmin(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwic3ViIjoiYWRtaW4iLCJqdGkiOiIyNzZkMTkxNSIsInJvbGUiOiJhZG1pbiIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjM1MzQ1IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNTEiLCJodHRwOi8vbG9jYWxob3N0OjUwMjUiLCJodHRwczovL2xvY2FsaG9zdDo3MTMzIl0sIm5iZiI6MTY3NTI0NDA3MCwiZXhwIjoxNzYxNjQ0MDcwLCJpYXQiOjE2NzUyNDQwNzEsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.pOP9s-QQxYuNiIAl9Tj6RTnE3mxhB3xoc771L9qFvOY");
    }
    
    public static void AuthorizeUser(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ikx1ZV9IYWx2b3Jzb242N0Bob3RtYWlsLmNvbSIsInN1YiI6Ikx1ZV9IYWx2b3Jzb242N0Bob3RtYWlsLmNvbSIsImp0aSI6Ijg1ZTUzZmRlIiwicm9sZSI6InVzZXIiLCJhdWQiOlsiaHR0cDovL2xvY2FsaG9zdDozNTM0NSIsImh0dHBzOi8vbG9jYWxob3N0OjQ0MzUxIiwiaHR0cDovL2xvY2FsaG9zdDo1MDI1IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEzMyJdLCJuYmYiOjE2NzUyNDc1MjYsImV4cCI6MTc2MTY0NzUyNiwiaWF0IjoxNjc1MjQ3NTI2LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.1qxq1_8vZxHkMgqVcLTqd6ks2gVUnzstGUo38GqbrXk");
    }
}