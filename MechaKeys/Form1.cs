using System;
using System.IO;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using NAudio.Wave;
using Microsoft.Win32;

namespace keysimulator
{
    public partial class MechaKeys : Form
    {
        //Seriously someone is selling a program like this, pathetic....
        private IKeyboardMouseEvents m_GlobalHook;
        static bool niggadontcrash;
        public MechaKeys()
        {
            InitializeComponent();
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += KeyDown;
            m_GlobalHook.KeyPress += KeyPress;
            m_GlobalHook.KeyUp += KeyUp;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        public void Wait(int ms)
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < ms)
                Application.DoEvents();
        }

        public void playSound(string textbox)
        {
            Wait(2);
            IWavePlayer waveOutDevice = new WaveOut();
            AudioFileReader audioFileReader = new AudioFileReader($"{textbox}");
            //audioFileReader.Volume = int.Parse(textBox6.Text);
            waveOutDevice.Init(audioFileReader);

            float userVal;
            if (float.TryParse(textBox6.Text, out userVal))
            {
            }

            float volumenfix = 0.00f + userVal;
            audioFileReader.Volume = volumenfix;
            waveOutDevice.Play();
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            niggadontcrash = true;
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            niggadontcrash = false;
        }
        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (checkBox1.Checked && niggadontcrash == false)
            {
                try
                {
                    if (e.KeyCode == Keys.Alt)
                    {
                        playSound(textBox2.Text);
                    }
                    else if (e.KeyCode == Keys.Shift)
                    {
                        playSound(textBox3.Text);
                    }
                    else if (e.KeyCode == Keys.Enter)
                    {
                        playSound(textBox4.Text);
                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        playSound(textBox5.Text);
                    }
                    else
                    {
                        playSound(textBox1.Text);
                    }
                }
                catch
                {
                    MessageBox.Show("Uuuups error getting the dir");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBox6.Text = textBox6.Text.Remove(textBox6.Text.Length - 1);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox1.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox1.Text = fileName;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox2.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox2.Text = fileName;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox3.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox3.Text = fileName;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox4.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox4.Text = fileName;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox5.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox5.Text = fileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void MechaKeys_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_GlobalHook.KeyDown -= KeyDown;
            m_GlobalHook.KeyPress -= KeyPress;
            m_GlobalHook.KeyUp -= KeyUp;
            m_GlobalHook.Dispose();

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (checkBox2.Checked)
                rk.SetValue("keysimulator", Application.ExecutablePath);
            else
                rk.DeleteValue("keysimulator", false);

            Properties.Settings.Default.checkbox = checkBox1.Checked;
            Properties.Settings.Default.checkbox2 = checkBox2.Checked;
            Properties.Settings.Default.textbox1 = textBox1.Text;
            Properties.Settings.Default.textbox2 = textBox2.Text;
            Properties.Settings.Default.textbox3 = textBox3.Text;
            Properties.Settings.Default.textbox4 = textBox4.Text;
            Properties.Settings.Default.textbox5 = textBox5.Text;
            Properties.Settings.Default.textbox6 = textBox6.Text;
            Properties.Settings.Default.Save();
        }

        private void MechaKeys_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Properties.Settings.Default.checkbox;
            checkBox2.Checked = Properties.Settings.Default.checkbox2;
            textBox1.Text = Properties.Settings.Default.textbox1;
            textBox2.Text = Properties.Settings.Default.textbox2;
            textBox3.Text = Properties.Settings.Default.textbox3;
            textBox4.Text = Properties.Settings.Default.textbox4;
            textBox5.Text = Properties.Settings.Default.textbox5;
            textBox6.Text = Properties.Settings.Default.textbox6;
            Properties.Settings.Default.Reset();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MechaKeys_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                keysimulatornotify.Visible = true;
                keysimulatornotify.ShowBalloonTip(1000);
            }
        }

        private void MechaKeys_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            keysimulatornotify.Visible = false;
        }

        private void keysimulatornotify_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            keysimulatornotify.Visible = false;
        }

        private void thirteenTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
