using Android.App;
using Android.Runtime;

namespace AppRpgEtec
{
    //habilitar o trafico de dados para ele transitar no meio externo
    [Application(UsesCleartextTraffic=true)]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
