using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sinatra.ApiClient.Clients
{
    public class SimpleClient
    {
        private readonly HttpClient _httpClient;

        public SimpleClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<TResponse> Invoke<TResponse>(HttpMethod method, Uri uri, object body = default) where TResponse : class
        {
            // TODO: add authorization context, moderate _httpClient.DefaultRequestHeaders.Authorization
            HttpRequestMessage request = new HttpRequestMessage(method, uri);

            if (body != default)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body));
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            // TODO: define exception classes for different status codes
            // e.g AuthorizationException for 401Unauthorized
            // e.g ForbiddenException for 403Forbidden
            // e.g ServerErrorException for 500InternalServerError

            // TODO: invoke authorize in case of 401 and retry the action with new Authorization header,
            // and only then throw an exception

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await response.Content.ReadAsStringAsync());
                if (errorResponse != null)
                {
                    throw new ValidationException(errorResponse.Errors.Select(x => new Exceptions.ValidationError
                    {
                        Field = x.Field,
                        Message = x.Message
                    }).ToList());
                }
                else
                {
                    throw new Exception();
                }
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }

            throw new Exception();
        }
    }
}
