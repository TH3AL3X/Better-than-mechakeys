using System;
using System.IO;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using NAudio.Wave;
using Microsoft.Win32;
using System.Threading;
using System.Runtime.InteropServices;

namespace keysimulator
{
    public partial class MechaKeys : Form
    {
        //Seriously someone is selling a program like this, pathetic....
        private IKeyboardMouseEvents m_GlobalHook;
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

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

        public void playSound(string textbox)
        {
            IWavePlayer waveOutDevice = new WaveOut();
            AudioFileReader audioFileReader = new AudioFileReader($"{textbox}");
            waveOutDevice.Init(audioFileReader);
            // lol don't kill me pls XDDDDDDDDDDDDDDDDDD
            float userVal;
            if (float.TryParse(trackBar1.Value.ToString(), out userVal))
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
                                // little fix for no ear rapping with shift and control, don't kill me again
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
            Properties.Settings.Default.trackbar1 = trackBar1.Value;
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
            trackBar1.Value = Properties.Settings.Default.trackbar1;

            // another fix, i'm retarded :-)
            label6.Text = $"{trackBar1.Value} % volume";
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // la meme
            label6.Text = $"{trackBar1.Value} % volume";

            int volumeMixed = ((ushort.MaxValue / 10) * trackBar1.Value);
            uint setVolume = (((uint)volumeMixed & 0x0000ffff) | ((uint)volumeMixed << 16));
            waveOutSetVolume(IntPtr.Zero, setVolume);
        }
    }
}
