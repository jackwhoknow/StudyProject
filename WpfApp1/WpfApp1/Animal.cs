using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfApp1
{
    class Animal : INotifyPropertyChanged
    {
        private string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
                NotifyPropertyChange("Name");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
