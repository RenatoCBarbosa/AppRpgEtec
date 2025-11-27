using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Models;

namespace AppRpgEtec.Services.Armas
{
    public class ArmaService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "https://rpgsaramarcely.azurewebsites.net/Armas";
       
        private string _token;
        public ArmaService(string token)
        {
            _token = token;
            _request = new Request();
        }

        public async Task<ObservableCollection<Arma>> GetArmasAsync()
        {
            string urlComplementar = string.Format("{0}", "/GetAll");
            ObservableCollection<Models.Arma> listaArmas = await
                _request.GetAsync<ObservableCollection<Models.Arma>>(apiUrlBase + urlComplementar, _token);

            return listaArmas;
        }
        public async Task<Arma> GetArmaAsync(int armaId)
        {
            string urlComplementar = string.Format("/{0}", armaId);

            var arma = await
                _request.GetAsync<Models.Arma>(apiUrlBase + urlComplementar, _token);

            return arma;
        }

        public async Task<int> PostArmaAsync(Arma a)
        {
            return await _request.PostReturnIntAsync(apiUrlBase, a, _token);
        }
        public async Task<int> PutArmaAsync(Arma a)
        {
            var result = await _request.PutAsync(apiUrlBase, a, _token);
            return result;
        }
        public async Task<int> DeleteArmaAsync(int armaId)
        {
            string urlComplementar = string.Format("/{0}", armaId);
            var result = await _request.DeleteAsync(apiUrlBase + urlComplementar, _token);
            return result;
        }

    }
}
