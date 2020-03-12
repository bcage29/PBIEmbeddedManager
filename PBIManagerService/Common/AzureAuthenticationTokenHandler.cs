using PBIManagerService.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace PBIManagerService.Common
{
    public class AzureAuthenticationTokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public AzureAuthenticationTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // request the access token
            var accessToken = await _tokenService.GetAzureAccessTokenAsync();

            // set the bearer token to the outgoing request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
