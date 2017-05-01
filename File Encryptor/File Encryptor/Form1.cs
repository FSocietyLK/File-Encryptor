using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Encryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 6; i++)
            {
                dataGridView1.Rows.Add();
            }
            defaultDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\File Encryptor\\";
            Directory.CreateDirectory(defaultDir);
            openFileDialog1.InitialDirectory = defaultDir;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All Files|*.*";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int count = 0;
                bool exist = false;
                string[] fileList = openFileDialog1.FileNames;
                dataGridView1.AllowUserToAddRows = false;
                for (int i = dataGridView1.Rows.Count - 1; i > -1; --i)
                {
                    foreach (String file in fileList)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        if (!row.IsNewRow && row.Cells[0].Value != null && String.Equals(file, row.Cells[0].Value))
                        {
                            exist = true;
                            break;
                        }
                    }
                }
                if (!exist)
                {
                    for (int i = dataGridView1.Rows.Count - 1; i > -1; --i)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        if (!row.IsNewRow && row.Cells[0].Value == null)
                        {
                            dataGridView1.Rows.RemoveAt(i);
                        }
                    }
                    foreach (var file in fileList)
                    {
                        dataGridView1.Rows.Add(file);
                        var size = new FileInfo(file).Length / (1024f * 1024f);
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = Path.GetExtension(file);
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = size.ToString("####0.00000");
                    }
                    count = dataGridView1.Rows.Count;
                    if (count < 6)
                    {
                        for (int i = 0; i < 6 - count; i++)
                        {
                            dataGridView1.Rows.Add();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selected files(s) already exist.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                openFileDialog1.Reset();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                bool exist = false;
                string[] fileList = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.*");
                dataGridView1.AllowUserToAddRows = false;
                for (int i = dataGridView1.Rows.Count - 1; i > -1; --i)
                {
                    foreach (var file in fileList)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        if (!row.IsNewRow && row.Cells[0].Value != null && String.Equals(file, row.Cells[0].Value))
                        {
                            exist = true;
                            break;
                        }
                    }
                }
                if (!exist)
                {
                    for (int i = dataGridView1.Rows.Count - 1; i > -1; --i)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        if (!row.IsNewRow && row.Cells[0].Value == null)
                        {
                            dataGridView1.Rows.RemoveAt(i);
                        }
                    }
                    foreach (var file in fileList)
                    {
                        dataGridView1.Rows.Add(file);
                        var size = new FileInfo(file).Length / (1024f * 1024f);
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = Path.GetExtension(file);
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = size.ToString("####0.00000");
                    }
                    if (dataGridView1.Rows.Count < 6)
                    {
                        for (int i = 0; i < 6 - fileList.Length; i++)
                        {
                            dataGridView1.Rows.Add();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selected files already exist.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    fsIn = row.Cells[0].Value.ToString();
                    fsEnc = Path.Combine(label1.Text, "Encrypted" + fsIn.Substring(fsIn.LastIndexOf('\\') + 1));
                    if (String.Equals(textBox1.Text, textBox2.Text))
                    {
                        if (!String.IsNullOrEmpty(textBox1.Text))
                        {
                            Encrypt(@fsIn, @fsEnc, textBox1.Text);
                            row.Cells[3].Value = "Encrypted";
                            successfull = true;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Please enter a password.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            break;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Passwords do not match.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        break;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (successfull)
            {
                System.Windows.Forms.MessageBox.Show("Successfully encrypted.", "Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    fsEnc = row.Cells[0].Value.ToString();
                    fsDec = Path.Combine(label1.Text, "Decrypted" + fsEnc.Substring(fsEnc.IndexOf("Encrypted") + 9));
                    if (!String.IsNullOrEmpty(textBox1.Text))
                    {
                        successfull = Decrypt(@fsEnc, @fsDec, textBox1.Text);
                        if (successfull)
                        {
                            row.Cells[3].Value = "Decrypted";
                        }
                        else
                        {
                            row.Cells[3].Value = "Error";
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Please enter the password.", "Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (successfull)
            {
                System.Windows.Forms.MessageBox.Show("Successfully decrypted.", "Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Decryted with errors. Check if the password correct.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = dataGridView1.Rows.Count - 1; i > -1; --i)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (!row.IsNewRow && row.Cells[0].Value != null)
                {
                    row.Cells[0].Value = null;
                    dataGridView1.Rows.RemoveAt(i);
                    count++;
                }
            }
            if (count <= 6)
            {
                for (int i = 0; i < count; i++)
                {
                    dataGridView1.Rows.Add();
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    dataGridView1.Rows.Add();
                }
            }
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int count = 0;
            dataGridView1.SelectedRows[0].Cells[0].Value = null;
            for (int i = dataGridView1.Rows.Count - 1; i > -1; --i)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (!row.IsNewRow && row.Cells[0].Value == null)
                {
                    count++;
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
            if (dataGridView1.Rows.Count < 6 && count <= 6)
            {
                for (int i = 0; i < count; i++)
                {
                    dataGridView1.Rows.Add();
                }
            }
        }
    }
}
