using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_of_Life
{
    public partial class Modal : Form
    {
        public Modal()
        {
            InitializeComponent();
        }

        public int GetNumber1()
        {
            return (int)numericUpDown1.Value;
        }
        public void SetNumber1(int num)
        {
            numericUpDown1.Value = num;
        }

        public int GetNumber2()
        {
            return (int)numericUpDown2.Value;
        }

        public void SetNumber2(int num)
        {
            numericUpDown2.Value = num;
        }

        public int GetNumber3()
        {
            return (int)numericUpDown3.Value;
        }
        public void SetNumber3(int num)
        {
            numericUpDown3.Value = num;
        }

    }
}
