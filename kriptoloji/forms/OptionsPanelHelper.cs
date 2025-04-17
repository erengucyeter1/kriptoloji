using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kriptoloji.forms
{
    internal class OptionsPanelHelper
    {
        Panel panel;
        public OptionsPanelHelper(Panel panel)
        {
            this.panel = panel;
        }

        public void SetPanel(Type optionsClass)
        {
            panel.Controls.Clear();
            if (optionsClass == null)
            {
                return;
            }
            int index = 0;
            var optionNamesProperty = optionsClass.GetProperty("optionNames");
            if (optionNamesProperty != null)
            {
                var optionNames = optionNamesProperty.GetValue(null) as string[];
                if (optionNames != null)
                {
                    Button randomBtn = new Button();
                    randomBtn.Text = "Random";
                    randomBtn.Location = new System.Drawing.Point(130, 10);
                    bool buttonFlag = false;


                    int maxHeight = 0;

                    foreach (string key in optionNames)
                    {
                        var component = new KeyOptionComponent(key);

                        string randomValueMethodName = "GetRandom" + key;
                        var randomValueMethod = optionsClass.GetMethod(randomValueMethodName);

                        if (randomValueMethod != null)
                        {
                            randomBtn.Click += (sender, e) =>
                            {

                                if (randomValueMethod != null)
                                {

                                    Object[] panelParams = new Object[panel.Controls.Count];
                                    int paramİndex = 0;
                                    foreach (var item in panel.Controls)
                                    {
                                        if (item is KeyOptionComponent)
                                        {
                                            var keyOption = item as KeyOptionComponent;
                                            panelParams[paramİndex] = new KeyValuePair<string, string>(keyOption.KeyName, keyOption.GetValue);
                                            paramİndex++;
                                        }
                                    }

                                    try
                                    {
                                        var randomValue = randomValueMethod.Invoke(null, new object[] { panelParams });
                                        component.Value = randomValue.ToString();

                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Rastgele Değer Üretilirken Bir Hata Oluştu.\nLütfen Girlmesi Gereken Parametreleri Gözden Geçirin!");
                                    }

                                }
                            };
                            buttonFlag = true;

                        }
                        


                        component.Location = new System.Drawing.Point(5, 5 + (index * (component.Size.Height)));
                        maxHeight += component.Size.Height;

                        panel.Controls.Add(component);
                        index++;
                    }
                    randomBtn.Size = new System.Drawing.Size(50, maxHeight- 10);
                    if (buttonFlag)
                        this.panel.Controls.Add(randomBtn);
                    randomBtn.BringToFront();

                }
                else
                {
                    MessageBox.Show("optionNames property is null or not a string array.");
                }
            }
            else
            {
                MessageBox.Show("optionNames property not found in " + optionsClass.Name);
            }
        }

    }
}
