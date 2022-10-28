using DRMDesktopUILibrary.Api.Interfaces;
using DRMDesktopUILibrary.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public SaleEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task Post(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/Sale", sale))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
