namespace AppRpgEtec.Views.Usuarios;

public partial class LocalizacaoView : ContentPage
{
	LocalizacaoViewModel viewModel;
	public LocalizacaoView()
	{
		InitializeComponent();

		viewModel = new LocalizacaoViewModel();
		//viewModel.IniciarlizarMapa();
		viewModel.ExibirUsuariosNoMapa();

		BindingContext = viewModel;
	}
}