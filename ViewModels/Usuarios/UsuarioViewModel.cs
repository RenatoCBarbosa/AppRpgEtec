using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Views.Personagens;
using AppRpgEtec.Views.Usuarios;
using Microsoft.Maui.Controls;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        //ctor + tab: para criar um construtor
        public UsuarioViewModel()
        {
            uService = new UsuarioService(); // inicializa o serviço que fará a chamada de autenticação
            InicializarCommands(); // configura os comandos que serão usados na interface
        }

        //quer dizer que eu to falando que um async ta chamando um await de AutenticarUsuario
        public void InicializarCommands()
        {
            AutenticarCommand = new Command(async () => await AutenticarUsuario()); // comando que executa a autenticação
            RegistrarCommand = new Command(async () => await RegistrarUsuario()); // comando que executa o registro
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro()); // comando que executa o direcionamento
        }

        private UsuarioService uService; // instância do serviço de usuário

        public ICommand AutenticarCommand { get; set; } // comando que será vinculado ao botão de login
        public ICommand RegistrarCommand { get; set; } // comando que será vinculado ao metodo de registrar usuário
        public ICommand DirecionarCadastroCommand { get; set; } //vinculação para direcionar para cadastro

        #region AtributosPropriedades

        private string login = string.Empty;
        private string senha = string.Empty;

        // propriedade que armazena o login digitado pelo usuário
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(); // notifica a interface sobre a mudança
            }
        }

        // propriedade que armazena a senha digitada pelo usuário
        public string Senha
        {
            get => senha;
            set
            {
                senha = value;
                OnPropertyChanged(); // notifica a interface sobre a mudança
            }
        }

        #endregion

        // segunda region

        #region Metodos

        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task AutenticarUsuario()
        {
            try
            {
                Usuario u = new Usuario(); // cria objeto com dados de login
                u.Username = login;
                u.PassowordString = senha;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u); // envia dados para API e aguarda resposta

                //! - garante que o token não seja null ou vazio antes de prosseguir. Caso contrário, nao vai rolar logar no negocio
                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-vindo(a), {uAutenticado.Username}";

                    //guardando dados para uso futuro
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    //coleta de geolocalização
                    _isCheckingLocation = true;
                    _cancelTokenSource = new CancellationTokenSource();
                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                    Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                    Usuario uLoc = new Usuario();
                    uLoc.Id = uAutenticado.Id;
                    uLoc.Latitude = uAutenticado.Latitude;
                    uLoc.Longitude = uAutenticado.Longitude;

                    UsuarioService uServiceLoc = new UsuarioService(uAutenticado.Token);
                    await uServiceLoc.PutAtualizarLocalizacaoAsync(uLoc);

                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok"); 

                    Application.Current.MainPage = new AppShell(); 
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Informação", "Dados incorretos. (¬_¬ )", "Ok"); 
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação", ex.Message + ex.InnerException, "Ok"); 
            }
        }

        #endregion

        #region Métodos

        public async Task RegistrarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PassowordString = Senha;

                Usuario uRegistrado = await uService.PostRegistrarUsuarioAsync(u);

                //! = significa DIFERENTE 
                if (uRegistrado.Id != 0)
                {
                    string mensagem = $"Usuário Id {uRegistrado.Id}, registrado com sucesso";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    await Application.Current.MainPage
                        .Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
        #endregion

        public async Task DirecionarParaCadastro() //método para exibição da view de Cadastro
        {
            try
            {
                await Application.Current.MainPage
                    .Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", ex.Message + "Deatlhes: " + ex.InnerException, "Ok");
                }
            }
        }
    }
}