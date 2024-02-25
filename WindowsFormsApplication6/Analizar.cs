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
using System.IO;

namespace WindowsFormsApplication6
{
    public partial class Analizar : Form
    {
        public Analizar()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        public string[] Reservadas = new string[] { "pInicio", "pFin", "pIf", "pElse", "pSwitch", "pCase", "pDefault", "pBreak", "pWhile", "pFor", "pDo", "pInt", "pFloat", "pDouble", "pChar", "pDecimal", "pBool", "pString", "pFactorial", "pShort", "pMayus", "pLength", "pSqrt", "pTrue", "pFalse", "pMinus", "print", "pSalir", "pLeer", "pConvertpDecimal", "pConvertpChar", "pConvertpString", "pConvertpInt", "pLog", "pRedondear" };
        string[] Simbolos = new string[] { "<", ">", "<=", ">=", ":", ";", "(", ")", "+", "-", "*", "/", "%", ".", ",", "{", "}", "[", "]", "=", "&&", "||", "==", "!=", "++", "--" };

        private static int x;
        string nomarchivox;
        public void leerarchivo(string nomarchivo)
        {
            StreamReader reader = new StreamReader(nomarchivo, System.Text.Encoding.Default);

            string texto;

            texto = reader.ReadToEnd();


            reader.Close();

            txtentrada.Text = texto;


        }
        private void btnAnalizar_Click(object sender, EventArgs e)
        {

            if (this.dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows.Clear();
            }
            this.AnalizarLexico();
            cambiarColor();

        }
        private void cambiarColor()
        {
            foreach (string x in Reservadas)
            {

                int posicion = 0;

                while (posicion <= txtentrada.Text.LastIndexOf(x))
                {
                    txtentrada.Find(x, posicion, txtentrada.TextLength, RichTextBoxFinds.WholeWord);
                    txtentrada.SelectionColor = Color.Red;

                    posicion = posicion + 1;

                }

                txtentrada.SelectionStart = txtentrada.TextLength;
                txtentrada.SelectionColor = Color.Black;

            }
        }
        private void AnalizarLexico()
        {
            char[] array = this.txtentrada.Text.ToCharArray();
            int i = 0;
            string text = "";
            while (i < array.Length)
            {

                if (array[i] == ' ' || array[i] == '\n' || array[i] == '\t' || array[i] == '"')
                {
                    i++;
                }
                else
                {
                    if (char.IsLetter(array[i]))
                    {
                        WindowsFormsApplication6.Analizar.x = i;
                        text += array[i];
                        i++;
                        if (i < array.Length)
                        {
                            while (char.IsLetterOrDigit(array[i]) || array[i] == '_' || array[i] == '"' || array[i] == ' ')
                            {
                                text += array[i];
                                i++;
                                if (i == array.Length)
                                {
                                    break;
                                }


                            }


                        }



                        this.PalabraReservada(text);
                        text = "";
                    }
                    else
                    {
                        if (char.IsDigit(array[i]))
                        {
                            WindowsFormsApplication6.Analizar.x = i;
                            text += array[i];
                            i++;
                            if (i < array.Length)
                            {
                                while (char.IsDigit(array[i]) || array[i] == '_')
                                {
                                    text += array[i];
                                    i++;
                                    if (i == array.Length)
                                    {
                                        break;
                                    }
                                }
                            }
                            this.Numero(text);
                            text = "";
                        }
                        else
                        {
                            if (array[i] == '|' || array[i] == '&' || array[i] == '=' || array[i] == '*' || array[i] == '<' || array[i] == '>' || array[i] == '+' || array[i] == '-' || array[i] == ';' || array[i] == ':' || array[i] == '{' || array[i] == '}' || array[i] == '(' || array[i] == ')' || array[i] == '%' || array[i] == '#' || array[i] == '!' || array[i] == '/' || array[i] == '.' || array[i] == ',' || array[i] == '[' || array[i] == ']' || array[i] == '$' || array[i] == '_' || array[i] == '"')
                            {
                                WindowsFormsApplication6.Analizar.x = i;
                                text += array[i];
                                i++;
                                if (i < array.Length)
                                {
                                    while (array[i] == '|' || array[i] == '&' || array[i] == '=' || array[i] == '*' || array[i] == '<' || array[i] == '>' || array[i] == '+' || array[i] == '-' || array[i] == ';' || array[i] == ':' || array[i] == '{' || array[i] == '}' || array[i] == '(' || array[i] == ')' || array[i] == '%' || array[i] == '#' || array[i] == '!' || array[i] == '/' || array[i] == '.' || array[i] == ',' || array[i] == '[' || array[i] == ']' || array[i] == '$' || array[i] == '_' || array[i] == '"')
                                    {
                                        text += array[i];
                                        i++;
                                        if (i == array.Length)
                                        {

                                            break;
                                        }
                                    }
                                }
                                this.Simbolo(text);

                                text = "";
                            }

                        }
                    }
                }

            }

        }


