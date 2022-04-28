using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    /// <summary>
    /// example of using SecretClient to retrieve secret in vault
    /// To enable injecting SecretClient into KeyVaultController add the following to program.cs
    /// services.AddAzureClients(builder => builder.AddSecretClient(new Uri(vaultUri)));
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KeyVaultController : ControllerBase
    {
        private readonly SecretClient secretClient;
        public KeyVaultController(SecretClient secretClient) => this.secretClient = secretClient;


        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                KeyVaultSecret keyValueSecret = await secretClient.GetSecretAsync("ConnectionStrings--AzureConnection");
                return Ok(keyValueSecret.Value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
