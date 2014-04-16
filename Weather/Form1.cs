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
        string json;
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Wroclaw");
            comboBox1.Items.Add("Warsaw");
            comboBox1.Items.Add("London");
            comboBox1.Items.Add("New York");
            comboBox1.Items.Add("Los Angeles");
            comboBox1.Text = "Wroclaw";
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string icon = "weather.png";
            bool err = false;

            var city = comboBox1.Text;
            if (city.Contains(' '))
                city.Replace(' ', '_');
            using(WebClient wc = new WebClient())
            {
                try
                {
                    json = wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=" + comboBox1.Text + ",pl");

                }
                catch
                {
                    richTextBox1.Text = "Server not responding. Try again in a second.";
                    err = true;
                }
            }
            if (!err)
            {
                var weather = JsonConvert.DeserializeObject<WeatherObject>(json);

                if (pictureBox1.Image != null)
                    weatherIcon.Dispose();

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile("http://openweathermap.org/img/w/" + weather.Weather[0].icon + ".png", icon);
                }
                var time = UnixTimeStampToDateTime(weather.dt);
                richTextBox1.Text = String.Format("Temperatura: {0:0.0} °C,\nCiśnienie: {1} hPa,\nNiebo: "
                                + weather.Weather[0].description + ",\nOdczyt: " + time.ToLongTimeString() + '.', weather.Main.temp - 273.15, weather.Main.pressure);
                weatherIcon = new Bitmap(icon);
                pictureBox1.Image = (Image)weatherIcon;
            }
        }
    }
}
