using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;


namespace AnalysisService
{
    class Communication
    {
        private const string default_host = "localhost";
        private const int default_port = 6379;
        private const string default_user = "admin";
        private const string default_password = "this_should_not_be_a_const";
        
        private ConnectionMultiplexer redis;
        private IDatabase database;
        public Communication(string host=default_host, int port=default_port, string user=default_user, string password=default_password)
        {
            redis = SecureConnection(host, port,user,password);
            database = redis.GetDatabase();
        }

        public List<string> GetCategory(string category)
        {
            var keys = database.SetScan(category + ":");
            List<string> jsons = new List<string>();
            foreach (var key in keys)
            {
                var json = database.StringGet(key.ToString());
                jsons.Add(json);
            }

            return jsons;
        }

        public void AddToCategory(List<string> jsons)
        {
            for (int i = 1; i <= jsons.Count; i++)
            {
                database.StringSet("idx:analytics:" + i.ToString(), jsons[i - 1]);
            }
        }

        protected ConnectionMultiplexer SecureConnection(string host, int port, string user, string password)
        {
            ConfigurationOptions options = new ConfigurationOptions
            {
                EndPoints = { { host, port } },
                User = user,  // use your Redis user. More info https://redis.io/docs/management/security/acl/
                Password = password, // use your Redis password
                Ssl = true,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12                
            };
            
            options.CertificateSelection += delegate
            {
                return new X509Certificate2("redis.pfx", "secret"); // use the password you specified for pfx file
            };
            options.CertificateValidation += ValidateServerCertificate;

            return ConnectionMultiplexer.Connect(options);
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            if (certificate == null) {
                return false;       
            }

            var ca = new X509Certificate2("redis_ca.pem");
            bool verdict = (certificate.Issuer == ca.Subject);
            if (verdict) {
                return true;
            }
            Console.WriteLine("Certificate error: {0}", sslpolicyerrors);
            return false;
        }
    }
}
