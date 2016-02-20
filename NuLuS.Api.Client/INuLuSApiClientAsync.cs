using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace NuLuS.Api.Client
{
    [ContractClass(typeof(NuLuSApiClientAsyncContracts))]
    public interface INuLuSApiClientAsync
    {
        Task<LookupResponse[]> LookupAsync(string product, params string[] phoneNumbers);
    }

    [ContractClassFor(typeof(INuLuSApiClientAsync))]
    public abstract class NuLuSApiClientAsyncContracts : INuLuSApiClientAsync
    {
        public Task<LookupResponse[]> LookupAsync(string product, params string[] phoneNumbers)
        {
            Contract.Requires(product != null && phoneNumbers != null && phoneNumbers.Any());
            Contract.Ensures(Contract.Result<Task<LookupResponse[]>>() != null);

            return default(Task<LookupResponse[]>);
        }
    }
}
