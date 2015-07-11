using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Kid_Icarus_Uprising_Save_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SaveGame Save; // SaveGame storage.
        public struct SaveGame
        {
            public byte[] Data;
            public bool Valid;
            public int Palutena, Viridi, Hearts, Enemy, Score, Difficulty, Trophy, Stats, Achievements;

            // Savedata Map
            public SaveGame(byte[] data)
            {
                Data = data;
                Valid = Data.Length == 66296;
                Achievements = 12;
                Hearts = 488;
                Palutena = 492;
                Viridi = 496;
                Trophy = 1132;
                Enemy = 33648;
                Score = 65640;
                Difficulty = 65772;
                Stats = 65976;
            }
        }

        private void importFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() != DialogResult.OK) // Test result.
                return;
            try
            {
                Save = new SaveGame(File.ReadAllBytes(openFileDialog1.FileName));
            }
            catch (IOException)
            {
                MessageBox.Show("You didn't import any file.");
            }

            if (!Save.Valid)
            {
                MessageBox.Show("You didn't import a valid save file.");
                return;
            }

            // Load save contents.
            heartsUpDown.Value = BitConverter.ToInt32(Save.Data, Save.Hearts);
            updateAchievements();

            palutenaBox.Value = BitConverter.ToInt32(Save.Data, Save.Palutena);
            viridiBox.Value = BitConverter.ToInt32(Save.Data, Save.Viridi);

            countTrophies();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int scoreOffset = Save.Score + comboBox_Chapter.SelectedIndex*4;
            score.Value = BitConverter.ToInt32(Save.Data, scoreOffset);
            int enemyOffset = Save.Enemy + comboBox_Chapter.SelectedIndex*8;
            enemies.Value = BitConverter.ToInt32(Save.Data, enemyOffset);
            int difficultyOffset = Save.Difficulty + comboBox_Chapter.SelectedIndex*4;
            float intensity = BitConverter.ToSingle(Save.Data, difficultyOffset);
            textBox13.Text = intensity.ToString();
            difficulty.Value = (int)(intensity * 10);
        }
        
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            textBox13.Text = ((float)difficulty.Value / 10).ToString();
            int difficultyOffset = Save.Difficulty + comboBox_Chapter.SelectedIndex * 4;
            Array.Copy(BitConverter.GetBytes((float)difficulty.Value / 10), 0, Save.Data, difficultyOffset, 4);
            float intensity = BitConverter.ToSingle(Save.Data, difficultyOffset);
            textBox13.Text = intensity.ToString();
            difficulty.Value = (int)(intensity * 10);
        }

        private void statsNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int offset = Save.Stats + statsNames.SelectedIndex*4;
            float val = BitConverter.ToInt32(Save.Data, offset);
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
                val = BitConverter.ToSingle(Save.Data, offset);
                statsVal.DecimalPlaces = 1;
                statsVal.Maximum = 9;
            }
            statsVal.Value = (decimal)val;
        }

        private void unlockAll_Click(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int offset = Save.Achievements;
            for (int i = 0; i < 360; i++)
            {
                if (Save.Data[offset + i] == 0 || Save.Data[offset + i] == 4)
                {
                    Save.Data[offset + i] = 2;
                }
            }
            updateAchievements();
        }
        private void hideButton_Click(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int offset = Save.Achievements;
            for (int i = 0; i < 360; i++)
            {
                if (Save.Data[offset + i] == 0)
                {
                    Save.Data[offset + i] = 4;
                }
            }
            updateAchievements();
        }
        private void featherButton_Click(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int offset = Save.Achievements;
            for (int i = 0; i < 360; i++)
            {
                if (Save.Data[offset + i] == 3)
                {
                    Save.Data[offset + i] = 2;
                }
            }
            updateAchievements();
        }

        private void updateAchievements()
        {
            int offset = Save.Achievements;
            int notunlocked = 0;
            int unlocked = 0;
            int feather = 0;
            int hint = 0;
            for (int i = 0; i < 360; i++)
            {
                switch (Save.Data[offset + i])
                {
                    case 0:
                        notunlocked++;
                        break;
                    case 2:
                        unlocked++;
                        break;
                    case 3:
                        feather++;
                        break;
                    case 4:
                        hint++;
                        break;
                }
            }
            unlockedBox.Text = unlocked.ToString();
            hintBox.Text = hint.ToString();
            lockedBox.Text = notunlocked.ToString();
            featherBox.Text = feather.ToString();
        }

        private void unlockTrophies_Click(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            for (int i = 0; i < 412; i++)
            {
                if (Save.Data[Save.Trophy + i] == 0)
                {
                    Save.Data[Save.Trophy + i] = 1;
                }
            }
            countTrophies();
        }

        private void neverReleased_Click(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            Save.Data[1538] = 1;
            Save.Data[1540] = 1;
            countTrophies();
        }
        private void countTrophies()
        {
            int trophies = 0;
            for (int i = 0; i < 412; i++)
            {
                if (Save.Data[Save.Trophy + i] > 0)
                {
                    trophies++;
                }
            }
            trophyBox.Text = trophies.ToString();
        }
        
        private void heartsUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            Array.Copy(BitConverter.GetBytes((int)heartsUpDown.Value), 0, Save.Data, Save.Hearts, 4);
            heartsUpDown.Value = BitConverter.ToInt32(Save.Data, Save.Hearts);
        }

        private void score_ValueChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int offset = Save.Score + comboBox_Chapter.SelectedIndex * 4;
            Array.Copy(BitConverter.GetBytes((int)score.Value), 0, Save.Data, offset, 4);
            score.Value = BitConverter.ToInt32(Save.Data, offset);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int offset = Save.Enemy + comboBox_Chapter.SelectedIndex * 8;
            Array.Copy(BitConverter.GetBytes((int)enemies.Value), 0, Save.Data, offset, 4);
            enemies.Value = BitConverter.ToInt32(Save.Data, offset);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            int offset = Save.Stats + statsNames.SelectedIndex * 4;
            int multiplier = (new[] { 1, 2, 49, 50, 53, 57, 58 }).Contains(statsNames.SelectedIndex) ? 3600 : 1;

            Array.Copy(BitConverter.GetBytes((int)statsVal.Value * multiplier), 0, Save.Data, offset, 4);

            if ((new[] { 24, 25 }).Contains(statsNames.SelectedIndex))
            {
                Array.Copy(BitConverter.GetBytes((int)statsVal.Value), 0, Save.Data, offset, 4);
            }

            float val = BitConverter.ToInt32(Save.Data, offset);
            unit.Text = "units";
            statsVal.DecimalPlaces = 0;
            statsVal.Maximum = 9999999999;
            if ((new[] { 1, 2, 49, 50, 53, 57, 58 }).Contains(statsNames.SelectedIndex))
            {
                val /= 3600;
                unit.Text = "minutes";
            }
            if ((new[] { 24, 25 }).Contains(statsNames.SelectedIndex))
            {
                val = BitConverter.ToSingle(Save.Data, offset);
                statsVal.DecimalPlaces = 1;
                statsVal.Maximum = 9;
            }
            statsVal.Value = (decimal)val;
        }

        private void palutenaBox_ValueChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            Array.Copy(BitConverter.GetBytes((int)palutenaBox.Value), 0, Save.Data, Save.Palutena, 4);
            palutenaBox.Value = BitConverter.ToInt32(Save.Data, Save.Palutena);
        }

        private void viridiBox_ValueChanged(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            Array.Copy(BitConverter.GetBytes((int)viridiBox.Value), 0, Save.Data, Save.Viridi, 4);
            viridiBox.Value = BitConverter.ToInt32(Save.Data, Save.Viridi);
        }

        private void saveFile_Click(object sender, EventArgs e)
        {
            if (!Save.Valid) return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                FileName = "s01.sav"
            };
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) 
                return;

            try
            {
                File.WriteAllBytes(saveFileDialog1.FileName, Save.Data);
            }
            catch (IOException)
            {
                MessageBox.Show("You didn't save any file.");
            }
        }
    }
}
