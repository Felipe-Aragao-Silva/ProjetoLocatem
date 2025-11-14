using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TelaPrincipalAtualizado.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace TelaPrincipalAtualizado.ViewModels
{
    public partial class HistoricoViewModel : ObservableObject
    {
        // Coleções originais (não filtradas)
        private readonly ObservableCollection<ItemModel> _todasCompras;
        private readonly ObservableCollection<ItemModel> _todasOfertas;

        [ObservableProperty]
        private ObservableCollection<ItemModel> historicoCompra;

        [ObservableProperty]
        private ObservableCollection<ItemModel> ofertas;

        [ObservableProperty]
        private string tipoSelecionado = "Todos";

        [ObservableProperty]
        private double precoMaximo = 200;


        public HistoricoViewModel()
        {
            _todasCompras = new ObservableCollection<ItemModel>
            {
                new() { Nome="Betoneira 400L Schulz", PrecoPorDia="R$ 89,00/dia", Valor=89, Localizacao="Campinas - SP", Tipo="Equipamento", ImageSource= "betoneiradois.png" },
                new() { Nome="Serra Mármore Makita", PrecoPorDia="R$ 25,00/dia", Valor=25, Localizacao="Guarulhos - SP", Tipo="Ferramenta", ImageSource="serradois.png" },
                new() { Nome="Compressor Schulz", PrecoPorDia="R$ 50,00/dia", Valor=50, Localizacao="Santos - SP", Tipo="Equipamento", ImageSource="compressordois.png" },
                new() { Nome="Lixadeira Dewalt", PrecoPorDia="R$ 35,00/dia", Valor=35, Localizacao="Sorocaba - SP", Tipo="Ferramenta", ImageSource="lixadeiradois.png" },
                new() { Nome="Cortadora de Piso Norton", PrecoPorDia="R$ 40,00/dia", Valor=40, Localizacao="Jundiaí - SP", Tipo="Equipamento", ImageSource="cortadoradois.png" },
                new() { Nome="Parafusadeira Bosch", PrecoPorDia="R$ 12,90/dia", Valor=12.9m, Localizacao="São Paulo - SP", Tipo="Ferramenta", ImageSource="parafusadeira.png" }
            };

            _todasOfertas = new ObservableCollection<ItemModel>
            {
                new() { Nome="Gerador Gasolina 2500W", PrecoPorDia="R$ 110,00/dia", Valor=110, Localizacao="São Paulo - SP", Tipo="Equipamento", ImageSource="gerador.png" },
                new() { Nome="Furadeira Black&Decker", PrecoPorDia="R$ 30,00/dia", Valor=30, Localizacao="Curitiba - PR", Tipo="Ferramenta", ImageSource="furadeirablack.png" },
                new() { Nome="Esmerilhadeira Bosch", PrecoPorDia="R$ 33,00/dia", Valor=33, Localizacao="São Paulo - SP", Tipo="Ferramenta", ImageSource="esmerilhadeira.png" },
                new() { Nome="Andaime Tubular", PrecoPorDia="R$ 65,00/dia", Valor=65, Localizacao="Campinas - SP", Tipo="Equipamento", ImageSource="andaime.png" },
                new() { Nome="Martelo Demolidor", PrecoPorDia="R$ 55,00/dia", Valor=55, Localizacao="Rio de Janeiro - RJ", Tipo="Ferramenta", ImageSource="martelodois.png" },
                new() { Nome="Lavadora de Alta Pressão", PrecoPorDia="R$ 45,00/dia", Valor=45, Localizacao="Santos - SP", Tipo="Equipamento", ImageSource="lavadoradois.png" }
            };

            // Inicializa listas exibidas
            HistoricoCompra = new ObservableCollection<ItemModel>(_todasCompras);
            Ofertas = new ObservableCollection<ItemModel>(_todasOfertas);
        }


        // COMANDO: Aplicar Filtro
        [RelayCommand]
        public void AplicarFiltro()
        {
            var historicoFiltrado = _todasCompras
                .Where(i => (TipoSelecionado == "Todos" || i.Tipo == TipoSelecionado)
                            && i.Valor <= (decimal)PrecoMaximo)
                .ToList();

            HistoricoCompra = new ObservableCollection<ItemModel>(historicoFiltrado);

            var ofertasFiltradas = _todasOfertas
                .Where(i => (TipoSelecionado == "Todos" || i.Tipo == TipoSelecionado)
                            && i.Valor <= (decimal)PrecoMaximo)
                .ToList();

            Ofertas = new ObservableCollection<ItemModel>(ofertasFiltradas);
        }

        // COMANDO: Limpar Filtros
        [RelayCommand]
        public void LimparFiltros()
        {
            TipoSelecionado = "Todos";
            PrecoMaximo = 200;

            HistoricoCompra = new ObservableCollection<ItemModel>(_todasCompras);
            Ofertas = new ObservableCollection<ItemModel>(_todasOfertas);
        }

        // COMANDO: Abrir FiltroPage        
        [RelayCommand]
        public async Task AbrirFiltro()
        {
            var filtro = new Views.FiltroPage(this);
            await App.Current.MainPage.Navigation.PushModalAsync(filtro);
        }
    }
}
