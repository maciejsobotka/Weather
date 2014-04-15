using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace Weather
{
    public partial class Form1 : Form
    {
        private Bitmap weatherIcon;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string json;
            string icon = "Weather.png";

            using(WebClient wc = new WebClient())
            {
                json=wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=Wroclaw,pl");
            }
            var weather = JsonConvert.DeserializeObject<WeatherObject>(json);
            if (pictureBox1.Image != null)
            {
                //pictureBox1.Dispose();
                weatherIcon.Dispose();
            }
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile("http://openweathermap.org/img/w/01d.png",icon);
            }
            richTextBox1.Text = String.Format("Temperatura: {0:0.0} °C,\nCiśnienie: {1} hPa.",
                                weather.Main.temp-273.15, weather.Main.pressure);
            weatherIcon = new Bitmap(icon);
            pictureBox1.Image = (Image)weatherIcon;
        }
    }
}
