using CoreZipCode.Services.ZipCode.SmartyStreetsApi;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.ZipCode.SmartyStreetsApi
{
    public class SmartyStreetsTest
    {
        private const string ExpectedZipcodeResponse = "[{\"input_index\":0,\"city_states\":[{\"city\":\"Cupertino\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true},{\"city\":\"Monte Vista\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true},{\"city\":\"Permanente\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true}],\"zipcodes\":[{\"zipcode\":\"95014\",\"zipcode_type\":\"S\",\"default_city\":\"Cupertino\",\"county_fips\":\"06085\",\"county_name\":\"Santa Clara\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"latitude\":37.32098,\"longitude\":-122.03838,\"precision\":\"Zip5\"}]}]";
        private const string ExpectedParamResponse = "[{\"input_index\":0,\"candidate_index\":0,\"delivery_line_1\":\"1600 Amphitheatre Pkwy\",\"last_line\":\"Mountain View CA 94043-1351\",\"delivery_point_barcode\":\"940431351000\",\"components\":{\"primary_number\":\"1600\",\"street_name\":\"Amphitheatre\",\"street_suffix\":\"Pkwy\",\"city_name\":\"Mountain View\",\"default_city_name\":\"Mountain View\",\"state_abbreviation\":\"CA\",\"zipcode\":\"94043\",\"plus4_code\":\"1351\",\"delivery_point\":\"00\",\"delivery_point_check_digit\":\"0\"},\"metadata\":{\"record_type\":\"S\",\"zip_type\":\"Standard\",\"county_fips\":\"06085\",\"county_name\":\"Santa Clara\",\"carrier_route\":\"C909\",\"congressional_district\":\"18\",\"rdi\":\"Commercial\",\"elot_sequence\":\"0094\",\"elot_sort\":\"A\",\"latitude\":37.42357,\"longitude\":-122.08661,\"precision\":\"Zip9\",\"time_zone\":\"Pacific\",\"utc_offset\":-8,\"dst\":true},\"analysis\":{\"dpv_match_code\":\"Y\",\"dpv_footnotes\":\"AABB\",\"dpv_cmra\":\"N\",\"dpv_vacant\":\"N\",\"active\":\"N\"}}]";
        private const string ExpectedState = "CA";
        private const string ExpectedCity = "Cupertino";
        private const string ZipCodeTest = "95014";
        private const string SendAsync = "SendAsync";
        private const string MockUri = "https://unit.test.com/";
        private const string SmartyStreetsParameterState = "CA";
        private const string SmartyStreetsParameterCity = "Mountain View";
        private const string SmartyStreetsParameterStreet = "1600 Amphitheatre Pkwy";
        private const string InvalidStreetMessage = "Invalid Street, parameter over size of 64 characters.";
        private const string InvalidCityMessage = "Invalid City, parameter over size of 64 characters.";
        private const string InvalidStateMessage = "Invalid State, parameter over size of 32 characters.";
        private const string InvalidZipCodeFormatMessage = "Invalid ZipCode Format";
        private const string InvalidZipCodeSizeMessage = "Invalid ZipCode Size";
        private const string AuthToken = "some auth token";
        private const string AuthId = "some auth id";

        private readonly SmartyStreets _service;
        private readonly SmartyStreets _serviceParam;

        public SmartyStreetsTest()
        {
            _service = ConfigureService(ExpectedZipcodeResponse);
            _serviceParam = ConfigureService(ExpectedParamResponse);
        }

        private static SmartyStreets ConfigureService(string response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    SendAsync,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(MockUri)
            };

            return new SmartyStreets(httpClient, "test-auth-id", "test-auth-token");
        }

        [Fact]
        public static void ConstructorTest()
        {
            var actual = new SmartyStreets(AuthId, AuthToken);
            Assert.NotNull(actual);
        }

        [Fact]
        public static void ConstructorNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(new HttpClient(), AuthId, null));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(new HttpClient(), null, AuthToken));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(new HttpClient(), null, null));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(new HttpClient(), AuthId, string.Empty));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(new HttpClient(), string.Empty, AuthToken));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(new HttpClient(), string.Empty, string.Empty));

            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(AuthId, null));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(null, AuthToken));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(null, null));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(AuthId, string.Empty));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(string.Empty, AuthToken));
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets(string.Empty, string.Empty));
        }

        [Fact]
        public void MustGetSingleZipCodeJsonString()
        {
            var actual = _service.Execute(ZipCodeTest);

            Assert.Equal(ExpectedZipcodeResponse, actual);
        }

        [Fact]
        public void MustGetZipCodeByParamsJsonString()
        {
            var actual = _serviceParam.Execute(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.Equal(ExpectedParamResponse, actual);
        }

        [Fact]
        public void MustGetSingleZipCodeObject()
        {
            var actual = _service.GetAddress<List<SmartyStreetsModel>>(ZipCodeTest);

            Assert.IsType<List<SmartyStreetsModel>>(actual);
            Assert.Equal(ExpectedCity, actual[0].CityStates[0].City);
            Assert.Equal(ExpectedState, actual[0].CityStates[0].StateAbbreviation);
        }

        [Fact]
        public void MustGetZipCodeByParamsList()
        {
            var actual = _serviceParam.ListAddresses<SmartyStreetsParamsModel>(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.IsType<List<SmartyStreetsParamsModel>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal(SmartyStreetsParameterCity, actual[0].Components.CityName);
            Assert.Equal(SmartyStreetsParameterState, actual[0].Components.StateAbbreviation);
        }

        [Fact]
        public void MustThrowTheExceptions()
        {
            var exception = Assert.Throws<SmartyStreetsException>(() => _service.Execute(" 12345678901234567890 "));
            Assert.Equal(InvalidZipCodeSizeMessage, exception.Message);

            exception = Assert.Throws<SmartyStreetsException>(() => _service.Execute(" 12A"));
            Assert.Equal(InvalidZipCodeSizeMessage, exception.Message);

            exception = Assert.Throws<SmartyStreetsException>(() => _service.Execute(" 123A5678 "));
            Assert.Equal(InvalidZipCodeFormatMessage, exception.Message);

            exception = Assert.Throws<SmartyStreetsException>(() => _service.Execute("Lorem ipsum dolor sit amet amet sit", "Mountain View", "1600 Amphitheatre Pkwy"));
            Assert.Equal(InvalidStateMessage, exception.Message);

            exception = Assert.Throws<SmartyStreetsException>(() => _service.Execute("CA", "Lorem ipsum dolor sit amet, consectetur adipiscing elit posuere posuere.", "1600 Amphitheatre Pkwy"));
            Assert.Equal(InvalidCityMessage, exception.Message);

            exception = Assert.Throws<SmartyStreetsException>(() => _service.Execute("CA", "Mountain View", "Lorem ipsum dolor sit amet, consectetur adipiscing elit posuere posuere."));
            Assert.Equal(InvalidStreetMessage, exception.Message);
        }

        [Fact]
        public async Task MustGetSingleZipCodeJsonStringAsync()
        {
            var actual = await _service.ExecuteAsync(ZipCodeTest);

            Assert.Equal(ExpectedZipcodeResponse, actual);
        }

        [Fact]
        public async Task MustGetListZipCodeJsonStringAsync()
        {
            var actual = await _serviceParam.ExecuteAsync(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.Equal(ExpectedParamResponse, actual);
        }

        [Fact]
        public async Task MustGetSingleZipCodeObjectAsync()
        {
            var actual = await _service.GetAddressAsync<List<SmartyStreetsModel>>(ZipCodeTest);

            Assert.IsType<List<SmartyStreetsModel>>(actual);
            Assert.Equal(ExpectedCity, actual[0].CityStates[0].City);
            Assert.Equal(ExpectedState, actual[0].CityStates[0].StateAbbreviation);
        }

        [Fact]
        public async Task MustGetZipCodeByParamsListAsync()
        {
            var actual = await _serviceParam.ListAddressesAsync<SmartyStreetsParamsModel>(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.IsType<List<SmartyStreetsParamsModel>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal(SmartyStreetsParameterCity, actual[0].Components.CityName);
            Assert.Equal(SmartyStreetsParameterState, actual[0].Components.StateAbbreviation);
        }

        [Fact]
        public void MustThrowTheExceptionsAsync()
        {
            var exception = Assert.ThrowsAsync<SmartyStreetsException>(() => _service.ExecuteAsync(" 12345678901234567890 "));
            Assert.Equal(InvalidZipCodeSizeMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<SmartyStreetsException>(() => _service.ExecuteAsync(" 12A"));
            Assert.Equal(InvalidZipCodeSizeMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<SmartyStreetsException>(() => _service.ExecuteAsync(" 123A5678 "));
            Assert.Equal(InvalidZipCodeFormatMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<SmartyStreetsException>(() => _service.ExecuteAsync("Lorem ipsum dolor sit amet amet sit", "Mountain View", "1600 Amphitheatre Pkwy"));
            Assert.Equal(InvalidStateMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<SmartyStreetsException>(() => _service.ExecuteAsync("CA", "Lorem ipsum dolor sit amet, consectetur adipiscing elit posuere posuere.", "1600 Amphitheatre Pkwy"));
            Assert.Equal(InvalidCityMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<SmartyStreetsException>(() => _service.ExecuteAsync("CA", "Mountain View", "Lorem ipsum dolor sit amet, consectetur adipiscing elit posuere posuere."));
            Assert.Equal(InvalidStreetMessage, exception.Result.Message);
        }
    }
}