using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IBlobHelper
    {

        Task<Guid> UploadBlobAsync(IFormFile file, string container);

        Task<Guid> UploadBlobAsync(byte[] file, string container);

        Task<Guid> UploadBlobAsync(string image, string container);
    }
}
