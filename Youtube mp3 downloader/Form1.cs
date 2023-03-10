using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaToolkit;
using VideoLibrary;

namespace Youtube_mp3_downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Boolean format = true;

        private void mp3_CheckedChanged(object sender, EventArgs e)
        {
            format = true;
        }

        private void mp4_CheckedChanged(object sender, EventArgs e)
        {
            format = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Lütfen Klasör Seçiniz"})
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    GetTittle();
                    var yt = YouTube.Default;
                    var video = await yt.GetVideoAsync(textbox1.Text);
                    File.WriteAllBytes(fbd.SelectedPath + @"\" + video.FullName, await video.GetBytesAsync());
                    var inputfile = new MediaToolkit.Model.MediaFile { Filename = fbd.SelectedPath + @"\" + video.FullName };
                    var outputfile = new MediaToolkit.Model.MediaFile { Filename = $"{fbd.SelectedPath + @"\" + video.FullName}.mp3" };

                    using(var enging =new Engine())
                    {
                        enging.GetMetadata(inputfile);
                        enging.Convert(inputfile, outputfile);
                    }

                    if (format == true)
                    {
                        File.Delete(fbd.SelectedPath + @"\" + video.FullName);
                    }
                    else
                    {
                        File.Delete($"{fbd.SelectedPath + @"\" + video.FullName}.mp3");
                    }

                progressBar1.Value = 100;
                MessageBox.Show("İndirme Tamamlandı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Lütfen Dosya Yolu Seçiniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);



            }
        }





        void GetTittle()
        {
            WebRequest istek = HttpWebRequest.Create(textbox1.Text);
            WebResponse yanit;

            yanit = istek.GetResponse();
            StreamReader sr = new StreamReader(yanit.GetResponseStream());
            string gelen = sr.ReadToEnd();
            int baslangic = gelen.IndexOf("<title>") + 7;
            int bitis = gelen.Substring(baslangic).IndexOf("</title>");
            string gelenbilgi = gelen.Substring(baslangic, bitis);
            label1.Text = (gelenbilgi);

        }


    }
}
