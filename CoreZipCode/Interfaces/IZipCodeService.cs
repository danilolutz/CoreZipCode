using CoreZipCode.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    public interface IZipCodeService
    {
        Task<Result<T>> GetAddressAsync<T>(string zipCode) where T : class;
        Task<Result<IList<T>>> ListAddressesAsync<T>(string state, string city, string street) where T : class;
    }
}
