using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Payconiq.Datamodel;

namespace Payconiq
{
    public class OnlineClient
    {
        public OnlineClient(Uri uri)
        {
            _uri = uri;
        }

        private readonly Uri _uri;
        private static readonly HttpClient _client = new HttpClient();

        private readonly JsonSerializerOptions _serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<PaymentResponse> CreatePaymentAsync(
            PaymentRequest request,
            string key)
        {
            var httpResponse = await _client.SendAsync(asHttpRequest(request, key));
            return await asResponseAsync(httpResponse);
        }

        private HttpRequestMessage asHttpRequest(
            PaymentRequest request,
            string key)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, PaymentRequestUri);

            //create header
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);

            //create body
            var content = JsonSerializer.Serialize(request, _serializeOptions);
            httpRequest.Content = new StringContent(content);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpRequest;
        }

        private Uri PaymentRequestUri
        {
            get
            {
                return new Uri(_uri, "v3/payments");
            }
        }

        private async Task<PaymentResponse> asResponseAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return new PaymentResponse(); // #TODO throw exception instead

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PaymentResponse>(content, _serializeOptions);
        }

        public async void GetQrAsync(Uri qrUri)
        {
            byte[] content = await _client.GetByteArrayAsync(qrUri);
        }
    }
}