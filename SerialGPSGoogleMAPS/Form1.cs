using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SerialGPSGoogleMAPS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string RXmssg = System.String.Empty;
        string latitude = System.String.Empty;
        string longitude = System.String.Empty;

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = cb_port.SelectedItem.ToString();
            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.Open();
                    btn_conectar.Enabled = false;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Port might be already Open or Port does not exist");
                throw;
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder tracker = new StringBuilder();
                tracker.Append("https://www.google.com.mx/maps/place/");
                if (longitude != string.Empty && latitude != string.Empty)
                {
                    tracker.Append(latitude + "," + longitude);
                    webBrowser1.Navigate(tracker.ToString());
                }
                else
                {
                    MessageBox.Show("error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error");
                throw;
            }

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RXmssg = serialPort1.ReadLine();
            if (RXmssg!=string.Empty)
            {
            this.Invoke(new EventHandler(desplegar));
            }
        }

        private void desplegar(object sender, EventArgs e)
        {
            string[] cadena = RXmssg.Split(';');
            int numero = cadena.Length;
            try
            {
            tb_speed.Text = cadena[4];
            latitude = cadena[2];
            tb_latitud.Text = latitude;
            longitude = cadena[3];
            tb_longitud.Text = longitude;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cb_port.DataSource = SerialPort.GetPortNames();
        }
    }
}