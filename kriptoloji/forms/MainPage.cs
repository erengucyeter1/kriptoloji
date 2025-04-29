using kriptoloji.core;
using kriptoloji.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            if(!this.DecryptRadioButton.Checked)
            {
                return;
            }
            if(string.IsNullOrEmpty(this.InputTextBox.Text))
            {
                return;
            }

            if (!this.InputTextBox.Text.Contains("$QuickFill$"))
            {
                return;
            }

            ParseInstantDecryptedData(this.InputTextBox.Text);



        }


        private void ParseInstantDecryptedData(string data)
        {
            string[] lines = data.Split('&');

            StringBuilder stringBuilder = new StringBuilder();

            Dictionary<string, string> options = new Dictionary<string, string>();

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                string[] parts = line.Split(new char[] { ':' }, 2);

                string key = parts[0].Trim();   

                string value = parts.Length > 1 ? parts[1].Trim() : string.Empty;

                options[key] = value;
            }
            string methodName = options[options.Keys.FirstOrDefault(x => x.Contains("Method"))].ToString();

            MethodComboBox.SelectedIndex = (int)GetMethodFromName(methodName);

            this.InputTextBox.Text = options["CryptedText"].ToString(); 

            //Methods method = GetMethodFromName(methodName);
            //SetOptionsPanel(method);

            foreach (KeyOptionComponent component in OptionsPanel.Controls.OfType<KeyOptionComponent>())
            {
                if (options.ContainsKey(component.KeyName))
                {
                    component.Value = options[component.KeyName];
                }
                else
                {
                    component.Value = string.Empty;
                }
            }

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
                CopyData();
            }
            //catch (Exception ex)
            {
              // MessageBox.Show(ex.Message.ToString());
            }

            
            

        }

        private void CopyData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("$QuickFill$");
            stringBuilder.Append("&");
            stringBuilder.Append("Method:");
            stringBuilder.Append(GetSelectedMethod().ToString());
            stringBuilder.Append("&");
            stringBuilder.Append("CryptedText:");
            stringBuilder.Append(this.OutputTextBox.Text);

            foreach(KeyOptionComponent component in OptionsPanel.Controls.OfType<KeyOptionComponent>())
            {
                stringBuilder.Append("&");
                stringBuilder.Append(component.KeyName + ":" + component.Value);
            }

            Clipboard.SetText(stringBuilder.ToString());

        }

        private void MethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOptionsPanel(GetSelectedMethod());
        }

        private void SetOptionsPanel(Methods optionsClass)
        {
            switch (optionsClass)
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

        private Methods GetMethodFromName(string methodName)
        {
            if (Enum.TryParse<Methods>(methodName, out var result))
            {
                return result;
            }
            return Methods.None;
        }
    }
}
