namespace AppRpgEtec.Views.Personagens;
using AppRpgEtec.ViewModels;
using AppRpgEtec.ViewModels.Personagens;

public partial class CadastroPersonagemView : ContentPage
{
	private CadastroPersonagemViewModel cadViewModel;
	public CadastroPersonagemView()
	{
		InitializeComponent();
		cadViewModel = new CadastroPersonagemViewModel();
		BindingContext = cadViewModel;
		Title = "Novo Personagem";
	}
}