using Microsoft.Azure;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Factories;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Management.Automation;
using Microsoft.Azure.Subscriptions;
using Microsoft.Azure.Subscriptions.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.SCSMAutomationConnector.AzureAutomation
{
    public static class Test
    {

        
        public static async void TestAzure()
        {

            var acc = new AzureAccount() {
               Type = AzureAccount.AccountType.User
            };

            var env = AzureEnvironment.PublicEnvironments["AzureCloud"];

            var tenant = new AzureTenant() {
                Id = Guid.Empty

            };

           var ctx = new AzureContext(acc, env, tenant);

            //var c = AzureSession.AuthenticationFactory.Authenticate(acc, env, "common", null, ShowDialog.Auto, TokenCache.DefaultShared);

            //var scred = AzureSession.AuthenticationFactory.GetServiceClientCredentials(ctx);
            

            var cred = AzureSession.AuthenticationFactory.Authenticate(acc, env, AuthenticationFactory.CommonAdTenant, null, ShowDialog.Auto, TokenCache.DefaultShared);

            var u = new Uri(AzureEnvironmentConstants.AzureResourceManagerEndpoint);

            var tenantsClient = AzureSession.ClientFactory.CreateCustomClient<SubscriptionClient>(
                new TokenCloudCredentials(cred.AccessToken), u);

            var tenants = await tenantsClient.Tenants.ListAsync();


            foreach (var t in tenants.TenantIds)
            {
                //var subCred = AzureSession.AuthenticationFactory.Authenticate(acc, env, t.TenantId, null, ShowDialog.Auto, TokenCache.DefaultShared);

                //tenantsClient.Credentials = new TokenCloudCredentials(subCred.AccessToken);



                var subsrciptions = await tenantsClient.Subscriptions.ListAsync();
            }


            


            //AzureSession.AuthenticationFactory.GetSubscriptionCloudCredentials(ctx, AzureEnvironment.Endpoint.ResourceManager);

            var cl = AzureSession.ClientFactory.CreateClient<AutomationManagementClient>(ctx, AzureEnvironment.Endpoint.ResourceManager);

            var rbs = cl.Runbooks.ListAsync("bula", "kurac");

        }

    }
}
