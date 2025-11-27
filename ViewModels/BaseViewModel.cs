using System;
using System.Collections.Generic;
using System.ComponentModel; // necessário para INotifyPropertyChanged
using System.Linq;
using System.Runtime.CompilerServices; // necessário para [CallerMemberName]
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.ViewModels
{
    // classe base para ViewModels que implementa o padrão de notificação de mudança de propriedade
    public class BaseViewModel : INotifyPropertyChanged
    {
        // evento que será disparado quando uma propriedade for alterada
        //? - Esse evento pode existir, ou pode ser null
        public event PropertyChangedEventHandler? PropertyChanged;

        // método que dispara o evento PropertyChanged
        // o atributo [CallerMemberName] permite que o nome da propriedade que chamou este método
        // seja passado automaticamente, sem precisar especificar manualmente
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // verifica se há algum assinante do evento e dispara a notificação de mudança
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
