using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace ProyectoNetCore.Helpers
{
    public class HelperPathProvider
    {
        private IServer server;

        public HelperPathProvider(IServer server)
        {
            this.server = server;
        }

        public string MapUrlServerPath()
        {
            var adresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = adresses.FirstOrDefault();
            return serverUrl;
        }
    }
}
