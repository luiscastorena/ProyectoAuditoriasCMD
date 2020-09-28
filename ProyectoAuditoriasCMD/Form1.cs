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
        public static  int contador = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteCommand("nmap -sn 192.168.100.98/24");
            timer1.Start();
        }
        static void ExecuteCommand(string _Command)
        {

            //Indicamos que deseamos inicializar el proceso cmd.exe junto a un comando de arranque. 
            //(/C, le indicamos al proceso cmd que deseamos que cuando termine la tarea asignada se cierre el proceso).
            //Para mas informacion consulte la ayuda de la consola con cmd.exe /? 
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + _Command);
            // Indicamos que la salida del proceso se redireccione en un Stream
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            //Indica que el proceso no despliegue una pantalla negra (El proceso se ejecuta en background)
            procStartInfo.CreateNoWindow = false;
            //Inicializa el proceso
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            //Consigue la salida de la Consola(Stream) y devuelve una cadena de texto
            string result = proc.StandardOutput.ReadToEnd();
            //Muestra en pantalla la salida del Comando
            Resultados(result);
            contador++;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ExecuteCommand("nmap -sn 192.168.100.98/24");
        }

        static  void Resultados(string result)
        {
            int primerTotal = 0;
            int segundoTotal = 0;
            int ultimoTotal = 0;
            primerTotal = Regex.Matches(result, "MAC Address").Count;

            if (contador == 1)
            {
                MessageBox.Show("Hay " + primerTotal + " dispositivos conectados");
            }
            if(contador == 1)
            {
                segundoTotal = Regex.Matches(result, "MAC Address").Count;
                if (segundoTotal > primerTotal)
                {
                    MessageBox.Show("Hay un nuevo dispositivo encontrado");
                    email();
                }

            }
            if(contador > 1)
            {
                ultimoTotal = Regex.Matches(result, "MAC Address").Count;
                if(ultimoTotal > segundoTotal)
                {
                    MessageBox.Show("Hay un nuevo dispositivo encontrado");
                    email();
                }
            }
        }

        static void email()
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add("josehenderson10@gmail.com");
            msg.Subject = "Se encontro un nuevo intruso en su red Wi-Fi";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = "Se encontro un intruso en su red Wi-Fi: ";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.From = new System.Net.Mail.MailAddress("luiscastorena.095@gmail.com");
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential("luiscastorena.095@gmail.com", "rko000rko");
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
    }

}
