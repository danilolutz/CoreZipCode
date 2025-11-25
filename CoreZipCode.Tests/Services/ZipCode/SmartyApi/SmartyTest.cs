using CoreZipCode.Interfaces;
using CoreZipCode.Result;
using CoreZipCode.Services.ZipCode.SmartyApi;
using CoreZipCode.Services.ZipCode.ViaCepApi;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.ZipCode.SmartyApi
{
    public class SmartyTests
    {
        private const string AuthId = "test-id";
        private const string AuthToken = "test-token";
        private const string ZipcodeResponse = "[{\"input_index\":0,\"city_states\":[{\"city\":\"Cupertino\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true},{\"city\":\"Monte Vista\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true},{\"city\":\"Permanente\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"mailable_city\":true}],\"zipcodes\":[{\"zipcode\":\"95014\",\"zipcode_type\":\"S\",\"default_city\":\"Cupertino\",\"county_fips\":\"06085\",\"county_name\":\"Santa Clara\",\"state_abbreviation\":\"CA\",\"state\":\"California\",\"latitude\":37.32098,\"longitude\":-122.03838,\"precision\":\"Zip5\"}]}]";
        private const string StreetResponse = "[{\"input_index\":0,\"candidate_index\":0,\"delivery_line_1\":\"1600 Amphitheatre Pkwy\",\"last_line\":\"Mountain View CA 94043-1351\",\"delivery_point_barcode\":\"940431351000\",\"components\":{\"primary_number\":\"1600\",\"street_name\":\"Amphitheatre\",\"street_suffix\":\"Pkwy\",\"city_name\":\"Mountain View\",\"default_city_name\":\"Mountain View\",\"state_abbreviation\":\"CA\",\"zipcode\":\"94043\",\"plus4_code\":\"1351\",\"delivery_point\":\"00\",\"delivery_point_check_digit\":\"0\"},\"metadata\":{\"record_type\":\"S\",\"zip_type\":\"Standard\",\"county_fips\":\"06085\",\"county_name\":\"Santa Clara\",\"carrier_route\":\"C909\",\"congressional_district\":\"18\",\"rdi\":\"Commercial\",\"elot_sequence\":\"0094\",\"elot_sort\":\"A\",\"latitude\":37.42357,\"longitude\":-122.08661,\"precision\":\"Zip9\",\"time_zone\":\"Pacific\",\"utc_offset\":-8,\"dst\":true},\"analysis\":{\"dpv_match_code\":\"Y\",\"dpv_footnotes\":\"AABB\",\"dpv_cmra\":\"N\",\"dpv_vacant\":\"N\",\"active\":\"N\"}}]";

        private readonly Mock<IApiHandler> _handlerMock = new();
        private readonly Smarty _service;

        public SmartyTests()
        {
            _service = new Smarty(_handlerMock.Object, AuthId, AuthToken);
        }

        [Fact]
        public void Constructor_Creates_Instance_With_HttpClient()
        {
            new Smarty(new HttpClient(), AuthId, AuthToken);
        }

        [Fact]
        public void Constructor_Requires_NonEmpty_Credentials()
        {
            Assert.Throws<ArgumentNullException>(() => new Smarty(null, AuthToken));
            Assert.Throws<ArgumentNullException>(() => new Smarty(AuthId, null));
            Assert.Throws<ArgumentNullException>(() => new Smarty("", AuthToken));
            Assert.Throws<ArgumentNullException>(() => new Smarty(AuthId, ""));
        }

        [Fact]
        public async Task GetAddressAsync_ValidZip_Returns_Success_With_ListOfSmartyModel()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync(It.Is<string>(u => u.Contains("zipcode=95014"))))
                .ReturnsAsync(Result<string>.Success(ZipcodeResponse));

            var result = await _service.GetAddressAsync<List<SmartyModel>>("95014");

            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
            Assert.Equal("Cupertino", result.Value[0].CityStates[0].City);
            Assert.Equal("CA", result.Value[0].CityStates[0].StateAbbreviation);
        }

        [Fact]
        public async Task ListAddressesAsync_ValidAddress_Returns_Success_With_StreetModel()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success(StreetResponse));

            var result = await _service.ListAddressesAsync<SmartyParamsModel>("CA", "Mountain View", "1600 Amphitheatre Pkwy");

            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
            Assert.Equal("Mountain View", result.Value[0].Components.CityName);
            Assert.Equal("CA", result.Value[0].Components.StateAbbreviation);
        }

        [Fact]
        public async Task GetAddressAsync_ApiReturns404_Returns_Failure()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Failure(new ApiError(HttpStatusCode.NotFound, "Not found")));

            var result = await _service.GetAddressAsync<List<SmartyModel>>("00000");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.StatusCode);
        }

        [Fact]
        public async Task ListAddressesAsync_InvalidState_Throws_SmartyException()
        {
            var longState = new string('A', 33);
            var ex = await Assert.ThrowsAsync<SmartyException>(
                () => _service.ListAddressesAsync<SmartyParamsModel>(longState, "City", "Street"));

            Assert.Contains("exceeds 32 characters", ex.Message);
        }

        [Fact]
        public async Task GetAddressAsync_InvalidZipFormat_Throws_SmartyException()
        {
            var ex = await Assert.ThrowsAsync<SmartyException>(
                () => _service.GetAddressAsync<List<SmartyModel>>("12345a"));

            Assert.Contains("Invalid ZipCode Format", ex.Message);
        }

        [Fact]
        public async Task GetAddressAsync_InvalidZipSize_Throws_SmartyException()
        {
            var ex = await Assert.ThrowsAsync<SmartyException>(
                () => _service.GetAddressAsync<List<SmartyModel>>("ABC"));

            Assert.Contains("Invalid ZipCode Size", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_InvalidCity_Throws_SmartyException()
        {
            var longCity = new string('A', 65);
            var ex = await Assert.ThrowsAsync<SmartyException>(
                () => _service.ListAddressesAsync<SmartyParamsModel>("CA", longCity, "Street"));

            Assert.Contains("exceeds 64 characters", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_InvalidStreet_Throws_SmartyException()
        {
            var longStreet = new string('A', 65);
            var ex = await Assert.ThrowsAsync<SmartyException>(
                () => _service.ListAddressesAsync<SmartyParamsModel>("CA", "City", longStreet));

            Assert.Contains("exceeds 64 characters", ex.Message);
        }

        [Fact]
        public void SetZipCodeUrl_Generates_Correct_Url()
        {
            var service = new Smarty(AuthId, AuthToken);
            var url = service.SetZipCodeUrl("95014");

            Assert.Contains("us-zipcode.api.smartystreets.com", url);
            Assert.Contains("auth-id=test-id", url);
            Assert.Contains("auth-token=test-token", url);
            Assert.Contains("zipcode=95014", url);
        }

        [Fact]
        public void SetZipCodeUrlBy_Generates_Correct_Url_With_Encoded_Parameters()
        {
            var service = new Smarty(AuthId, AuthToken);
            var url = service.SetZipCodeUrlBy("CA", "San José", "Main St");

            Assert.Contains("us-street.api.smartystreets.com", url);
            Assert.Contains("state=CA", url);
            Assert.Contains("city=San%20Jos%C3%A9", url);
            Assert.Contains("street=Main%20St", url);
        }
    }
}
