using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// 1. Import the InteropServices type
using System.Runtime.InteropServices;

namespace WindowSeach
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        public bool IsVisable = true;

        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.DimGray;
            this.TransparencyKey = Color.DimGray;

            InitializeComponent();

            int UniqueHotkeyId = 1;
            int HotKeyCode = (int)Keys.Space;
            Boolean F9Registered = RegisterHotKey(this.Handle, UniqueHotkeyId, 0x0002, HotKeyCode);

            // 4. Verify if the hotkey was succesfully registered, if not, show message in the console
            if (F9Registered)
            {
                Console.WriteLine("Global Hotkey F9 was succesfully registered");
            }
            else
            {
                Console.WriteLine("Global Hotkey F9 couldn't be registered !");
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();

                if (id == 1)
                {
                    if (IsVisable)
                    {
                        this.Hide();
                    }
                    else
                    {
                        this.Show();
                    }

                    IsVisable = !IsVisable;
                }
            }
            base.WndProc(ref m);
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
