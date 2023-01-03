using OriginalCircuit.AltiumSharp;
using OriginalCircuit.AltiumSharp.Drawing;
using SymbolBuilder;
using SymbolBuilder.Mappers;
using SymbolBuilder.Translators;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

            if (cboJson.Items.Count > 0)
            {
                cboJson.SelectedIndex = 0;
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

        private async void btnProcess_Click(object sender, EventArgs e)
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

            string[] sourceFiles;

            if (txtSourceDir.Text.Contains("SimplicityStudio"))
            {
                sourceFiles = Directory.GetFiles(txtSourceDir.Text, "*.device", SearchOption.AllDirectories);
            }
            else
            {
                sourceFiles = Directory.GetFiles(txtSourceDir.Text);
            }


            pbProgress.Value = 0;
            pbProgress.Maximum = sourceFiles.Length;
            pbProgress.Visible = true;

            lblProgress.Text = $"0/{sourceFiles.Length}";
            lblProgress.Visible = true;

            var mapper = new JsonMapper($"JSON/{(string)cboJson.SelectedItem}.json");
            
            int i = 0;
            object locker = new object();
            await Parallel.ForEachAsync(sourceFiles, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, async (file, token) =>
            //foreach (var file in sourceFiles)
            {
                lock (locker)
                {
                    i++;
                    lblProgress.Invoke(() => lblProgress.Text = $"{i}/{sourceFiles.Length}");
                    pbProgress.Invoke(() => pbProgress.Value = i);
                    //lblProgress.Text = $"{i}/{sourceFiles.Length}";
                    //pbProgress.Value = i;
                }

                try
                {
                    var packageName = Path.GetFileNameWithoutExtension(file.Replace("signal_configuration", "")).Trim();

                    var arranger = new PinArranger(mapper);
                    
                    await arranger.LoadFromFileAsync(Path.Combine(txtSourceDir.Text, file));

                    var symbolDefinitions = arranger.Execute();

                    AltiumOutput altiumOutput = new AltiumOutput();
                    var schLib = (SchLib)altiumOutput.GenerateNativeType(symbolDefinitions);

                    foreach (var comp in schLib.Items)
                    {
                        comp.CurrentPartId = comp.GetAllPrimitives().Where(p => p.Owner != null).Min(p => p.OwnerPartId) ?? 1;
                    }

                    foreach (var component in schLib.Items)
                    {
                        string libRef = $"{txtManufacturerName.Text.Trim()} {component.DesignItemId.Trim()}"
                            .ToUpper()
                            .Trim()
                            .Replace("\\", "_")
                            .Replace("/","_");

                        string fileName = $"SCH - {txtFormatFolder.Text} - {libRef}.SchLib";
                        var fullFileName = Path.Combine(txtDestinationDir.Text, fileName);

                        var writer = new SchLibWriter();

                        var newLib = new SchLib();

                        component.LibReference = libRef;

                        newLib.Add(component);

                        writer.Write(newLib, fullFileName, true);

                        if (chkImages.Checked && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            var renderer = new SchLibRenderer(newLib.Header, null)
                            {
                                BackgroundColor = Color.White
                            };

                            foreach (var item in newLib.Items)
                            {
                                int r = 0;
                                var selectedOwnerId = 0;

                                while (component.GetAllPrimitives().Where(p => p.OwnerPartId > selectedOwnerId).Any())
                                {
                                    selectedOwnerId = component.GetAllPrimitives().Where(p => p.OwnerPartId > selectedOwnerId)?.Min(p => p.OwnerPartId) ?? selectedOwnerId;
                                    component.CurrentPartId = selectedOwnerId;
                                    renderer.Component = item;

                                    using (var image = new Bitmap(1024, 1024))
                                    using (var g = Graphics.FromImage(image))
                                    {
                                        renderer.Render(g, 1024, 1024, true, false);
                                        image.Save(Path.Combine(imagesPath, fileName.Replace(".SchLib", $"_{r++}.png")), ImageFormat.Png);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception: {ex.Message} {Environment.NewLine} AT:  {ex.StackTrace}");
                    // todo: Handle errors so the user knows something went wrong, without interrupting the export
                }
            }
            );


            btnProcess.Enabled = true;
        }

        private void chkImages_CheckedChanged(object sender, EventArgs e)
        {
            txtImagesDir.Enabled = chkImages.Checked;
            btnImgsDir.Enabled = chkImages.Checked;
        }
    }
}
