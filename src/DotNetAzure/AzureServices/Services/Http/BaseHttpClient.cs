using System.Net.Http.Json;

namespace AzureServices.Services.Http
{
    public abstract class BaseHttpClient
    {
        protected readonly HttpClient _client;
        public BaseHttpClient(HttpClient httpclient)
        {
            _client = httpclient;
        }

        /// <summary>
        /// CreateAuthHeaderAsync 
        /// </summary>
        /// <returns></returns>
        internal abstract Task CreateAuthHeaderAsync();

        internal async Task<BaseHttpResult<HttpResponseMessage>> GetAsync(string url)
        {
            await CreateAuthHeaderAsync();
            try
            {
                var result = await _client.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    return new BaseHttpSuccessResult<HttpResponseMessage>(new(), result);
                }
                else
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"StatusCode: {result.StatusCode}", $"Content: {result.Content}", $"ReasonPhrase: {result.ReasonPhrase}" }, result.StatusCode);
                }

            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }
        internal async Task<BaseHttpResult<HttpResponseMessage>> PostAsync(string url, HttpContent content)
        {
            await CreateAuthHeaderAsync();
            try
            {
                var result = await _client.PostAsync(url, content);
                if (result.IsSuccessStatusCode)
                {
                    return new BaseHttpSuccessResult<HttpResponseMessage>(new(), result);
                }
                else
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"StatusCode: {result.StatusCode}", $"Content: {result.Content}", $"ReasonPhrase: {result.ReasonPhrase}" }, result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }
        internal async Task<BaseHttpResult<HttpResponseMessage>> PutAsync(string url, HttpContent content)
        {
            await CreateAuthHeaderAsync();
            try
            {
                var result = await _client.PutAsync(url, content);
                if (result.IsSuccessStatusCode)
                {
                    return new BaseHttpSuccessResult<HttpResponseMessage>(new(), result);
                }
                else
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"StatusCode: {result.StatusCode}", $"Content: {result.Content}", $"ReasonPhrase: {result.ReasonPhrase}" }, result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }
        internal async Task<BaseHttpResult<HttpResponseMessage>> DeleteAsync(string url)
        {
            await CreateAuthHeaderAsync();
            try
            {
                var result = await _client.DeleteAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    return new BaseHttpSuccessResult<HttpResponseMessage>(new(), result);
                }
                else
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"StatusCode: {result.StatusCode}", $"Content: {result.Content}", $"ReasonPhrase: {result.ReasonPhrase}" }, result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }
        internal async Task<BaseHttpResult<HttpResponseMessage>> PostAsJsonAsync<T>(string url, T content)
        {
            await CreateAuthHeaderAsync();
            try
            {
                var result = await _client.PostAsJsonAsync(url, content);
                if (result.IsSuccessStatusCode)
                {
                    return new BaseHttpSuccessResult<HttpResponseMessage>(new(), result);
                }
                else
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"StatusCode: {result.StatusCode}", $"Content: {result.Content}", $"ReasonPhrase: {result.ReasonPhrase}" }, result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        internal async Task<BaseHttpResult<HttpResponseMessage>> PutAsJsonAsync<T>(string url, T content)
        {
            await CreateAuthHeaderAsync();
            try
            {
                var result = await _client.PutAsJsonAsync(url, content);
                if (result.IsSuccessStatusCode)
                {
                    return new BaseHttpSuccessResult<HttpResponseMessage>(new(), result);
                }
                else
                {
                    return new BaseHttpErrorResult<HttpResponseMessage>(new() { $"StatusCode: {result.StatusCode}", $"Content: {result.Content}", $"ReasonPhrase: {result.ReasonPhrase}" }, result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return new BaseHttpErrorResult<HttpResponseMessage>(new() { ex.Message }, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
