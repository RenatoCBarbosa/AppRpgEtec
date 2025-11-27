using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using AppRpgEtec.Services.Personagens;

namespace AppRpgEtec.ViewModels.Armas
{
    public class CadastroArmaViewModel : BaseViewModel
    {
        private ArmaService aService;
        private PersonagemService pService;

        public CadastroArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);
            pService = new PersonagemService(token);

            ObterPersonagens();

            SalvarCommand = new Command(SalvarArma);
        }

        public ICommand SalvarCommand { get; set; }



        #region Atributos_Propriedades

        private int id;
        private string nome;
        private int dano;
        private int personagemId;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged(nameof(Nome));
            }
        }
        public int Dano
        {
            get => dano;
            set
            {
                dano = value;
                OnPropertyChanged(nameof(Dano));

            }
        }
        public int PersonagemId
        {
            get => personagemId;
            set
            {
                personagemId = value;
                OnPropertyChanged(nameof(PersonagemId));
            }
        }

        private Personagem personagemSelecionado;
        public Personagem PersonagemSelecionado
        {
            get { return personagemSelecionado; }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    OnPropertyChanged(nameof(PersonagemSelecionado));
                }
            }
        }

        public ObservableCollection<Personagem> Personagens { get; set; }

        #endregion

        #region Metodos



        public async void ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public async void SalvarArma()
        {
            try
            {
                Arma model = new Arma()
                {
                    Id = this.id,
                    Nome = this.nome,
                    Dano = this.dano,
                    PersonagemId = this.personagemSelecionado.Id
                };

                if (model.Id == 0)
                    await aService.PostArmaAsync(model);
                else
                    await aService.PutArmaAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvo com sucesso", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }


        #endregion

    }
}
