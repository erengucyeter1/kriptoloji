using kriptoloji.core;
using kriptoloji.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace kriptoloji.forms
{
    public partial class MainPage : Form
    {
        private OptionsPanelHelper optionsPanelHelper;
        ICryptAlgorithm algorithm;

        public MainPage()
        {
            InitializeComponent();
            optionsPanelHelper = new OptionsPanelHelper(OptionsPanel);
            initComboBox();


        }

        private void initComboBox()
        {
            Methods[] methods = (Methods[])Enum.GetValues(typeof(Methods));

            MethodComboBox.DataSource = methods;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {

            Dictionary<string , string> options = new Dictionary<string , string>();

            bool clearFlag = true;


            foreach (KeyOptionComponent component in OptionsPanel.Controls.OfType<KeyOptionComponent>())
            {
                options[component.KeyName] = component.Value;
            }

            switch (GetSelectedMethod())
            {
                case Methods.Kaydırmalı:
                    this.algorithm = new Kaydirmali(options);
                    break;
            
                case Methods.Doğrusal:
                    this.algorithm = new Dogrusal(options);
                    break;
                case Methods.YerDeğiştirme:
                    this.algorithm = new YerDegistirme(options);
                    break;
                case Methods.Permutasyon:
                    this.algorithm = new Permutasyon(options);
                    break;
                case Methods.SayıAnahtarlı:
                    this.algorithm = new SayiAnahtarli(options);
                    break;
                case Methods.Rota:
                    this.algorithm = new Rota(options);
                    break;
                case Methods.ZikZak:
                    this.algorithm = new ZikZak(options);
                    break;
                case Methods.Vigenere:
                    this.algorithm = new Vigenere(options);
                    break;
                case Methods.DörtKare:
                    this.algorithm = new DortKare(options);
                    clearFlag = false;
                    break;
                case Methods.Hill:
                    this.algorithm = new Hill(options);
                    break;

            }

            CryptHandler cryptHandler = new CryptHandler(this.algorithm);

            //try
            {
                string result = cryptHandler.Apply(GetInputText(), this.CryptRadioButton.Checked, (clearFlag || this.CryptRadioButton.Checked));
                this.SetOutputText(result);
            }
            //catch (Exception ex)
            {
              // MessageBox.Show(ex.Message.ToString());
            }

            
            

        }

        private void MethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GetSelectedMethod())
            {
                case Methods.Kaydırmalı:
                    optionsPanelHelper.SetPanel(typeof(Kaydirmali));
                    break;
                case Methods.Doğrusal:
                    optionsPanelHelper.SetPanel(typeof(Dogrusal));
                    break;
                case Methods.YerDeğiştirme:
                    optionsPanelHelper.SetPanel(typeof(YerDegistirme));
                    break;
                case Methods.Permutasyon:
                    optionsPanelHelper.SetPanel(typeof(Permutasyon));
                    break;
                case Methods.SayıAnahtarlı:
                    optionsPanelHelper.SetPanel(typeof(SayiAnahtarli));
                    break;
                case Methods.Rota:
                    optionsPanelHelper.SetPanel(typeof(Rota));
                    break;
                case Methods.ZikZak:
                    optionsPanelHelper.SetPanel(typeof(ZikZak));
                    break;
                case Methods.Vigenere:
                    optionsPanelHelper.SetPanel(typeof(Vigenere));
                    break;
                case Methods.DörtKare:
                    optionsPanelHelper.SetPanel(typeof(DortKare));
                    break;
                case Methods.Hill:
                    optionsPanelHelper.SetPanel(typeof(Hill));
                    break;
                default:
                    optionsPanelHelper.SetPanel(null);
                    break;
            }
        }

        private Methods GetSelectedMethod()
        {
            int index = MethodComboBox.SelectedIndex;
            Methods SelectedMethod = (Methods)Enum.GetValues(typeof(Methods)).GetValue(index);
            return SelectedMethod;
        }

        private string GetInputText()
        {
            return this.InputTextBox.Text;
        }
        private void SetOutputText(string text)
        {
            this.OutputTextBox.Text = text;
        }
    }
}
