using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Headless.ServicePlatform.Tests.ApiIntegration.RESTCountries
{
    [TestFixture]
    public class RESTCountriesApiProxyTests : ApiProxyTestsBase
    {
        [Test]
        public async Task InboundUri_Should_Request_RESTCountriesApi()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/countries/all/");
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