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
        private const string ExpectedResponse = "[{\"input_index\":0,\"city_states\":[{\"city\":\"Cupertino\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true},{\"city\":\"Monte Vista\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true},{\"city\":\"Permanente\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true}],\"zipcodes\":[{\"zipcode\":\"95014\",\"zipcode_type\":\"S\",\"default_city\":\"Cupertino\",\"county_fips\":\"06085\",\"county_name\":\"Santa Clara\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"latitude\":37.32098,\"longitude\":-122.03838,\"precision\":\"Zip5\"}]}]";
        private const string ExpectedListResponse = "[{\"input_index\":0,\"candidate_index\":0,\"delivery_line_1\":\"1600 Amphitheatre Pkwy\",\"last_line\":\"Mountain View CA 94043-1351\",\"delivery_point_barcode\":\"940431351000\",\"components\":{\"primary_number\":\"1600\",\"street_name\":\"Amphitheatre\",\"street_suffix\":\"Pkwy\",\"city_name\":\"Mountain View\",\"default_city_name\":\"Mountain View\",\"state_abbreviation\":\"CA\",\"zipcode\":\"94043\",\"plus4_code\":\"1351\",\"delivery_point\":\"00\",\"delivery_point_check_digit\":\"0\"},\"metadata\":{\"record_type\":\"S\",\"zip_type\":\"Standard\",\"county_fips\":\"06085\",\"county_name\":\"Santa Clara\",\"carrier_route\":\"C909\",\"congressional_district\":\"18\",\"rdi\":\"Commercial\",\"elot_sequence\":\"0094\",\"elot_sort\":\"A\",\"latitude\":37.42357,\"longitude\":-122.08661,\"precision\":\"Zip9\",\"time_zone\":\"Pacific\",\"utc_offset\":-8,\"dst\":true},\"analysis\":{\"dpv_match_code\":\"Y\",\"dpv_footnotes\":\"AABB\",\"dpv_cmra\":\"N\",\"dpv_vacant\":\"N\",\"active\":\"N\"}}]";
        private const string ExpectedState = "CA";
        private const string ExpectedCity = "Cupertino";
        private const string ZipCodeTest = "95014";
        private const string SendAsync = "SendAsync";
        private const string MockUri = "https://unit.test.com/";
        private const string SmartyStreetsParameterState = "CA";
        private const string SmartyStreetsParameterCity = "Mountain View";
        private const string SmartyStreetsParameterStreet = "1600 Amphitheatre Pkwy";
        private const string InvalidStreetMessage = "Invalid Street Param";
        private const string InvalidCityMessage = "Invalid City Param";
        private const string InvalidStateMessage = "Invalid State Param";
        private const string InvalidZipCodeFormatMessage = "Invalid ZipCode Format";
        private const string InvalidZipCodeSizeMessage = "Invalid ZipCode Size";

        private readonly SmartyStreets _service;
        private readonly SmartyStreets _serviceList;
        private Mock<HttpMessageHandler> _handlerMock;

        public SmartyStreetsTest()
        {
            _service = ConfigureService(ExpectedResponse);
            _serviceList = ConfigureService(ExpectedListResponse);
        }

        private SmartyStreets ConfigureService(string response)
        {
            _handlerMock = new Mock<HttpMessageHandler>();

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    SendAsync,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response),
                })
                .Verifiable();

            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(MockUri)
            };

            return new SmartyStreets(httpClient, "test-auth-id", "test-auth-token");
        }

        [Fact]
        public void Constructor_null_test()
        {
            Assert.Throws<ArgumentNullException>(() => new SmartyStreets("", null));
        }

        [Fact]
        public void MustGetSingleZipCodeJsonString()
        {
            var actual = _service.Execute(ZipCodeTest);

            Assert.Equal(ExpectedResponse, actual);
        }

        [Fact]
        public void MustGetZipCodeByParamsJsonString()
        {
            var actual = _serviceList.Execute(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.Equal(ExpectedListResponse, actual);
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
            var actual = _serviceList.ListAddresses<SmartyStreetsParamsModel>(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.IsType<List<SmartyStreetsParamsModel>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal(SmartyStreetsParameterCity, actual[0].Components.CityName);
            Assert.Equal(SmartyStreetsParameterState, actual[0].Components.StateAbbreviation);
        }

        // [Fact]
        // public void MustThrowTheExceptions()
        // {
        //     var exception = Assert.Throws<ViaCepException>(() => _service.Execute(" 12345-67 "));
        //     Assert.Equal(InvalidZipCodeSizeMessage, exception.Message);

        //     exception = Assert.Throws<ViaCepException>(() => _service.Execute(" 123A5-678 "));
        //     Assert.Equal(InvalidZipCodeFormatMessage, exception.Message);

        //     exception = Assert.Throws<ViaCepException>(() => _service.Execute("U", "Araraquara", "barão do rio"));
        //     Assert.Equal(InvalidStateMessage, exception.Message);

        //     exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ar", "barão do rio"));
        //     Assert.Equal(InvalidCityMessage, exception.Message);

        //     exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ara", "ba"));
        //     Assert.Equal(InvalidStreetMessage, exception.Message);

        //     exception = Assert.Throws<ViaCepException>(() => _service.Execute("", "Araraquara", "barão do rio"));
        //     Assert.Equal(InvalidStateMessage, exception.Message);

        //     exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "", "barão do rio"));
        //     Assert.Equal(InvalidCityMessage, exception.Message);

        //     exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ara", ""));
        //     Assert.Equal(InvalidStreetMessage, exception.Message);
        // }

        [Fact]
        public async void MustGetSingleZipCodeJsonStringAsync()
        {
            var actual = await _service.ExecuteAsync(ZipCodeTest);

            Assert.Equal(ExpectedResponse, actual);
        }

        [Fact]
        public async void MustGetListZipCodeJsonStringAsync()
        {
            var actual = await _serviceList.ExecuteAsync(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.Equal(ExpectedListResponse, actual);
        }

        [Fact]
        public async void MustGetSingleZipCodeObjectAsync()
        {
            var actual = await _service.GetAddressAsync<List<SmartyStreetsModel>>(ZipCodeTest);

            Assert.IsType<List<SmartyStreetsModel>>(actual);
            Assert.Equal(ExpectedCity, actual[0].CityStates[0].City);
            Assert.Equal(ExpectedState, actual[0].CityStates[0].StateAbbreviation);
        }

        [Fact]
        public async void MustGetZipCodeByParamsListAsync()
        {
            var actual = await _serviceList.ListAddressesAsync<SmartyStreetsParamsModel>(SmartyStreetsParameterState, SmartyStreetsParameterCity, SmartyStreetsParameterStreet);

            Assert.IsType<List<SmartyStreetsParamsModel>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal(SmartyStreetsParameterCity, actual[0].Components.CityName);
            Assert.Equal(SmartyStreetsParameterState, actual[0].Components.StateAbbreviation);
        }

        // [Fact]
        // public void MustThrowTheExceptionsAsync()
        // {
        //     var exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 12345-67 "));
        //     Assert.Equal(InvalidZipCodeSizeMessage, exception.Result.Message);

        //     exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 123A5-678 "));
        //     Assert.Equal(InvalidZipCodeFormatMessage, exception.Result.Message);

        //     exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("U", "Araraquara", "barão do rio"));
        //     Assert.Equal(InvalidStateMessage, exception.Result.Message);

        //     exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ar", "barão do rio"));
        //     Assert.Equal(InvalidCityMessage, exception.Result.Message);

        //     exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", "ba"));
        //     Assert.Equal(InvalidStreetMessage, exception.Result.Message);

        //     exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("", "Araraquara", "barão do rio"));
        //     Assert.Equal(InvalidStateMessage, exception.Result.Message);

        //     exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "", "barão do rio"));
        //     Assert.Equal(InvalidCityMessage, exception.Result.Message);

        //     exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", ""));
        //     Assert.Equal(InvalidStreetMessage, exception.Result.Message);
        // }
    }
}