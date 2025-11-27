using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class ImagemUsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;
        private static string conexaoAzureStorage = "DefaultEndpointsProtocol=https;AccountName=etecstoregerenato;AccountKey=V+eShFWe1ynAifKR9aoiY+O/eRAxzht4ojCycOp5HgUTu0RgMZdO2vVdXBVOk8l6ywiRzhjHnQHm+AStpGAIbQ==;EndpointSuffix=core.windows.net";
        private static string container = "arquivos";

        public ImagemUsuarioViewModel()
        {
            string token = Preferences.Get("UsuariToken", string.Empty);
            uService = new UsuarioService(token);

            FotografarCommand = new Command(Fotografar);
            SalvarImagemCommand = new Command(SalvarImagemAzure);
            AbrirImagemCommand = new Command(AbrirGaleria);
        }

        public ICommand FotografarCommand { get; }
        public ICommand SalvarImagemCommand { get; }
        public ICommand AbrirImagemCommand { get; }

        private ImageSource fonteImagem;

        public ImageSource FonteImagem
        {
            get { return fonteImagem; }
            set
            {
                fonteImagem = value;
                OnPropertyChanged();
            }
        }

        private byte[] foto;

        public byte[] Foto
        {
            get => foto;
            set
            {
                foto = value;
                OnPropertyChanged();
            }
        }

        public async void AbrirGaleria()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                    if (photo != null)
                    {
                        using (Stream sourceStream = await photo.OpenReadAsync())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                await sourceStream.CopyToAsync(ms);

                                Foto = ms.ToArray();

                                FonteImagem = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Detalhes: " + ex.InnerException, "OK");
            }
        }

        public async void Fotografar()
        {
            try
            {
                //Verificação se o dispositivo suporta mídia como câmera e galeria.
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    //Chamada para a câmera do dispositivo. Fica aguardando usuário tirar foto.
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        using (Stream sourceStream = await photo.OpenReadAsync())//Leitura dos bytes da foto para Stream
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                await sourceStream.CopyToAsync(ms);//Conversão do Stream para MemoryStream (arquivo em memória)

                                //Carregamento do array de bytes a partir da memória para a propriedade da ViewModel
                                Foto = ms.ToArray();

                                //Carregamento do controle que apresenta a imagem para a ViewModel
                                FonteImagem = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Detalhes: " + ex.InnerException, "OK");
            }
        }

        public async void SalvarImagemAzure()
        {
            try
            {
                Usuario u = new Usuario();
                u.Foto = foto;
                u.Id = Preferences.Get("UsuarioId", 0);

                string fileName = $"{u.Id}.jpg";

                //define o BLOB no qual a imagem será armazenada
                var blobClient = new BlobClient(conexaoAzureStorage, container, fileName);

                if (blobClient.Exists())
                    blobClient.Delete();

                using (var stream = new MemoryStream(u.Foto))
                {
                    blobClient.Upload(stream);
                }

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

    }
}