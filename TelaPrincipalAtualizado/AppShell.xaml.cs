using Microsoft.Maui.Controls;
using TelaCadastroLocatem.Views;
using TelaPrincipalAtualizado;
namespace TelaCadastroLocatem
{
    public partial class AppShell : Shell
    {
        private static bool _routeaRegistered; // Campo estático para rastrear se a rota já foi registrada.
        public AppShell()
        {
            InitializeComponent();

            // 1. REGISTRO DA TELA DE CADASTRO DO LOCATÁRIO
            // Isso permite que você navegue para ela usando "LocatarioPage"
            Routing.RegisterRoute("LocatarioPage", typeof(LocatarioPage));

            // 2. REGISTRO DA TELA DE LOGIN
            // Isso permite que você navegue diretamente para ela usando "LoginPage"
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));

            Routing.RegisterRoute("LocadorPage", typeof(LocadorPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));

            //Routing.RegisterRoute("LocatarioPage", typeof(LocatarioPage));

            //if (!_routeaRegistered)
            //{
            //    Routing.RegisterRoute(nameof(Views.LocatarioPage), typeof(Views.LocatarioPage)); // Registra a rota para LocatarioPage, permitindo a navegação para essa página usando seu nome.
            //    _routeaRegistered = true; // Define o campo como true para evitar registros duplicados.
            //}

            //if (!_routeaRegistered)
            //{
            //    Routing.RegisterRoute(nameof(Views.LocadorPage), typeof(Views.LocadorPage)); // Registra a rota para locadorPage, permitindo a navegação para essa página usando seu nome.
            //    _routeaRegistered = true; // Define o campo como true para evitar registros duplicados.
            //}
        }
    }
}
