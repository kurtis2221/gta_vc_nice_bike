using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace gta_vc_nice_bike
{
    public partial class Form1 : Form
    {
        struct Vector3
        {
            public float x, y, z;

            public Vector3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        SndPlayer snd_nice_bike;
        SndPlayer snd_no_my_bike;
        SndPlayer snd_miscom;

        const string game_exe = "gta-vc";
        const string cfg_file = "gta_vc_nice_bike.ini";
        const uint player_ptr = 0x94AD28;

        uint player_addr;
        uint last_vehicle_addr;
        uint last_driver_addr;

        uint[] driver_conditions;
        uint[] vehicle_conditions;

        bool stolen_vehicle;
        bool mission_complete;

        bool setting_nice_bike;
        bool setting_no_my_bike;
        bool setting_miscom;

        MemoryEdit.Memory mem;
        Process game;

        public Form1()
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            if (!LoadSettings(() => new StreamReader(cfg_file, Encoding.Default)))
                if (!LoadSettings(() => new StringReader(Properties.Resources.gta_vc_nice_bike)))
                    Environment.Exit(0);
            mem = new MemoryEdit.Memory();
            InitializeComponent();
            tmr_scan.Enabled = true;
            tmr_bike.Enabled = true;
        }

        private bool LoadSettings(Func<TextReader> hndl)
        {
            try
            {
                using (TextReader sr = hndl())
                {
                    snd_nice_bike = new SndPlayer(sr.ReadLine());
                    snd_no_my_bike = new SndPlayer(sr.ReadLine());
                    snd_miscom = new SndPlayer(sr.ReadLine());
                    //Volume
                    float snd_vol = float.Parse(sr.ReadLine());
                    float mus_vol = float.Parse(sr.ReadLine());
                    snd_nice_bike.SetVolume(snd_vol);
                    snd_no_my_bike.SetVolume(snd_vol);
                    snd_miscom.SetVolume(mus_vol);
                    //Conditions
                    string line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line)) vehicle_conditions = Array.ConvertAll(line.Split(','), x => uint.Parse(x));
                    else vehicle_conditions = new uint[0];
                    line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line)) driver_conditions = Array.ConvertAll(line.Split(','), x => uint.Parse(x));
                    else driver_conditions = new uint[0];
                    //Settings
                    string settings = (sr.ReadLine() ?? "111").PadRight(4, '1');
                    setting_nice_bike = settings[0] != '0';
                    setting_no_my_bike = settings[1] != '0';
                    setting_miscom = settings[2] != '0';
                    //Nice bike sound play test
                    snd_nice_bike.Play();
                }
            }
            catch (FileNotFoundException)
            {
                try
                {
                    File.WriteAllText(cfg_file, Properties.Resources.gta_vc_nice_bike, Encoding.Default);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ScanForGame()
        {
            Process[] procs = Process.GetProcessesByName(game_exe);
            if (procs.Length > 0)
            {
                game = procs[0];
                mem.Attach((uint)game.Id, MemoryEdit.Memory.ProcessAccessFlags.VirtualMemoryRead);
            }
        }

        private void tmr_scan_Tick(object sender, EventArgs e)
        {
            if (game == null || game.HasExited) ScanForGame();
        }

        private void tmr_bike_Tick(object sender, EventArgs e)
        {
            if (game == null) return;
            NiceBike();
        }

        private void NiceBike()
        {
            player_addr = mem.Read(player_ptr);
            //0x3A8 - last controlled vehicle/vehicle about to be entered
            uint vehicle_addr = mem.Read(player_addr + 0x3A8);
            //0x18 = running to enter vehicle
            //0x3A = entering vehicle
            uint player_state = mem.Read(player_addr + 0x244);
            bool different_vehicle = last_vehicle_addr != vehicle_addr;
            uint vehicle_model = mem.Read(vehicle_addr + 0x5C);
            bool vehicle_cond_met = vehicle_conditions.Length == 0 || vehicle_conditions.Contains(vehicle_model);
            if (different_vehicle
                && (player_state == 0x18 || player_state == 0x3A)
                && vehicle_cond_met)
            {
                last_vehicle_addr = vehicle_addr;
                if (setting_nice_bike) snd_nice_bike.Play();
            }
            //
            uint driver_addr = mem.Read(vehicle_addr + 0x1A8);
            uint driver_state = mem.Read(driver_addr + 0x244);
            uint driver_model = mem.Read(driver_addr + 0x5C);
            bool not_player_driver = driver_addr != player_addr;
            bool different_driver = last_driver_addr != driver_addr;
            bool driver_cond_met = driver_conditions.Length == 0 || driver_conditions.Contains(driver_model);
            //vehicle_model == 401 && driver_model == 51
            //51, WMYST
            //0x39 = getting jacked by being pulled out
            //0x3C = exiting vehicle
            if (different_driver && not_player_driver
                && (driver_state == 0x39 || driver_state == 0x3C)
                && driver_cond_met)
            {
                last_driver_addr = driver_addr;
                if (setting_no_my_bike) snd_no_my_bike.Play();
                stolen_vehicle = true;
                mission_complete = false;
            }
            if (!setting_miscom) return;
            //0x32 = sitting in vehicle
            if (stolen_vehicle && !mission_complete && player_state == 0x32)
            {
                Vector3 pos1 = GetPos(player_addr);
                Vector3 pos2 = GetPos(last_driver_addr);
                float dist = (float)Math.Sqrt(Math.Pow(pos1.x - pos2.x, 2) + Math.Pow(pos1.y - pos2.y, 2) + Math.Pow(pos1.z - pos2.z, 2));
                if (dist > 30.0f)
                {
                    stolen_vehicle = false;
                    mission_complete = true;
                    snd_miscom.Play();
                }
            }
        }

        private Vector3 GetPos(uint addr)
        {
            uint pos_ptr = addr + 0x34;
            return new Vector3
            (
                mem.ReadFloat(pos_ptr),
                mem.ReadFloat(pos_ptr + 0x4),
                mem.ReadFloat(pos_ptr + 0x8)
            );
        }

        private void bt_about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program written by Kurtis (2024)\nWritten in Visual C# 2022 (.NET 4.5.2)", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
