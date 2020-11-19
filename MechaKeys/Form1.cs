using System;
using System.IO;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using NAudio.Wave;
using Microsoft.Win32;
using System.Threading;

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
        public void searchFile(string textBox)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox = fileName;
            }
        }
        public void playSound(string textbox)
        {
            //Wait(2);
            IWavePlayer waveOutDevice = new WaveOut();
            AudioFileReader audioFileReader = new AudioFileReader($"{textbox}");
            waveOutDevice.Init(audioFileReader);

            float userVal;
            if (float.TryParse(textBox6.Text, out userVal))
            {
                // skeetit
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
                    // better than if else if else
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                                ThreadPool.QueueUserWorkItem(yes => playSound(textBox4.Text));
                            break;
                        case Keys.Delete:
                                ThreadPool.QueueUserWorkItem(yes => playSound(textBox5.Text));
                            break;
                        default:
                            if (e.Control)
                            {
                                // little fix, don't kill me again
                            }
                            else if (!e.Shift)
                            {
                                ThreadPool.QueueUserWorkItem(yes => playSound(textBox1.Text));
                            }
                            break;
                    }
                }
                catch
                {
                    MessageBox.Show("Uuuups error getting the dir");
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBox6.Text = textBox6.Text.Remove(textBox6.Text.Length - 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //searchFile(textBox1.Text);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox1.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox1.Text = fileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //searchFile(textBox4.Text);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox4.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox4.Text = fileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = textBox5.Text;
            openFileDialog1.Filter = "mp3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox5.Text = fileName;
            }
            //searchFile(textBox5.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.checkbox = checkBox1.Checked;
            Properties.Settings.Default.checkbox2 = checkBox2.Checked;
            Properties.Settings.Default.textbox1 = textBox1.Text;
            Properties.Settings.Default.textbox4 = textBox4.Text;
            Properties.Settings.Default.textbox5 = textBox5.Text;
            Properties.Settings.Default.textbox6 = textBox6.Text;
            Properties.Settings.Default.Save();
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
        }

        private void MechaKeys_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Properties.Settings.Default.checkbox;
            checkBox2.Checked = Properties.Settings.Default.checkbox2;
            textBox1.Text = Properties.Settings.Default.textbox1;
            textBox4.Text = Properties.Settings.Default.textbox4;
            textBox5.Text = Properties.Settings.Default.textbox5;
            textBox6.Text = Properties.Settings.Default.textbox6;
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

        private void keysimulatornotify_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            keysimulatornotify.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
        }
    }
}
