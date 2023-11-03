using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionApp","Aution app full access")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client{
                ClientId="postman"
            }
        };
}
