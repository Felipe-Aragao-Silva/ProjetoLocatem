using System.Collections.ObjectModel; // Usado para ObservableCollection — lista que avisa a tela quando altera.
using System.ComponentModel;         // Necessário para INotifyPropertyChanged — avisa mudanças à interface.
using System.Runtime.CompilerServices; // Permite usar CallerMemberName no OnPropertyChanged.
using System.Windows.Input;          // Para usar comandos (ICommand).
using TelaPrincipalAtualizado.Models;           // Importa a classe ProdutoModel.
using System.Linq;                  // Para usar .Sum(), .Any(), etc.
using Microsoft.Maui.Controls;      // Necessário para DisplayAlert e comandos.

namespace TelaPrincipalAtualizado.ViewModels
{
    public partial class CarrinhoViewModel : INotifyPropertyChanged
    {
        // Evento que notifica a interface que uma propriedade mudou.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Método que dispara o evento acima quando alguma propriedade muda.
        void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // ------------------- LISTA DE PRODUTOS -------------------

        // Lista de produtos exibidos na tela.
        // ObservableCollection avisa a interface se remover, adicionar ou alterar itens.
        public ObservableCollection<ProdutoModel> Produtos { get; set; }

        // ------------------- LISTAS PARA OS PICKERS -------------------

        // Quantidades disponíveis para o usuário escolher (1 a 10).
        public ObservableCollection<int> QuantidadesDisponiveis { get; } =
            new(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

        // Dias que o usuário pode alugar (exemplo: 1, 3, 5 dias, etc.)
        public ObservableCollection<int> DiasDisponiveis { get; } =
            new(new[] { 1, 3, 5, 7, 10, 15, 30 });

        // ------------------- TOTAL DO CARRINHO -------------------

        private decimal _total; // Valor armazenado
        public decimal Total   // Propriedade ligada à interface
        {
            get => _total;
            set { _total = value; OnPropertyChanged(); }
        }

        // ------------------- COMANDOS -------------------

        public ICommand RemoverItemCommand { get; }      // Remove produto do carrinho
        public ICommand ConfirmarLocacaoCommand { get; } // Finaliza a locação

        // ------------------- CONSTRUTOR -------------------

        [Obsolete] // Apenas para evitar alerta de API antiga, não causa erro.
        public CarrinhoViewModel()
        {
            // Adiciona um produto como exemplo inicial no carrinho
            Produtos = new ObservableCollection<ProdutoModel>
            {
                new ProdutoModel
                {
                    Nome = "Kit Parafusadeira DeWALT",
                    Loja = "Loja Oficial Dewalt",
                    Imagem = "ff.jpg",
                    PrecoComDesconto = 200m,
                    Quantidade = 1,
                    DiasSelecionados = 1
                }
            };

            // Toda vez que alguma propriedade de um produto mudar (ex.: quantidade ou dias),
            // recalcula o total do carrinho.
            foreach (var p in Produtos)
                p.PropertyChanged += (_, __) => CalcularTotal();

            // Quando adicionar ou remover item da lista, recalcula também.
            Produtos.CollectionChanged += (_, __) => CalcularTotal();

            // Comando para remover item específico do carrinho.
            RemoverItemCommand = new Command<ProdutoModel>(p =>
            {
                if (p != null && Produtos.Contains(p))
                    Produtos.Remove(p);
            });

            // Comando que confirma a locação — mostra um alerta na tela.
            ConfirmarLocacaoCommand = new Command(async () =>
            {
                await App.Current.MainPage.DisplayAlert("Sucesso", "Locação Confirmada!", "OK");
            });

            // Calcula o valor total logo no início.
            CalcularTotal();
        }

        // ------------------- FUNÇÃO QUE CALCULA O TOTAL -------------------

        void CalcularTotal()
        {
            // Multiplica: preço do produto * quantidade * dias selecionados, para todos os itens.
            Total = Produtos.Sum(p => p.PrecoComDesconto * p.Quantidade * p.DiasSelecionados);
        }
    }
}