        private void Numero(string conca)
        {
            if (int.Parse(conca) >= 0)
            {
                this.Insertar(conca, "Número entero");
                return;
            }

        }
        bool inicio = false;
        bool fin = false;
        private void Simbolo(string conca)
        {
            switch (conca)
            {
                case "<":
                    this.Insertar(conca, "Operador menor que");

                    break;
                case ">":
                    this.Insertar(conca, "Operador mayor que");

                    break;
                case "<=":
                    this.Insertar(conca, "Operador menor igual que");

                    break;
                case ">=":
                    this.Insertar(conca, "Operador mayor igual que");

                    break;
                case ":":
                    this.Insertar(conca, "Caracter de dos puntos");

                    break;
                case ";":
                    this.Insertar(conca, "Caracter de punto y coma");

                    break;
                case "(":
                    this.Insertar(conca, "Caracter de abrir paréntesis");

                    break;
                case ")":
                    this.Insertar(conca, "Caracter de cerrar paréntesis");

                    break;
                case "+":
                    this.Insertar(conca, "Operador suma o concatenacion");

                    break;
                case "-":
                    this.Insertar(conca, "Operador resta");

                    break;
                case "*":
                    this.Insertar(conca, "Operador multiplicación");

                    break;
                case "/":
                    this.Insertar(conca, "Operador División");

                    break;
                case "%":
                    this.Insertar(conca, "Operador Resto");

                    break;
                case ".":
                    this.Insertar(conca, "Caracter punto");

                    break;
                case ",":
                    this.Insertar(conca, "Caracter coma");

                    break;
                case "}":
                    this.Insertar(conca, "Delimitador: Caracter de cerra llave");

                    break;
                case "{":
                    this.Insertar(conca, "Delimitador: Caracter de abrir llave");

                    break;
                case "[":
                    this.Insertar(conca, "Caracter de abrir corchete");

                    break;
                case "]":
                    this.Insertar(conca, "Caracter de cerrar corchete");

                    break;
                case "=":
                    this.Insertar(conca, "Operador asignacion");

                    break;
                case "&":
                    this.Insertar(conca, "Operador Condicional Y");

                    break;
                case "|":
                    this.Insertar(conca, "Operador Condicional O");

                    break;
                case "==":
                    this.Insertar(conca, "Operador de igualdad");

                    break;
                case "!=":
                    this.Insertar(conca, "Operador de desigualdad");

                    break;
                case "++":
                    this.Insertar(conca, "Operador Incrementar en 1");

                    break;
                case "--":
                    this.Insertar(conca, "Operador Disminuir en 1");

                    break;

                case "_":
                    this.Insertar(conca, "Guión bajo");

                    break;

                case "/*":
                    this.Insertar(conca, "Signo de apertura de comentario");
                    inicio = true;

                    break;
                case "*/":

                    this.Insertar(conca, "Signo de clausura de comentario");
                    fin = true;
                    break;


            }

        }
        //variable 
        String saliendo = "";
        private void PalabraReservada(string conca)
        {

            switch (conca.Trim())
            {
                case "pInicio":
                    this.Insertar(conca, "Palabra Reservada: Declara el inicio del programa.");

                    return;
                case "pFin":
                    this.Insertar(conca, "Palabra Reservada: Declara el fin del programa.");

                    return;
                case "pSwitch":
                    this.Insertar(conca, "Palabra Reservada: Sentencia que se utiliza para tomar decisiones múltiples.");

                    return;
                case "pIf":
                    this.Insertar(conca, "Palabra Reservada: Evalúa una condición y si el resultado es verdadero ejecuta las instrucciones a continuación.");

                    return;
                case "pElse":
                    this.Insertar(conca, "Palabra Reservada: Ejecuta la acción contraria");

                    return;
                case "pCase":
                    this.Insertar(conca, "Palabra Reservada: Indica el caso en una sentencia switch.");

                    return;
                case "pDefault":
                    this.Insertar(conca, "Palabra Reservada: Se usa para valores o correspondientes a los anteriores.");

                    return;
                case "pBreak":
                    this.Insertar(conca, "Palabra Reservada: Salto de sentencia.");

                    return;
                case "pWhile":
                    this.Insertar(conca, "Palabra Reservada: Repite la sentencia o bloques de sentencia.");

                    return;
                case "pFor":
                    this.Insertar(conca, "Palabra Reservada: Inicio del ciclo para, dará la cantidad de vueltas de acuerdo al rango asignado.");

                    return;
                case "pDo":
                    this.Insertar(conca, "Palabra Reservada: Hace que una instrucción se ejecute.");

                    return;
                case "pInt":
                    this.Insertar(conca, "Palabra Reservada: Valor entero.");

                    return;
                case "pFloat":
                    this.Insertar(conca, "Palabra Reservada: Es un tipo de dato numérico con decimal");

                    return;
                case "pDouble":
                    this.Insertar(conca, "Palabra Reservada: Representa un numero de 64 bits de precisión doble.");

                    return;
                case "pChar":
                    this.Insertar(conca, "Palabra Reservada: Indica que el tipo de dato es un carácter.");

                    return;
                case "pDecimal":
                    this.Insertar(conca, "Palabra Reservada: Son los valores que contienen decimales.");

                    return;
                case "pBool":
                    this.Insertar(conca, "Palabra Reservada: Es un valor que puede tener dos valores true y false.");

                    return;
                case "pString":
                    this.Insertar(conca, "Palabra Reservada: Indica que el tipo de dato es cadena.");

                    return;
                case "pShort":
                    this.Insertar(conca, "Palabra Reservada: Un tipo de dato numérico que va desde -32.768 a 32.767");

                    return;
                case "pMayus":
                    this.Insertar(conca, "Palabra Reservada: Convierte una cadena en Mayuscula");

                    return;
                case "pLength":
                    this.Insertar(conca, "Palabra Reservada: Nos sirve para verificar la cantidad de caracteres.");

                    return;

                case "pSqrt":
                    this.Insertar(conca, "Palabra Reservada: Devuelve la raíz cuadrada de un entero.");

                    return;

                case "pTrue":
                    this.Insertar(conca, "Palabra Reservada: Indica el valor verdadero de la variable del tipo booleano.");

                    return;

                case "pFalse":
                    this.Insertar(conca, "Palabra Reservada: Palabra Reservada: Indica el valor falso de la variable del tipo booleano.");

                    return;

                case "pMinus":
                    this.Insertar(conca, "Palabra Reservada: Devuelve una cadena en minusculas");

                    return;

                case "pRedondear":
                    this.Insertar(conca, "Palabra Reservada: Devuelve el valor entero de un numero con decimales");

                    return;
                case "print":
                    this.Insertar(conca, "Palabra Reservada: Para imprimir en pantalla.");
                    this.Simbolo(conca);
                    return;

                case "pSalir":
                    this.Insertar(conca, "Palabra Reservada: Se usa para salir de la consola.");

                    return;

                case "pLeer":
                    this.Insertar(conca, "Palabra Reservada: Función que sirve para leer una sentencia.");

                    return;

                case "pConvertpDecimal":
                    this.Insertar(conca, "Palabra Reservada: Método para convertir a decimal.");

                    return;

                case "pConvertpChar":
                    this.Insertar(conca, "Palabra Reservada: Método para convertir a char.");

                    return;

                case "pConvertpString":
                    this.Insertar(conca, "Palabra Reservada: Método para convertir a string.");

                    return;
                case "pConvertpInt":
                    this.Insertar(conca, "Palabra Reservada: Método para convertir a int.");

                    return;
                case "pFactorial":
                    this.Insertar(conca, "Palabra Reservada: Devuelve el Factorial de un numero");

                    return;

                case "pLog":
                    this.Insertar(conca, "Palabra Reservada: Funcion logarítmica");

                    return;

            }
            val(conca);
        }
        int[] indice = new int[100];
        String[] identificadores = new string[100];

