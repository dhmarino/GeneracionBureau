using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneracionBureau //Botones Dinamicos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var versiones = typeof(Form1).Assembly.GetName().Version;
            lblVersion.Text = "Versión " + versiones.ToString();
            lblVersion.Visible = true;
            lblVersion.Refresh();
            LeerArchivo();
            GenerarBotones();
        }
        string rutaArchivo = "C:\\valid\\clientes\\BUREAU\\clientes.ini"; // Ruta completa del archivo con los botones
        List<string[]> datos = new List<string[]>();
        private void LeerArchivo()
        {
            try
            {
                using (StreamReader sr = new StreamReader(rutaArchivo))
                {
                    string linea;
                    sr.ReadLine();
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] datosLinea = linea.Split(','); // Ajusta el delimitador si es diferente
                        datos.Add(datosLinea);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GenerarBotones()
        {// Limpiar el panel antes de agregar nuevos botones
            panel1.Controls.Clear();
            
            // Ordenar los datos por el nombre (primer elemento de cada línea)
            var datosOrdenados = datos.OrderBy(r => r[0]).ToList();

            int x = 10; // Posición inicial en X
            int y = 10; // Posición inicial en Y
            int anchoBoton = 180;
            int altoBoton = 40;
            int espacioEntreBotones = 10;
            int columnaActual = 0;
            int maxColumnas = panel1.Width / (anchoBoton + espacioEntreBotones); // Calcula el número máximo de columnas
            try
            {
                foreach (string[] datosLinea in datosOrdenados)
                {
                    Button btn = new Button();
                    btn.Text = datosLinea[0];
                    btn.Location = new Point(x, y);
                    btn.Size = new Size(anchoBoton, altoBoton);
                    btn.Image = Image.FromFile(datosLinea[3]);
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.TextAlign = ContentAlignment.MiddleRight;
                    //btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = new Font("Arial", 12, FontStyle.Bold);

                    if (columnaActual >= maxColumnas)
                    {
                        // Si se llegó al final de la fila, reiniciar la columna y aumentar la fila
                        columnaActual = 0;
                        y += altoBoton + espacioEntreBotones;
                    }
                    x = columnaActual * (anchoBoton + espacioEntreBotones) + 10;

                    btn.Location = new Point(x, y);
                    panel1.Controls.Add(btn);

                    columnaActual++;

                    // Manejar posibles excepciones al ejecutar el programa
                    btn.Click += (sender, e) =>
                    {
                        try
                        {
                            //System.Diagnostics.Process.Start(datosLinea[1]);
                            this.Enabled = false;
                            ProcessStartInfo psi = new ProcessStartInfo();
                            psi.WorkingDirectory = @datosLinea[1];
                            psi.FileName = @datosLinea[2];
                            psi.CreateNoWindow = true;
                            Process p = Process.Start(psi);
                            p.WaitForExit();
                            this.Activate();
                            this.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al ejecutar el programa: " + ex.Message);
                            this.Enabled = true;
                        }
                    };

                    panel1.Controls.Add(btn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            // Forzar una actualización de la interfaz del panel
            panel1.Refresh();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnHerramientas_Click(object sender, EventArgs e)
        {
            try
            {
                string cliente = "HBUREAU";
                StreamReader config = new StreamReader(cliente + ".ini", false);
                config.ReadLine();
                string inputdir = config.ReadLine().Trim();
                config.ReadLine();
                string nombre_ejecutable = config.ReadLine().Trim();

                this.Enabled = false;
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.WorkingDirectory = @inputdir;
                psi.FileName = @nombre_ejecutable;
                psi.CreateNoWindow = true;
                Process p = Process.Start(psi);
                p.WaitForExit();
                this.Activate();
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error..." + ex.Message);
                this.Enabled = true;
            }
        }

        
    }
}
