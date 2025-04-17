using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kriptoloji.forms
{
    internal class KeyOptionComponent : Panel
    {
        public readonly string KeyName;
        TextBox InputTextBox;

        public string Value { get
            {
                return InputTextBox.Text;
            }
            set
            {
                InputTextBox.Text = value;
            }
        }

       

        public string GetValue { 
            get
            {
                return InputTextBox.Text;
            }
        }
        public KeyOptionComponent(string name)
        {
            this.KeyName = name;   
            initComponent();
        }

        private void initComponent()
        {
            this.BackColor = Color.LightGray;



            Label nameLabel = new Label();
            nameLabel.Text = KeyName;
            nameLabel.Location = new System.Drawing.Point(5, 5);
            nameLabel.AutoSize = true;
            this.Controls.Add(nameLabel);

            InputTextBox = new TextBox();
            InputTextBox.BackColor = Color.Aqua;
            InputTextBox.Location = new System.Drawing.Point(5, 23 );


            this.Controls.Add(InputTextBox);

            int min_y = 15;

            foreach (Control control in this.Controls)
            {
                min_y += control.Size.Height;
            }

            this.Size = new Size(200, min_y);





        }
    }
}
