using Xunit;
using CoreZipCode.Interfaces;
using CoreZipCode.Services;
using System.Collections.Generic;
using System;

namespace CoreZipCode.Tests.Services
{
    public class ViaCepTest
    {
        private readonly ZipCodeBaseService _service;

        public ViaCepTest()
        {
            _service = new ViaCep();
        }

        [Fact]
        public void MustGetSingleZipCodeJsonString()
        {
            string expected = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"unidade\": \"\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\"\n}";
            var actual = _service.Execute("14810-100");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MustGetListZipCodeJsonString()
        {
            string expected = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"unidade\": \"\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\"\n  }\n]";
            var actual = _service.Execute("sp", "araraquara", "barão do rio");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MustGetSingleZipCodeObject()
        {
            var actual = _service.GetAddress<ViaCepAddress>("14810-100");

            Assert.IsType<ViaCepAddress>(actual);
            Assert.Equal("Araraquara", actual.City);
            Assert.Equal("SP", actual.State);
        }

        [Fact]
        public void MustGetZipCodeObjectList()
        {
            var actual = _service.ListAddresses<ViaCepAddress>("sp", "araraquara", "barão do rio");

            Assert.IsType<List<ViaCepAddress>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal("Araraquara", actual[0].City);
            Assert.Equal("SP", actual[0].State);
        }

        [Fact]
        public void MustThrowAnException()
        {
            Assert.Throws<Exception>(() => _service.Execute("123A"));
        }

        [Fact]
        public async void MustGetSingleZipCodeJsonStringAsync()
        {
            string expected = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"unidade\": \"\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\"\n}";
            var actual = await _service.ExecuteAsync("14810-100");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void MustGetListZipCodeJsonStringAsync()
        {
            string expected = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"unidade\": \"\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\"\n  }\n]";
            var actual = await _service.ExecuteAsync("sp", "araraquara", "barão do rio");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void MustGetSingleZipCodeObjectAsync()
        {
            var actual = await _service.GetAddressAsync<ViaCepAddress>("14810-100");

            Assert.IsType<ViaCepAddress>(actual);
            Assert.Equal("Araraquara", actual.City);
            Assert.Equal("SP", actual.State);
        }

        [Fact]
        public async void MustGetZipCodeObjectListAsync()
        {
            var actual = await _service.ListAddressesAsync<ViaCepAddress>("sp", "araraquara", "barão do rio");

            Assert.IsType<List<ViaCepAddress>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal("Araraquara", actual[0].City);
            Assert.Equal("SP", actual[0].State);
        }

        [Fact]
        public void MustThrowAnExceptionAsync()
        {
            Assert.ThrowsAsync<Exception>(() => _service.ExecuteAsync("123A"));
        }
    }
}