using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiemensXmlCreation.ViewModel
{
    public class BaseNotifier : INotifyPropertyChanged
    {

        #region Notify properties
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public void SetIfDiff<T>(ref T property,T value, [CallerMemberName] string memberName=null)
        {
            if(EqualityComparer<T>.Default.Equals(property, value))
                return;
            property = value;
            NotifyPropertyChanged(memberName);
        }
        #endregion
    }
}
