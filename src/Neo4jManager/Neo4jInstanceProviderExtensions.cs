﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Neo4jManager
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Neo4jInstanceProviderExtensions
    {
        public static async Task<bool> IsReady(this INeo4jDeployment deployment)
        {
            var endpoint = deployment.Endpoints.HttpsEndpoint ?? deployment.Endpoints.HttpEndpoint;
            var uriBuilder = new UriBuilder(endpoint);
            if (uriBuilder.Path.EndsWith("/"))
                uriBuilder.Path += "db/data/";
            else
                uriBuilder.Path += "/db/data/";

            try
            {
                var request = WebRequest.Create(uriBuilder.Uri);
                var response = await request.GetResponseAsync();

                var httpWebResponse = (HttpWebResponse)response;
                return httpWebResponse.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException)
            {
                return false;
            }
        }

        public static async Task WaitForReady(this INeo4jDeployment deployment, CancellationToken token)
        {
            if (deployment == null) return;

            Console.WriteLine("Waiting for Neo4j...");

            while (true)
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                var ready = await deployment.IsReady();
                if (ready)
                {
                    Console.WriteLine("Neo4j is up");
                    return;
                }

                Console.WriteLine("Waiting 1 second...");
                await Task.Delay(1000, token);
            }
        }
    }
}
