using System.ComponentModel;          // Biblioteca necessária para usar INotifyPropertyChanged
using System.Runtime.CompilerServices; // Permite usar o [CallerMemberName] para identificar automaticamente quem chamou o método.

namespace TelaPrincipalAtualizado.Models
{
    // Esta classe representa cada produto que aparece no carrinho.
    // Ela implementa INotifyPropertyChanged para avisar a interface (UI) quando algo muda.
    public partial class ProdutoModel : INotifyPropertyChanged
    {
        
        

       
            // Nome do produto (ex: "Furadeira DeWalt 20V")
            public string Nome { get; set; } = string.Empty;

            // Nome da loja que fornece o produto
            public string Loja { get; set; } = string.Empty;

            // Caminho da imagem do item (ex: "furadeira.png")
            public string Imagem { get; set; } = string.Empty;

            // Preço que será cobrado por dia com desconto aplicado
            public decimal PrecoComDesconto { get; set; }

            // --------------------- QUANTIDADE ---------------------

            // Campo privado para armazenar a quantidade selecionada
            private int _quantidade = 1;

            // Quantidade de peças que o usuário quer alugar
            public int Quantidade
            {
                get => _quantidade;                     // Retorna o valor atual
                set
                {
                    _quantidade = value;                // Atualiza o valor
                    OnPropertyChanged();                // Notifica a interface que mudou
                }
            }

            // --------------------- DIAS SELECIONADOS ---------------------

            // Campo privado para armazenar quantos dias o item será alugado
            private int _diasSelecionados = 1;

            // Quantidade de dias escolhida pelo usuário
            public int DiasSelecionados
            {
                get => _diasSelecionados;              // Retorna o valor atual
                set
                {
                    _diasSelecionados = value;         // Atualiza o valor
                    OnPropertyChanged();               // Notifica a UI da alteração
                }
            }

            // --------------------- NOTIFICAÇÃO PARA A UI ---------------------

            // Evento que avisa a interface gráfica (XAML) que um valor mudou.
            public event PropertyChangedEventHandler? PropertyChanged;

            // Método que dispara o evento PropertyChanged automaticamente.
            // O parâmetro [CallerMemberName] pega o nome da propriedade que chamou este método.
            void OnPropertyChanged([CallerMemberName] string? prop = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        
    }
}
