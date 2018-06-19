using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Headless.ServicePlatform.Tests.ApiIntegration.Contentful
{
    [TestFixture]
    public class ContentfulApiProxyTests : ApiProxyTestsBase
    {
        [Test]
        public async Task InboundUri_Should_Request_RESTCountriesApi()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/contentful/spaces/yadj1kx9rmg0/");
            var response = await TestServer.CreateClient().SendAsync(request);

            var responseContentAsString = await response.Content.ReadAsStringAsync();

            var responseAsAnonymousType = JsonConvert.DeserializeAnonymousType(responseContentAsString,
                new[]
                {
                    new {name = "", capital = ""}
                });

            responseAsAnonymousType.Should().NotBeNullOrEmpty();
            responseAsAnonymousType[0].name.Should().NotBeNullOrWhiteSpace();
            responseAsAnonymousType[0].capital.Should().NotBeNullOrWhiteSpace();
        }
    }
}