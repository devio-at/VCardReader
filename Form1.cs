using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MyProject.vCard;

namespace testVCardReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
               textBox1.Text = File.ReadAllText(openFileDialog1.FileName);
        }

        const int MaxPhones = 4, MaxEmail = 4, MaxAddresses = 4, MaxUrls = 2;

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

            textBox2.AppendText("Full Name\t");
            textBox2.AppendText("Surname\tGiven Name\tMiddle Name\t");
            textBox2.AppendText("Title\t");
            textBox2.AppendText("Prefix\t");
            textBox2.AppendText("Suffix\t");

            textBox2.AppendText("Birthday\t");
            textBox2.AppendText("Rev\t");
            textBox2.AppendText("Org\t");

            for (int i = 0; i < MaxPhones; i++)
            {
                textBox2.AppendText("Phone " + (i + 1).ToString() + "\t");
            }
            for (int i = 0; i < MaxEmail; i++)
            {
                textBox2.AppendText("Email " + (i + 1).ToString() + "\t");
            }
            for (int i = 0; i < MaxUrls; i++)
            {
                textBox2.AppendText("URL " + (i + 1).ToString() + "\t");
            }

            for (int i = 0; i < MaxAddresses; i++)
            {
                textBox2.AppendText("Address " + (i + 1).ToString() + "\t");
            }

            textBox2.AppendText("Note" + Environment.NewLine);

            foreach (string s in textBox1.Text.Split(new string[] { "END:VCARD" }, StringSplitOptions.RemoveEmptyEntries))
            {
                vCardReader vc = new vCardReader();
                vc.ParseLines(s);   //textBox1.Text);
                //textBox2.Clear();
                textBox2.AppendText(vc.FormattedName + "\t");
                textBox2.AppendText(vc.Surname + "\t" + vc.GivenName + "\t" + vc.MiddleName + "\t");
                textBox2.AppendText(vc.Title + "\t");
                textBox2.AppendText(vc.Prefix + "\t");
                textBox2.AppendText(vc.Suffix + "\t");

                textBox2.AppendText((vc.Birthday.HasValue ? vc.Birthday.Value.ToShortDateString() : "") + "\t");
                textBox2.AppendText((vc.Rev.HasValue ? vc.Rev.Value.ToShortDateString() + " " + vc.Rev.Value.ToShortTimeString() : "") + "\t");

                textBox2.AppendText(vc.Org + "\t");


                for (int i = 0; i < MaxPhones; i++)
                {
                    if (i < vc.Phones.Count)
                        textBox2.AppendText(/*vc.Phones[i].phoneType.ToString() + " " + vc.Phones[i].homeWorkType.ToString() + (vc.Phones[i].pref ? "Preferred" : "") + "=" +*/
                            (!vc.Phones[i].number.Contains(" ") ? "'" : "") + vc.Phones[i].number);
                    textBox2.AppendText("\t");
                }
                for (int i = 0; i < MaxEmail; i++)
                {
                    if (i < vc.Emails.Count)
                        textBox2.AppendText(/*vc.Emails[i].homeWorkType.ToString() + " " + (vc.Emails[i].pref ? "Preferred" : "") + "=" +*/ vc.Emails[i].address);
                    textBox2.AppendText("\t");
                }

                for (int i = 0; i < MaxUrls; i++)
                {
                    if (i < vc.URLs.Count)
                        textBox2.AppendText(/*vc.Emails[i].homeWorkType.ToString() + " " + (vc.Emails[i].pref ? "Preferred" : "") + "=" +*/ vc.URLs[i]);
                    textBox2.AppendText("\t");
                }

                for (int i = 0; i < MaxAddresses; i++)
                {
                    if (i < vc.Addresses.Count)
                    {
                        textBox2.AppendText(//vc.Addresses[i].homeWorkType.ToString() + "=" + vc.Addresses[i].po + "," + vc.Addresses[i].ext + ", "
                        vc.Addresses[i].street + ", "
                        + vc.Addresses[i].locality + ", "
                        + vc.Addresses[i].region + ", "
                        + vc.Addresses[i].postcode + ", "
                        + vc.Addresses[i].country);
                    }
                    textBox2.AppendText("\t");
                }

                textBox2.AppendText((vc.Note != null ? vc.Note.Replace(@"\n", " ").Trim() : "") + Environment.NewLine);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.SelectedText))
                textBox2.SelectAll();
            textBox2.Copy();
        }

        
    }
}