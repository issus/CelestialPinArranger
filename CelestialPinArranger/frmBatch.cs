using AltiumSharp;
using AltiumSharp.Drawing;
using SymbolBuilder;
using SymbolBuilder.Mappers;
using SymbolBuilder.Translators;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CelestialPinArranger
{
    public partial class frmBatch : Form
    {
        public frmBatch()
        {
            InitializeComponent();
        }

        private void frmBatch_Load(object sender, EventArgs e)
        {
            var files = Directory.GetFiles("JSON");

            foreach (var file in files)
            {
                cboJson.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        private void btnSourceDirectory_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtSourceDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnDestDirectory_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtDestinationDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnImgsDir_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtImagesDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;

            if (string.IsNullOrEmpty((string)cboJson.SelectedItem))
            {
                MessageBox.Show("Select a processor.");
                btnProcess.Enabled = true;

                return;
            }

            if (!Directory.Exists(txtSourceDir.Text))
            {
                MessageBox.Show("Source folder does not exist.");
                btnProcess.Enabled = true;

                return;
            }

            if (!Directory.Exists(txtDestinationDir.Text))
            {
                MessageBox.Show("Destination folder does not exist.");
                btnProcess.Enabled = true;

                return;
            }

            string imagesPath = txtImagesDir.Text;

            if (chkImages.Checked)
            {
                if (!imagesPath.Contains("/") && !imagesPath.Contains("\\"))
                {
                    imagesPath = Path.Combine(txtDestinationDir.Text, imagesPath);
                }

                if (!Directory.Exists(imagesPath))
                {
                    if (MessageBox.Show("Image folder does not exist. Create?", "Create Folder?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(imagesPath);
                    }
                    else
                    {
                        btnProcess.Enabled = true;

                        return;
                    }
                }
            }

            var sourceFiles = Directory.GetFiles(txtSourceDir.Text);

            pbProgress.Value = 0;
            pbProgress.Maximum = sourceFiles.Length;
            pbProgress.Visible = true;

            lblProgress.Text = $"0/{sourceFiles.Length}";
            lblProgress.Visible = true;

            var mapper = new JsonMapper($"JSON/{(string)cboJson.SelectedItem}.json");

            var writer = new SchLibWriter();

            int i = 0;
            foreach (var file in sourceFiles)
            {
                lblProgress.Text = $"{i++}/{sourceFiles.Length}";
                pbProgress.Value = i;

                var packageName = Path.GetFileNameWithoutExtension(file.Replace("signal_configuration", "")).Trim();

                var arranger = new PinArranger(mapper);
                arranger.LoadFromFile(Path.Combine(txtSourceDir.Text, file));

                var symbolDefinitions = arranger.Execute();

                AltiumOutput altiumOutput = new AltiumOutput();
                var schLib = (SchLib)altiumOutput.GenerateNativeType(symbolDefinitions);

                var component = schLib.Items[0];

                string fileName = $"SCH - {txtFormatFolder.Text} - {txtManufacturerName.Text} {packageName}.SchLib";
                var fullFileName = Path.Combine(txtDestinationDir.Text, fileName);

                var newLib = new SchLib();

                component.LibReference = string.Join(" ", txtManufacturerName.Text, packageName);

                newLib.Add(component);

                writer.Write(newLib, fullFileName, true);
                
                if (chkImages.Checked)
                {
                    var renderer = new SchLibRenderer(schLib.Header, null)
                    {
                        BackgroundColor = Color.White
                    };

                    int r = 0;
                    foreach (var item in schLib.Items)
                    {
                        renderer.Component = item;

                        using (var image = new Bitmap(1024, 1024))
                        using (var g = Graphics.FromImage(image))
                        {
                            renderer.Render(g, 1024, 1024, true, false);
                            image.Save(Path.Combine(imagesPath, fileName.Replace(".SchLib", $"_{r++}.png")), ImageFormat.Png);
                        }
                    }


                    }

                Application.DoEvents();
            }


            btnProcess.Enabled = true;
        }

        private void chkImages_CheckedChanged(object sender, EventArgs e)
        {
            txtImagesDir.Enabled = chkImages.Checked;
            btnImgsDir.Enabled = chkImages.Checked;
        }
    }
}
