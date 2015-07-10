using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Kid_Icarus_Uprising_Save_Editor
{
    public partial class Form1 : Form
    {
        byte[] bytes;
        int size = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    bytes = File.ReadAllBytes(file);
                    size = bytes.Length;
                }
                catch (IOException)
                {
                    MessageBox.Show("You didn't import any file.");
                }

            }
            if (size == 66296)
            {

                heartsUpDown.Value = (int) bytes[488] + (int) bytes[489] * 256 + (int) bytes[490] * 256 * 256 + (int) bytes[491] * 256 * 256 * 256;
                int startval = 12;
                int notunlocked = 0;
                int unlocked = 0;
                int feather = 0;
                int hint = 0;
                for (int i = 0; i < 360; i++)
                {
                    if ((int)bytes[startval + i] == 0)
                    {
                        notunlocked++;
                    }
                    if ((int)bytes[startval + i] == 2)
                    {
                        unlocked++;
                    }
                    if ((int)bytes[startval + i] == 3)
                    {
                        feather++;
                    }
                    if ((int)bytes[startval + i] == 4)
                    {
                        hint++;
                    }
                }
                unlockedBox.Text = unlocked.ToString();
                hintBox.Text = hint.ToString();
                lockedBox.Text = notunlocked.ToString();
                featherBox.Text = feather.ToString();
                int palutenastartval = 492;
                palutenaBox.Value = bytes[palutenastartval] + bytes[palutenastartval + 1] * 256 + bytes[palutenastartval + 2] * 256 * 256 + bytes[palutenastartval + 3] * 256 * 256 * 256;
                viridiBox.Value = bytes[palutenastartval + 4] + bytes[palutenastartval + 5] * 256 + bytes[palutenastartval + 6] * 256 * 256 + bytes[palutenastartval + 7] * 256 * 256 * 256;
                int trophyval = 1132;
                int trophies = 0;
                for (int i = 0; i < 412; i++)
                {
                    if ((int)bytes[trophyval + i] > 0)
                    {
                        trophies++;
                    }
                }
                trophyBox.Text = trophies.ToString();
            }
            else
            {
                MessageBox.Show("You didn't import a valid save file.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int scorestartval = 65640;
            score.Value = bytes[scorestartval + comboBox1.SelectedIndex * 4] + bytes[scorestartval + comboBox1.SelectedIndex * 4 + 1] * 256 + bytes[scorestartval + comboBox1.SelectedIndex * 4 + 2] * 256 * 256 + bytes[scorestartval + comboBox1.SelectedIndex * 4 + 3] * 256 * 256 * 256;
            int enemiesstartval = 33648;
            enemies.Value=bytes[enemiesstartval+comboBox1.SelectedIndex*8] + bytes[enemiesstartval+comboBox1.SelectedIndex*8+1]*256 + bytes[enemiesstartval+comboBox1.SelectedIndex*8+2]*256*256 + bytes[enemiesstartval+comboBox1.SelectedIndex*8+3]*256*256*256;
            int difficultystartval = 65772;
            byte[] temparray = new byte[4];
            Array.Copy(bytes, difficultystartval+comboBox1.SelectedIndex * 4, temparray, 0, 4);
            float intensity = BitConverter.ToSingle(temparray, 0);
            textBox13.Text = intensity.ToString();
            difficulty.Value = (int) (intensity * 10);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox13.Text = ((float) difficulty.Value / 10).ToString();
            byte[] bytetemp = BitConverter.GetBytes((float) difficulty.Value/10);
            int difficultystartval = 65772;
            bytes[difficultystartval + comboBox1.SelectedIndex * 4] = bytetemp[0];
            bytes[difficultystartval + 1 + comboBox1.SelectedIndex * 4] = bytetemp[1];
            bytes[difficultystartval + 2 + comboBox1.SelectedIndex * 4] = bytetemp[2];
            bytes[difficultystartval + 3 + comboBox1.SelectedIndex * 4] = bytetemp[3];
            textBox1.Text = System.BitConverter.ToString(bytetemp);
            byte[] temparray = new byte[4];
            Array.Copy(bytes, difficultystartval + comboBox1.SelectedIndex * 4, temparray, 0, 4);
            float intensity = BitConverter.ToSingle(temparray, 0);
            textBox13.Text = intensity.ToString();
            difficulty.Value = (int)(intensity * 10);


        }

        private void statsNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            int startval = 65976;
            float val = bytes[startval + (int)statsNames.SelectedIndex * 4] + bytes[startval + (int)statsNames.SelectedIndex * 4 + 1] * 256 + bytes[startval + (int)statsNames.SelectedIndex * 4 + 2] * 256 * 256 + bytes[startval + (int)statsNames.SelectedIndex * 4 + 3] * 256 * 256 * 256;
            unit.Text = "units";
            statsVal.DecimalPlaces = 0;
            statsVal.Maximum = 9999999999;
            if ((new[] { 1, 2, 49, 50, 53, 57, 58 }).Contains(statsNames.SelectedIndex)) {
                val = val / 3600;
                unit.Text = "minutes";
            }
            if ((new[] { 24, 25 }).Contains(statsNames.SelectedIndex))
            {
                val = BitConverter.ToSingle(bytes, startval + statsNames.SelectedIndex * 4);
                statsVal.DecimalPlaces = 1;
                statsVal.Maximum = 9;
            }
            statsVal.Value = (decimal) val;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int startval = 12;
            for (int i = 0; i < 360; i++)
            {
                if ((int)bytes[startval + i] == 0 || (int)bytes[startval + i] == 4)
                {
                    bytes[startval + i] = 2;
                }
            }
            int notunlocked = 0;
            int unlocked = 0;
            int feather = 0;
            int hint = 0;
            for (int i = 0; i < 360; i++)
            {
                if ((int)bytes[startval + i] == 0)
                {
                    notunlocked++;
                }
                if ((int)bytes[startval + i] == 2)
                {
                    unlocked++;
                }
                if ((int)bytes[startval + i] == 3)
                {
                    feather++;
                }
                if ((int)bytes[startval + i] == 4)
                {
                    hint++;
                }
            }
            unlockedBox.Text = unlocked.ToString();
            hintBox.Text = hint.ToString();
            lockedBox.Text = notunlocked.ToString();
            featherBox.Text = feather.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int startval = 12;
            for (int i = 0; i < 360; i++)
            {
                if ((int)bytes[startval + i] == 0)
                {
                    bytes[startval + i] = 4;
                }
            }
            int notunlocked = 0;
            int unlocked = 0;
            int feather = 0;
            int hint = 0;
            for (int i = 0; i < 360; i++)
            {
                if ((int)bytes[startval + i] == 0)
                {
                    notunlocked++;
                }
                if ((int)bytes[startval + i] == 2)
                {
                    unlocked++;
                }
                if ((int)bytes[startval + i] == 3)
                {
                    feather++;
                }
                if ((int)bytes[startval + i] == 4)
                {
                    hint++;
                }
            }
            unlockedBox.Text = unlocked.ToString();
            hintBox.Text = hint.ToString();
            lockedBox.Text = notunlocked.ToString();
            featherBox.Text = feather.ToString();
        }

        private void featherButton_Click(object sender, EventArgs e)
        {
            int startval = 12;
            for (int i = 0; i < 360; i++)
            {
                if ((int)bytes[startval + i] == 3)
                {
                    bytes[startval + i] = 2;
                }
            }
            int notunlocked = 0;
            int unlocked = 0;
            int feather = 0;
            int hint = 0;
            for (int i = 0; i < 360; i++)
            {
                if ((int)bytes[startval + i] == 0)
                {
                    notunlocked++;
                }
                if ((int)bytes[startval + i] == 2)
                {
                    unlocked++;
                }
                if ((int)bytes[startval + i] == 3)
                {
                    feather++;
                }
                if ((int)bytes[startval + i] == 4)
                {
                    hint++;
                }
            }
            unlockedBox.Text = unlocked.ToString();
            hintBox.Text = hint.ToString();
            lockedBox.Text = notunlocked.ToString();
            featherBox.Text = feather.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int trophyval = 1132;
            int trophies = 0;
            for (int i = 0; i < 412; i++)
            {
                if ((int)bytes[trophyval + i] == 0)
                {
                    bytes[trophyval + i] = 1;
                }
            }
            for (int i = 0; i < 412; i++)
            {
                if ((int)bytes[trophyval + i] > 0)
                {
                    trophies++;
                }
            }
            trophyBox.Text = trophies.ToString();
        }

        private void neverReleased_Click(object sender, EventArgs e)
        {
            
            int trophies = 0;
            bytes[1538] = 1;
            bytes[1540] = 1;
            for (int i = 0; i < 412; i++)
            {
                if ((int)bytes[1132 + i] > 0)
                {
                    trophies++;
                }
            }
            trophyBox.Text = trophies.ToString();
        }

        private void unit_TextChanged(object sender, EventArgs e)
        {

        }

        private void heartsUpDown_ValueChanged(object sender, EventArgs e)
        {
            int startval = 488;
            bytes[startval+3] = (byte) Math.Floor(heartsUpDown.Value/16777216);
            bytes[startval + 2] = (byte) Math.Floor((heartsUpDown.Value % 16777216) / 65536);
            bytes[startval + 1] = (byte) Math.Floor(((heartsUpDown.Value % 16777216) % 65536) / 256);
            bytes[startval] = (byte) Math.Floor(((heartsUpDown.Value % 16777216) % 65536) % 256);
            heartsUpDown.Value = (int)bytes[488] + (int)bytes[489] * 256 + (int)bytes[490] * 256 * 256 + (int)bytes[491] * 256 * 256 * 256;

        }

        private void score_ValueChanged(object sender, EventArgs e)
        {
            int startval = 65640;
            bytes[startval + 3 + comboBox1.SelectedIndex * 4] = (byte)Math.Floor(score.Value / 16777216);
            bytes[startval + 2 + comboBox1.SelectedIndex * 4] = (byte)Math.Floor((score.Value % 16777216) / 65536);
            bytes[startval + 1 + comboBox1.SelectedIndex * 4] = (byte)Math.Floor(((score.Value % 16777216) % 65536) / 256);
            bytes[startval + comboBox1.SelectedIndex * 4] = (byte)Math.Floor(((score.Value % 16777216) % 65536) % 256);
            score.Value = (int)bytes[startval + comboBox1.SelectedIndex * 4] + (int)bytes[startval + 1 + comboBox1.SelectedIndex * 4] * 256 + (int)bytes[startval + 2 + comboBox1.SelectedIndex * 4] * 256 * 256 + (int)bytes[startval + 3 + comboBox1.SelectedIndex * 4] * 256 * 256 * 256;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int startval = 33648;
            bytes[startval + 3 +comboBox1.SelectedIndex * 8] = (byte)Math.Floor(enemies.Value / 16777216);
            bytes[startval + 2 + comboBox1.SelectedIndex * 8] = (byte)Math.Floor((enemies.Value % 16777216) / 65536);
            bytes[startval + 1 + comboBox1.SelectedIndex * 8] = (byte)Math.Floor(((enemies.Value % 16777216) % 65536) / 256);
            bytes[startval + comboBox1.SelectedIndex * 8] = (byte)Math.Floor(((enemies.Value % 16777216) % 65536) % 256);
            enemies.Value = (int)bytes[startval + comboBox1.SelectedIndex * 8] + (int)bytes[startval + 1 + comboBox1.SelectedIndex * 8] * 256 + (int)bytes[startval + 2 + comboBox1.SelectedIndex * 8] * 256 * 256 + (int)bytes[startval + 3 + comboBox1.SelectedIndex * 8] * 256 * 256 * 256;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int startval = 65976;
            int multiplier;
            if ((new[] { 1, 2, 49, 50, 53, 57, 58 }).Contains(statsNames.SelectedIndex))
            {
                multiplier = 3600;
            }
            else {
                multiplier = 1;
            }
            bytes[startval + 3 + statsNames.SelectedIndex * 4] = (byte)Math.Floor(statsVal.Value * multiplier / 16777216);
            bytes[startval + 2 + statsNames.SelectedIndex * 4] = (byte)Math.Floor((statsVal.Value * multiplier % 16777216) / 65536);
            bytes[startval + 1 + statsNames.SelectedIndex * 4] = (byte)Math.Floor(((statsVal.Value * multiplier % 16777216) % 65536) / 256);
            bytes[startval + statsNames.SelectedIndex * 4] = (byte)Math.Floor(((statsVal.Value * multiplier % 16777216) % 65536) % 256);
            
            if ((new[] { 24, 25 }).Contains(statsNames.SelectedIndex))
            {
                byte[] bytetemp = BitConverter.GetBytes((float)statsVal.Value);
                bytes[startval + statsNames.SelectedIndex * 4] = bytetemp[0];
                bytes[startval + 1 + statsNames.SelectedIndex * 4] = bytetemp[1];
                bytes[startval + 2 + statsNames.SelectedIndex * 4] = bytetemp[2];
                bytes[startval + 3 + statsNames.SelectedIndex * 4] = bytetemp[3];
            }
            float val = bytes[startval + (int)statsNames.SelectedIndex * 4] + bytes[startval + (int)statsNames.SelectedIndex * 4 + 1] * 256 + bytes[startval + (int)statsNames.SelectedIndex * 4 + 2] * 256 * 256 + bytes[startval + (int)statsNames.SelectedIndex * 4 + 3] * 256 * 256 * 256;
            unit.Text = "units";
            statsVal.DecimalPlaces = 0;
            statsVal.Maximum = 9999999999;
            if ((new[] { 1, 2, 49, 50, 53, 57, 58 }).Contains(statsNames.SelectedIndex))
            {
                val = val / 3600;
                unit.Text = "minutes";
            }
            if ((new[] { 24, 25 }).Contains(statsNames.SelectedIndex))
            {
                val = BitConverter.ToSingle(bytes, startval + statsNames.SelectedIndex * 4);
                statsVal.DecimalPlaces = 1;
                statsVal.Maximum = 9;
            }
            statsVal.Value = (decimal)val;
        }

        private void palutenaBox_ValueChanged(object sender, EventArgs e)
        {
            int startval = 492;
            bytes[startval + 3] = (byte)Math.Floor(palutenaBox.Value / 16777216);
            bytes[startval + 2] = (byte)Math.Floor((palutenaBox.Value % 16777216) / 65536);
            bytes[startval + 1] = (byte)Math.Floor(((palutenaBox.Value % 16777216) % 65536) / 256);
            bytes[startval] = (byte)Math.Floor(((palutenaBox.Value % 16777216) % 65536) % 256);
            palutenaBox.Value = (int)bytes[startval] + (int)bytes[startval + 1] * 256 + (int)bytes[startval + 2] * 256 * 256 + (int)bytes[startval + 3] * 256 * 256 * 256;
        }

        private void viridiBox_ValueChanged(object sender, EventArgs e)
        {
            int startval = 496;
            bytes[startval + 3] = (byte)Math.Floor(viridiBox.Value / 16777216);
            bytes[startval + 2] = (byte)Math.Floor((viridiBox.Value % 16777216) / 65536);
            bytes[startval + 1] = (byte)Math.Floor(((viridiBox.Value % 16777216) % 65536) / 256);
            bytes[startval] = (byte)Math.Floor(((viridiBox.Value % 16777216) % 65536) % 256);
            viridiBox.Value = (int)bytes[startval] + (int)bytes[startval + 1] * 256 + (int)bytes[startval + 2] * 256 * 256 + (int)bytes[startval + 3] * 256 * 256 * 256;
        }
    }
}
