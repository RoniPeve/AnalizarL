using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication6
{
    public partial class PalabrasReservadas : Form
    {
        public PalabrasReservadas()
        {
            InitializeComponent();
        }

        Analizar obj = new Analizar();
        void palabras()
        {
            foreach (var item in obj.Reservadas)
            {
                richTextPalabras.Text = string.Join("\n", obj.Reservadas);
            }
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]

        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PalabrasReservadas_Load(object sender, EventArgs e)
        {
            palabras();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lblPalabras_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
