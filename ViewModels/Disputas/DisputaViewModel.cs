using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Database;
using AppRpgEtec.Services.Personagens;

namespace AppRpgEtec.ViewModels.Disputas
{
    public class DisputaViewModel : BaseViewModel
    {
        private PersonagemService pService;

        public ObservableCollection<Personagem> PersonagemEncontrados { get; set; }

        public PersonagemService Atacante { get; set; }

        public PersonagemService Oponente { get; set; }

        public DisputasViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);

            Atacante = new Personagem();
            Oponente = new Personagem();

            PersonagemEncontrados = new ObservableCollection<Personagem>();
        }

        public async Task PesquisarPersonagens(string textoPesquisaPersonagem)
        {
            try
            {
                PersonagemEncontrados = await pService.GetByNomeAproximadoAsync(textoPesquisaPersonagem);
                OnPropertyChanged(nameof(PersonagemEncontrados));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);

            Atacante = new Personagem();
            Oponente = new Personagem();

            PersonagensEncontrados = new ObservableCollection<Personagem>();

            PesquisarPersonagensCommand =
                new Command<string>(async (string pesquisa) => { await PesquisarPersonagens(pesquisa); });
        }

        public ICommandMapper PesquisarPersonagensCommand { get; set; }

        public string DescricaoPersonagemAtacante
            { get => Atacante.Nome; }

        public string DescricaoPersonagemOponente
            { get => Oponente.Nome; }


        public async void SelecionarPersonagem(Personagem p)

        {
            try
            {
                string tipoCombatente = await Application.Current.MainPage
                    .DisplayActionSheet("Atacante ou Oponente?", "Cancelar", "", "Atacante", "Oponente");

                if (tipoCombatente == "Atacante")
                {
                    Atacante = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemAtacante));
                }
                else if (tipoCombatente == "Oponente")
                {
                    Oponente = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemOponente));
                }
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private Personagem personagemSelecionado;

        public Personagem PersonagemSelecionado
        {
            set
            {
                if(value != null)
                {
                    personagemSelecionado = value;
                    SelecionarPersonagem(personagemSelecionado);
                    OnPropertyChanged();
                    PersonagemEncontrados.Clear();
                }
            }
        }
        private string textoBuscaDigitado = string.Empty;

        public string TextoBuscaDigitado
        {
            get { return textoBuscaDigitado; } 
            set 
            {
                if ((value != null && !string.IsNullOrEmpty(value) && value.Length > 0))
                {
                    textoBuscaDigitado = value;
                    _ = PesquisarPersonagens(textoBuscaDigitado);
                }
                else
                {
                    PersonagemEncontrados.Clear();
                }
            }

    }
}
