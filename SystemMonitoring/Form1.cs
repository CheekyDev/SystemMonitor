using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Runtime.InteropServices;

namespace SystemMonitoring
{
    public partial class Form1 : MetroForm
    {
        private float cpu;
        private float ram;
        private ulong installedMemory;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MEMORYSTATUSEX mEMORYSTATUSEX = new MEMORYSTATUSEX();

            if(GlobalMemoryStatusEx(mEMORYSTATUSEX))
            {
                installedMemory = mEMORYSTATUSEX.ullTotalPhys;
            }

            metroLabel8.Text = Convert.ToString(installedMemory / 1000000000) + " Гб";
            //Console.WriteLine(installedMemory);
            timer1.Interval = 1000;

            timer1.Start();
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

        private class MEMORYSTATUSEX
        {
            public uint dwLenght;
            public uint dwMemoryLenght;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPagefile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.dwLenght = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        private void timer1_Tick(object sender, EventArgs e)
        {
            cpu = performanceCPU.NextValue();
            ram = performanceRAM.NextValue();

            metroProgressBar1.Value = (int)cpu;
            metroProgressBar2.Value = (int)ram;

            metroLabel2.Text = Convert.ToString(Math.Round(cpu, 1)) + " %";
            metroLabel3.Text = Convert.ToString(Math.Round(ram, 1)) + " %"; //11, 9

            metroLabel11.Text = Convert.ToString(Math.Round((ram / 100 * installedMemory) / 1000000000, 1)) + " Гб";
            metroLabel9.Text = Convert.ToString(Math.Round((installedMemory - ram / 100 * installedMemory) / 1000000000, 1)) + " Гб";
            //Console.WriteLine(ram);

            chart1.Series["ЦП"].Points.AddY(cpu);
            chart1.Series["ОЗУ"].Points.AddY(ram);
        }
    }
}
