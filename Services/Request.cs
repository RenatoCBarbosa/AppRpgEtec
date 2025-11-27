using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppRpgEtec.Services
{
    public class Request
    {
        // envia uma requisição POST com um objeto e retorna um inteiro como resposta
        public async Task<int> PostReturnIntAsync<TResult>(string uri, TResult data, string token)
        {
            HttpClient httpClient = new HttpClient();

            // adiciona o token de autenticação no header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // serializa o objeto para JSON e define o tipo de conteúdo
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // envia a requisição POST
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            // lê o conteúdo da resposta como string
            string serialized = await response.Content.ReadAsStringAsync();

            // se a resposta for OK, converte a string para inteiro
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return int.Parse(serialized);
            else
                // ee não for OK, lança exceção com o conteúdo da resposta
                throw new Exception(serialized);
        }

        // envia uma requisição POST com um objeto e retorna um objeto desserializado como resposta
        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token)
        {
            HttpClient httpClient = new HttpClient();

            // adiciona o token de autenticação no header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // serializa o objeto para JSON e define o tipo de conteúdo
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // envia a requisição POST
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            // lê o conteúdo da resposta como string
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = data;

            // se a resposta for OK, desserializa o conteúdo para o tipo TResult
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized));
            else
                // se não for OK, lança exceção com o conteúdo da resposta
                throw new Exception(serialized);

            return result;
        }

        //metodo Put
        public async Task<int> PutAsync<TResult>(string uri, TResult data, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return int.Parse(serialized);
            else
                throw new Exception(serialized);
        }

        //metodo Get
        public async Task<TResult> GetAsync<TResult>(string uri, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            string serialized = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(serialized);

            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized));
            return result;
        }

        //metodo Delete 
        public async Task<int> DeleteAsync(string uri, string token)
{
HttpClient httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
token);
HttpResponseMessage response = await httpClient.DeleteAsync(uri);
string serialized = await response.Content.ReadAsStringAsync();
if (response.StatusCode == System.Net.HttpStatusCode.OK)
return int.Parse(serialized);
else
throw new Exception(serialized);
}
    }

}

