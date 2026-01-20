using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace TelaPrincipalAtualizado
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    // Remove TODOS os efeitos visuais nativos do Entry (bordas, foco, sublinhado)
                    ModifyEntry();
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        /// <summary>
        /// Remove bordas nativas, linha de foco e sublinhado do Entry em todas as plataformas
        /// </summary>
        private static void ModifyEntry()
        {
#if WINDOWS
            EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) =>
            {
                var textBox = handler.PlatformView;
                
                // Remove todas as bordas
                textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                textBox.Background = null;
                
                // Remove o visual de foco (borda azul)
                textBox.FocusVisualPrimaryThickness = new Microsoft.UI.Xaml.Thickness(0);
                textBox.FocusVisualSecondaryThickness = new Microsoft.UI.Xaml.Thickness(0);
                textBox.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
                
                // Remove padding interno extra
                textBox.Padding = new Microsoft.UI.Xaml.Thickness(0);
                
                // Define estilo para remover bordas em todos os estados
                textBox.Resources["TextControlBorderThemeThickness"] = new Microsoft.UI.Xaml.Thickness(0);
                textBox.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
            });
#elif ANDROID
            EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) =>
            {
                var editText = handler.PlatformView;
                
                // Remove o background nativo (linha/sublinhado)
                editText.Background = null;
                editText.SetBackgroundColor(Android.Graphics.Color.Transparent);
                
                // Remove padding interno padrão do Android
                editText.SetPadding(0, 0, 0, 0);
                
                // Remove a linha de sublinhado no foco
                editText.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            });
#elif IOS || MACCATALYST
            EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) =>
            {
                var textField = handler.PlatformView;
                
                // Remove background e bordas
                textField.BackgroundColor = UIKit.UIColor.Clear;
                textField.Layer.BorderWidth = 0;
                textField.BorderStyle = UIKit.UITextBorderStyle.None;
                
                // Remove sombra
                textField.Layer.ShadowOpacity = 0;
            });
#endif
        }
    }
}
