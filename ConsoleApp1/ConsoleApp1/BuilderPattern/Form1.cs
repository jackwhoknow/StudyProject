using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPattern
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            Pen pen = new Pen(Color.Yellow);
            PersonBuilder person1 = new PersonThinBuilder(this.pictureBox1.CreateGraphics(),pen);
            PersonDirector pd = new PersonDirector(person1);
            pd.CreatePerson();
            this.pictureBox1.Refresh();
        }
    }
}
