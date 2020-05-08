using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       
       
        public MainWindow()
        {
            InitializeComponent();

          string userName=  Properties.Settings.Default.UserName ;
          string password=  Properties.Settings.Default.Password ;


            Person person = new Person();
            person.Name = "刘光胜";

            Animal animal = new Animal();
            animal.Name = "熊猫";

            MainWinowVM mainVM = new MainWinowVM
            {
                MyPerson = person,
                MyAnimal = animal
            };
            this.DataContext = mainVM;

            //Binding binding = new Binding();
            //binding.Source = this.sliderFontSize;
            //binding.Path = new PropertyPath("Value");
            //binding.Mode = BindingMode.TwoWay;

            //this.tb2.SetBinding(TextBox.FontSizeProperty, binding);


            //CommandBinding commandBinding = new CommandBinding(ApplicationCommands.New);
            //commandBinding.Executed += CommandBinding_Executed;
            //this.CommandBindings.Add(commandBinding);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("New command triggered by "+e.Source.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.tb2.FontSize = 60;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
    }
}
