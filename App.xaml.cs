namespace AppRpgEtec
{
    // derivada de Application
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent(); 

            // define a página inicial como uma NavigationPage contendo a tela de login
            MainPage = new NavigationPage(new Views.Usuarios.LoginView());
        }

        // método sobrescrito que cria a janela principal da aplicação
        //protected override Window CreateWindow(IActivationState? activationState)
        //{
            // retorna uma nova janela com a estrutura definida em AppShell (provavelmente com Shell navigation)
           // return new Window(new AppShell());
        //}
    }
}
