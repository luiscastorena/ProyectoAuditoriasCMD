using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoAuditoriasCMD
{

    public partial class Form1 : Form
    {
        public static List<string> lxl = new List<string>();


        public static int contador = 0;
        public Form1()
        {

            InitializeComponent();
            textBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteCommand("nmap -sn 192.168.100.98/24");
            
        }
        public  void ExecuteCommand(string _Command)
        {
            contador++;
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + _Command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = false;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result;
            result = proc.StandardOutput.ReadLine();
           while(result != null)
            {
                lxl.Add(result);
                result = proc.StandardOutput.ReadLine();
            }
            Resultados(lxl);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ExecuteCommand("nmap -sn 192.168.100.98/24");
        }
        public  void Resultados(List<string> MAC)
        {
           List<string> dirrMac = new List<string>();
           List<string> dirrMac2 = new List<string>();
           string cadena = "";
            string cadena2 = "";
            if (contador == 1)
            {
                foreach (string s in MAC)
                {
                    if (s.StartsWith("MAC"))
                    {
                        dirrMac.Add(s);
                        cadena += s;
                        cadena += "\n";
                    }
                }
                RTB.Text = cadena;
            }
            if( contador > 1)
            {
                int primerR = dirrMac.Count;
                foreach (string s in MAC)
                {
                    if (s.StartsWith("MAC"))
                    {
                        dirrMac2.Add(s);
                        cadena2 += s;
                        cadena2 += "\n";
                    }
                }
                int segundoR = dirrMac2.Count;
                if( segundoR > primerR)
                {
                    textBox1.Enabled = true;
                    MessageBox.Show(" Hay un intruso en tu red");
                    RTB.Clear();
                    RTB.Text = cadena2;
                
                }

            }
                
            
        }

        public void email()
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(textBox1.Text);
            msg.Subject = "Se encontro un nuevo intruso en su red Wi-Fi";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = "Se encontro un intruso en su red Wi-Fi: ";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.From = new System.Net.Mail.MailAddress("luiscastorena.095@gmail.com");
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential("luiscastorena.095@gmail.com", "Aqui va la contraseña de tu correo");
            cliente.Port = 587;
            cliente.EnableSsl = true;
            cliente.Host = "smtp.gmail.com";
            try
            {
                cliente.Send(msg);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al enviar");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExecuteCommand("nmap -sn 192.168.100.98/24");

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            email();
        }
    }

}
