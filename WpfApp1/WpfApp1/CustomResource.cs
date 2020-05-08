using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class CustomResources
    {
        public static ComponentResourceKey SadTileBrushKey
        {
            get
            {
                return new ComponentResourceKey(typeof(CustomResources), "SadTileBrush");
            }
        }
    }
}
