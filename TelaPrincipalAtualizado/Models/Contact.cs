using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TelaPrincipalAtualizado.Models
{
    /// <summary>
    /// Enum para distinguir entre Locador e Locatário
    /// </summary>
    public enum UserRole
    {
        Locatario,  // Quem aluga
        Locador     // Quem disponibiliza para alugar
    }

    public class Contact : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AvatarSource { get; set; } = "usuario.png";
        public DateTime LastMessageTime { get; set; }
        
        /// <summary>
        /// Papel do usuário no sistema (Locador ou Locatário)
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Locatario;

        private bool _isOnline;
        /// <summary>
        /// Status de presença do contato
        /// </summary>
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                if (_isOnline != value)
                {
                    _isOnline = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusColor));
                }
            }
        }

        /// <summary>
        /// Cor do indicador de status baseado em IsOnline
        /// </summary>
        public Color StatusColor => IsOnline ? Color.FromArgb("#00C853") : Color.FromArgb("#9E9E9E");

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        // Propriedades computadas para binding
        public string FormattedLastMessageTime => LastMessageTime.ToString("HH:mm");
        
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#E0E0E0") : Color.FromArgb("#F0F0F0");

        /// <summary>
        /// Retorna o avatar ou um placeholder padrão se vazio
        /// </summary>
        public string DisplayAvatar => string.IsNullOrEmpty(AvatarSource) ? "usuario.png" : AvatarSource;

        /// <summary>
        /// Texto descritivo do papel do usuário
        /// </summary>
        public string RoleDescription => Role == UserRole.Locador ? "Locador" : "Locatário";

        public Contact() 
        {
            LastMessageTime = DateTime.Now;
        }

        public Contact(int id, string name, bool isOnline = false, UserRole role = UserRole.Locatario)
        {
            Id = id;
            Name = name;
            IsOnline = isOnline;
            Role = role;
            LastMessageTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
