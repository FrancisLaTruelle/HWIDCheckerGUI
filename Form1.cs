using System;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            update();
        }

        private void update()
        {
            ManagementObjectSearcher searcherWin32_Processor = new ManagementObjectSearcher("SELECT Name, SystemName, ProcessorId, SerialNumber FROM Win32_Processor");
            ManagementObjectSearcher searcherWin32_CsProduct = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct");
            ManagementObjectSearcher searcherWin32_OperatingSystem = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcherWin32_DiskDrive = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive");
            ManagementObjectSearcher searcherWin32_LogicalDisk = new ManagementObjectSearcher("SELECT VolumeSerialNumber, Name FROM Win32_LogicalDisk");
            ManagementObjectSearcher searcherWin32_BaseBoard = new ManagementObjectSearcher("SELECT SerialNumber, Product, Version FROM Win32_BaseBoard");
            ManagementObjectSearcher searcherWin32_NetworkAdapter = new ManagementObjectSearcher("SELECT Name, MACAddress FROM Win32_NetworkAdapter");
            ManagementObjectSearcher searcherWin32_BIOS = new ManagementObjectSearcher("SELECT Name, Version, SerialNumber FROM Win32_BIOS");

            foreach (ManagementObject info in searcherWin32_Processor.Get())
            {
                label2.Text = "Name: " + info["Name"].ToString() + "\nSystem Name: " + info["SystemName"].ToString() + "\nProcessor ID: " + info["ProcessorId"].ToString() + "\nSerial Number: " + info["SerialNumber"].ToString();
            }

            foreach (ManagementObject info in searcherWin32_CsProduct.Get())
            {
                label7.Text = "UUID: " + info["UUID"].ToString();
            }

            foreach (ManagementObject info in searcherWin32_OperatingSystem.Get())
            {
                label7.Text = label7.Text + "\nProduct ID: " + info["SerialNumber"].ToString();
            }

            RegistryKey keyBaseX64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey keyBaseX86 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey keyX64 = keyBaseX64.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", RegistryKeyPermissionCheck.ReadSubTree);
            RegistryKey keyX86 = keyBaseX86.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", RegistryKeyPermissionCheck.ReadSubTree);
            object resultObjX64 = keyX64.GetValue("MachineGuid", (object)"default");
            object resultObjX86 = keyX86.GetValue("MachineGuid", (object)"default");
            if (resultObjX64 != null && resultObjX64.ToString() != "default")
            {
                label7.Text = label7.Text + "\nHWID: " + resultObjX64.ToString();
            }
            if (resultObjX86 != null && resultObjX86.ToString() != "default")
            {
                label7.Text = label7.Text + "\nHWID: " + resultObjX86.ToString();
            }


            label11.Text = "";
            int countDD = 1;
            foreach (ManagementObject info in searcherWin32_DiskDrive.Get())
            {
                string[] serialDD = info["SerialNumber"].ToString().Split(new Char[] { '\n' });
                label11.Text = label11.Text + $"Serial Number ({countDD++}): {serialDD[0].Replace(" ", "")}\n";
            }

            foreach (ManagementObject info in searcherWin32_LogicalDisk.Get())
            {
                string[] serialLD = info["VolumeSerialNumber"].ToString().Split(new Char[] { '\n' });
                string[] nameLD = info["Name"].ToString().Split(new Char[] { '\n' });
                label11.Text = label11.Text + $"\nVolume ID ({nameLD[0]}): {serialLD[0].Replace(" ", "")}";
            }

            foreach (ManagementObject info in searcherWin32_BaseBoard.Get())
            {
                label3.Text = "Product: " + info["Product"].ToString() + "\nSerial Number: " + info["SerialNumber"].ToString() + "\nVersion: " + info["Version"].ToString();
            }

            label5.Text = "";
            foreach (ManagementObject info in searcherWin32_NetworkAdapter.Get())
            {
                object macNA = info["MACAddress"];
                if (!(macNA == null))
                {
                    string[] nameNA = info["Name"].ToString().Split(new Char[] { '\n' });
                    label5.Text = label5.Text + $"{nameNA[0]}: {macNA} \n";
                }
            }

            foreach (ManagementObject info in searcherWin32_BIOS.Get())
            {
                label14.Text = "Name: " + info["Name"].ToString() + "\nVersion: " + info["Version"].ToString() + "\nSerial Number: " + info["SerialNumber"].ToString();
            }

        }

        // Close window
        private void button1_Click(object sender, EventArgs e)
        {
             Application.Exit();
        }

        // Can moove window
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            update();
            button2.Text = "Updated !";
            Application.DoEvents();
            System.Threading.Thread.Sleep(3000);
            button2.Text = "Refresh";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String copy;
            copy = "HWID Checker v1\nCPU\n" + label2.Text + "\n\nBasebord\n" + label3.Text + "\n\nDisk Drive\n" + label11.Text + "\n\nBIOS\n" + label14.Text + "\n\nNetwork\n" + label5.Text + "\n\nOther\n" + label7.Text;
            Clipboard.SetText(copy);
            MessageBox.Show(copy);
        }

        private void label13_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