        private void val(String conca)
        {
            int posicion = conca.IndexOf(" ");

            if (conca.IndexOf("pString ") != -1)
            {
               
                PalabraReservada("pString");


            }
            else
            {
                if (conca.IndexOf("pInt ") != -1)
                {
                    PalabraReservada("pInt");

                }
                else
                {
                    if (conca.IndexOf("pFloat ") != -1)
                    {
                        //this.Insertar("13", "pFloat", "Palabra Reservada: Es un tipo de dato numérico con decimal");
                        PalabraReservada("pFloat");
                    }
                    else
                    {
                        if (conca.IndexOf("pDecimal ") != -1)
                        {
                            PalabraReservada("pDecimal");

                        }
                        else
                        {
                            if (conca.IndexOf("pDouble ") != -1)
                            {
                                PalabraReservada("pDouble");

                            }
                            else
                            {
                                if (conca.IndexOf("pBool ") != -1)
                                {
                                    PalabraReservada("pBool");

                                }
                                else
                                {
                                    if (conca.IndexOf("pChar ") != -1)
                                    {
                                        PalabraReservada("pChar");

                                    }
                                    else
                                    {
                                        if (conca.IndexOf("pShort ") != -1)
                                        {
                                            PalabraReservada("pShort");

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            String conocido = "";
            int k = 0;
           string[] datos = new string[] { "pString", "pChar", "pInt","pDecimal","pFloat","pShort","pBool","pDouble" };
            string[] otros = new string[] { "pInicio", "pFin", "pIf", "pElse", "pSwitch", "pCase", "pDefault", "pBreak", "pWhile", "pFor", "pDo", "pDouble","pDecimal","pFactorial","pMayus", "pLength", "pSqrt", "pTrue", "pFalse", "pMinus", "print", "pSalir", "pLeer", "pConvertpDecimal", "pConvertpChar", "pConvertpString", "pConvertpInt", "pLog", "pRedondear" };
            string sinespacio = conca.TrimStart();
            
            if (conca.IndexOf('"') != -1 && conca.IndexOf('"') != -1)
            {
                saliendo = conca;
                this.Insertar('"' + conca.Trim(), "cadena de caracteres");
            }
            else
            {

                if (inicio == true && fin == true )//sinespacio.IndexOf("/*") == -1 && sinespacio.IndexOf("pString") != 0 && sinespacio.IndexOf("pInt") != 0 && sinespacio.IndexOf("pChar") != 0 && sinespacio.IndexOf("pDecimal") != 0 )
                {
                    inicio = false;
                    fin = false;
                    this.Insertar(conca, "Es un Comentario");
                   

                }
                else
                {
                    
                    if (conca.IndexOf("pString ") != 0 && conca.IndexOf("pChar ") != 0 && conca.IndexOf("pDecimal ") != 0 && conca.IndexOf("pFloat ") != 0 && conca.IndexOf("pDouble ") != 0 && conca.IndexOf("pBool ") != 0 && conca.IndexOf("pShort ") != 0 && conca.IndexOf("pInt ") != 0)
                    {

                        string m = "identificador";
                        foreach (string item in datos)
                       {
                            if (conca.IndexOf(item+" ") != -1)
                            {
                                m = "identificador";
                            }
                            else
                            {
                                //m = "identificador descono";
                                foreach (string  item2 in otros)
                                {
                                    if (conca.IndexOf(item2 + " ") != -1)
                                    {
                                        m = "identificador desconocido";
                                        
                                    }
                                }
                            }
                        } 
                    this.Insertar(conca.Trim(), m);
                }
                    else
                    {
                        if (conca.IndexOf(" ") != -1 && conca.IndexOf("/*") == -1 && sinespacio.IndexOf("/*") == -1 && conca.IndexOf('"') == -1)
                        {

                            this.Insertar(conca.Remove(0, posicion).Trim(), "Identificador");

                        }
                    }



                }

            }
        }
        private void Insertar(string Tokens, string Tipo)
        {
            this.dataGridView1.RowCount++;
                this.dataGridView1[0, this.dataGridView1.RowCount - 1].Value = Tokens;
                this.dataGridView1[1, this.dataGridView1.RowCount - 1].Value = Tipo;
            // this.dataGridView1[0, this.dataGridView1.RowCount - 1].Value = codigo;

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtentrada.Text = "";
            dataGridView1.Rows.Clear();
        }
        private void Analizar_Load(object sender, EventArgs e)
        {
            
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
        }

        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            Integrantes inte = new Integrantes();
            inte.ShowDialog();
        }

        private void btnPalabras_Click(object sender, EventArgs e)
        {
            PalabrasReservadas pal = new PalabrasReservadas();
            pal.ShowDialog();
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
