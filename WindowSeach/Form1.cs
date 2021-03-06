﻿using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowSeach
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.DimGray;
            this.TransparencyKey = Color.DimGray;

            InitializeComponent();
            SetupHideAndShowHotKeys();

            var UniqueHotkeyId = 2;
            var HotKeyCode = (int)Keys.Enter;
            var ctrlModifier = 0x0000;
            var CtrlSpacebar = RegisterHotKey(this.Handle, UniqueHotkeyId, ctrlModifier, HotKeyCode);

            if (CtrlSpacebar)
            {
                Console.WriteLine("Global Hotkey F9 was succesfully registered");
            }
            else
            {
                Console.WriteLine("Global Hotkey F9 couldn't be registered !");
            }

            if (listBox1.Items.Count < 1)
            {
                listBox1.Hide();
            }
        }

        private void SetupHideAndShowHotKeys()
        {
            var UniqueHotkeyId = 1;
            var HotKeyCode = (int)Keys.Space;
            var ctrlModifier = 0x0002;
            var CtrlSpacebar = RegisterHotKey(this.Handle, UniqueHotkeyId, ctrlModifier, HotKeyCode);

            if (CtrlSpacebar)
            {
                Console.WriteLine("Global Hotkey F9 was succesfully registered");
            }
            else
            {
                Console.WriteLine("Global Hotkey F9 couldn't be registered !");
            }
        }

        private bool _isVisable = true;

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 0x0312)
            {
                int id = message.WParam.ToInt32();

                if (id == 1)
                {
                    if (_isVisable)
                    {
                        this.Hide();
                    }
                    else
                    {
                        this.Show();
                    }

                    _isVisable = !_isVisable;
                }

                if (id == 2)
                {
                    if (textBox1.Text.Contains("clear"))
                    {
                        listBox1.Items.Clear();
                    }

                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    GetSubDirectories(path, textBox1.Text);

                    if (listBox1.Items.Count > 0)
                    {
                        listBox1.Show();
                    }
                    else
                    {
                        listBox1.Hide();
                    }
                }
            }

            base.WndProc(ref message);
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

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

        public void GetSubDirectories(string dirctoryName, string search)
        {
            try
            {
                var directories = Directory.GetDirectories(dirctoryName, search, SearchOption.TopDirectoryOnly);
                foreach (var directory in directories)
                {
                    listBox1.Items.Add(directory);
                    GetSubDirectories(directory, search);
                }
            }
            catch
            {
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}
