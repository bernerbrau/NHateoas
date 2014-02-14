﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NHateoas.Integration.Tests.Controllers;
using NHateoas.Integration.Tests.Models;
using NUnit.Framework;

namespace NHateoas.Integration.Tests
{
    [TestFixture]
    public class IntegrationTest
    {
        private HttpMessageInvoker _httpMessageInvoker;
        private HttpServer _httpServer;

        [SetUp]
        public void InMemoryHosting()
        {
            var config = new HttpConfiguration();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.MapHttpAttributeRoutes();
            
            config.InitializeHypermedia();

            _httpServer = new HttpServer(config);
            _httpMessageInvoker = new HttpMessageInvoker(new InMemorySerializationHandler(_httpServer));
        }

        [TearDown]
        public void Tearown()
        {
            _httpMessageInvoker.Dispose();
            _httpServer.Dispose();
        }

        [Test]
        public void GetProducts()
        {
            string baseAddress = "http://dummyname/";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseAddress + "api/product");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = HttpMethod.Get;

            var cts = new CancellationTokenSource();

            using (HttpResponseMessage response = _httpMessageInvoker.SendAsync(request, cts.Token).Result)
            {
                Assume.That(response.Content, Is.Not.Null);
                var result = response.Content.ReadAsAsync<IEnumerable<Object>>(cts.Token).Result;
                Assume.That(result, Is.Not.Null);
                var asText = JsonConvert.SerializeObject(result);
                Assume.That(asText, Is.EqualTo("[{\"properties\":{\"Id\":1,\"Name\":\"Item1\",\"Price\":2.99},\"links\":[{\"rel\":[\"self\",\"__query\"],\"href\":\"api/Product\"},{\"rel\":[\"get-by-id\"],\"href\":\"api/Product/1\"}],\"actions\":[{\"name\":\"get_productdetails_by_id\",\"method\":\"GET\",\"href\":\"api/Product/1/Details\",\"fields\":[{\"name\":\"id\",\"value\":\"1\"}]}]},{\"properties\":{\"Id\":2,\"Name\":\"Item2\",\"Price\":3.99},\"links\":[{\"rel\":[\"self\",\"__query\"],\"href\":\"api/Product\"},{\"rel\":[\"get-by-id\"],\"href\":\"api/Product/2\"}],\"actions\":[{\"name\":\"get_productdetails_by_id\",\"method\":\"GET\",\"href\":\"api/Product/2/Details\",\"fields\":[{\"name\":\"id\",\"value\":\"2\"}]}]},{\"properties\":{\"Id\":3,\"Name\":\"Item3\",\"Price\":4.99},\"links\":[{\"rel\":[\"self\",\"__query\"],\"href\":\"api/Product\"},{\"rel\":[\"get-by-id\"],\"href\":\"api/Product/3\"}],\"actions\":[{\"name\":\"get_productdetails_by_id\",\"method\":\"GET\",\"href\":\"api/Product/3/Details\",\"fields\":[{\"name\":\"id\",\"value\":\"3\"}]}]},{\"properties\":{\"Id\":4,\"Name\":\"Item4\",\"Price\":5.99},\"links\":[{\"rel\":[\"self\",\"__query\"],\"href\":\"api/Product\"},{\"rel\":[\"get-by-id\"],\"href\":\"api/Product/4\"}],\"actions\":[{\"name\":\"get_productdetails_by_id\",\"method\":\"GET\",\"href\":\"api/Product/4/Details\",\"fields\":[{\"name\":\"id\",\"value\":\"4\"}]}]},{\"properties\":{\"Id\":5,\"Name\":\"Item5\",\"Price\":6.99},\"links\":[{\"rel\":[\"self\",\"__query\"],\"href\":\"api/Product\"},{\"rel\":[\"get-by-id\"],\"href\":\"api/Product/5\"}],\"actions\":[{\"name\":\"get_productdetails_by_id\",\"method\":\"GET\",\"href\":\"api/Product/5/Details\",\"fields\":[{\"name\":\"id\",\"value\":\"5\"}]}]}]"));
            }
        }

        [Test]
        public void GetProductDetails()
        {
            string baseAddress = "http://dummyname/";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseAddress + "api/Product/1/Details");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = HttpMethod.Get;

            var cts = new CancellationTokenSource();

            using (HttpResponseMessage response = _httpMessageInvoker.SendAsync(request, cts.Token).Result)
            {
                Assume.That(response.Content, Is.Not.Null);
                var result = response.Content.ReadAsAsync<Object>(cts.Token).Result;
                Assume.That(result, Is.Not.Null);
                var asText = JsonConvert.SerializeObject(result);
                Assume.That(asText,
                    Is.EqualTo(
                        "{\"properties\":{\"Id\":1,\"ProductId\":1,\"Details\":\"Cup details\"},\"links\":[{\"rel\":[\"get_productdetails_by_id\"],\"href\":\"api/Product/1/Details\"}],\"actions\":[{\"name\":\"post_by_value\",\"method\":\"POST\",\"href\":\"api/ProductDetails\",\"type\":\"application/x-www-form-urlencoded\",\"fields\":[{\"name\":\"Id\",\"value\":\"1\"},{\"name\":\"ProductId\",\"value\":\"1\"},{\"name\":\"Details\",\"value\":\"Cup details\"}]},{\"name\":\"put_by_id_value\",\"method\":\"PUT\",\"href\":\"api/ProductDetails/1\",\"fields\":[{\"name\":\"Id\",\"value\":\"1\"},{\"name\":\"ProductId\",\"value\":\"1\"},{\"name\":\"Details\",\"value\":\"Cup details\"}]},{\"name\":\"delete_by_id\",\"method\":\"DELETE\",\"href\":\"api/ProductDetails/1\"}]}"));

            }
        }
        [Test]
        public void GetProductDetailsById()
        {
            string baseAddress = "http://dummyname/";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseAddress + "api/ProductDetails/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = HttpMethod.Get;

            var cts = new CancellationTokenSource();

            using (HttpResponseMessage response = _httpMessageInvoker.SendAsync(request, cts.Token).Result)
            {
                Assume.That(response.Content, Is.Not.Null);
                var result = response.Content.ReadAsAsync<Object>(cts.Token).Result;
                Assume.That(result, Is.Not.Null);
                var asText = JsonConvert.SerializeObject(result);
                Assume.That(asText,
                    Is.EqualTo(
                        "{\"Id\":1,\"ProductId\":1,\"Details\":\"Cup details\",\"get_productdetails_by_id\":\"api/Product/1/Details\",\"post_by_value\":\"api/ProductDetails\",\"put_by_id_value\":\"api/ProductDetails/1\",\"delete_by_id\":\"api/ProductDetails/1\"}"));

            }
        }
    }
}