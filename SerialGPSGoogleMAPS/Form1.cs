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

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = cb_port.SelectedItem.ToString();
            try
            {
                if (!serialPort1.IsOpen)
                {
                    tb_serialPort.Text = serialPort1.PortName + " Listo!\r\n";
                    serialPort1.Open();
                    btn_conectar.Enabled = false;
                }
                else
                {
                    tb_serialPort.Text="ya esta abierto";
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
            tb_serialPort.Clear();
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
            tb_serialPort.AppendText(RXmssg);
            string[] cadena = RXmssg.Split(';');
            int numero = cadena.Length;
            tb_speed.Text = cadena[4];
            string latidude = cadena[2];
            tb_latitud.Text = latidude;
            string longitude = cadena[3];
            tb_longitud.Text = longitude;
            try
            {
                StringBuilder tracker = new StringBuilder();
                tracker.Append("http://maps.google.com/maps?q=");
                if (longitude!=string.Empty&&latidude!=string.Empty)
                {
                    tracker.Append(latidude+","+longitude);
                    webBrowser1.Navigate(tracker.ToString());
                }
                else
                {
                    MessageBox.Show("error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"error");
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