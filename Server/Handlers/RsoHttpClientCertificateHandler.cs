using Infrastructure.AppSettings;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Server.Handlers
{
    public class RsoHttpClientCertificateHandler : HttpClientHandler
    {
        public RsoHttpClientCertificateHandler(IWebHostEnvironment environment)
        {
            var location = Path.Combine(environment.ContentRootPath, AppSettingsProvider.RsoIntegration.CertPath);
            var content = File.ReadAllBytes(location);
            var clientCertificate = new X509Certificate2(content, AppSettingsProvider.RsoIntegration.CertPass,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            ClientCertificates.Add(clientCertificate);
        }
    }
}
