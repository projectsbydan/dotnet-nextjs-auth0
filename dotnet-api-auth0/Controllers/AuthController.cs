using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Paging;
using dotnet_api_auth0.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{

    public IOptions<Auth0Configuration> _auth0Configuration;

    public AuthController(IOptions<Auth0Configuration> auth0Configuration)
    {
        _auth0Configuration = auth0Configuration;
    }

    [Authorize]
    [HttpGet]
    public string GetData()
    {
        return "This is the secret data 🕵️‍♂️";
    }

    [Authorize]
    [HttpGet]
    public async Task<IPagedList<Auth0.ManagementApi.Models.User>> GetUsers()
    {
        var authClient = new AuthenticationApiClient(_auth0Configuration.Value.Domain);
        var authToken = await authClient.GetTokenAsync(new ClientCredentialsTokenRequest
        {
            Audience = $"https://{_auth0Configuration.Value.Domain}/api/v2/",
            ClientId = _auth0Configuration.Value.ClientId,
            ClientSecret = _auth0Configuration.Value.ClientSecret
        });

        var managementClient = new ManagementApiClient(authToken.AccessToken, _auth0Configuration.Value.Domain);
        var data = await managementClient.Users.GetAllAsync(new Auth0.ManagementApi.Models.GetUsersRequest(), new PaginationInfo(0, 10, true));

        return data;
    }
}