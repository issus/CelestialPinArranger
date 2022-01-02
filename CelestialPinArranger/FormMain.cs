using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AltiumSharp;
using AltiumSharp.Drawing;
using SymbolBuilder;
using SymbolBuilder.Mappers;
using SymbolBuilder.Readers;
using SymbolBuilder.Translators;
using System.Linq;

namespace CelestialPinArranger
{
    public partial class FormMain : Form
    {
        private FormZoom _formZoom = new FormZoom();
        private SchLib _schLib;
        private SchLibRenderer _renderer;

        public FormMain()
        {
            InitializeComponent();

            PinDataReader.Register(new DelimitedTextPinReader());
            PinDataReader.Register(new SchLibPinReader());
            PinDataReader.Register(new BxlPinReader());
            PinDataReader.Register(new CubeMXPinReader());
            PinDataReader.Register(new NxpMcuXpressoPinReader());
            PinDataReader.Register(new IbisPinReader());
            PinDataReader.Register(new KiCad5PinReader());
            PinDataReader.Register(new TiSysConfigPinReader());
            PinDataReader.Register(new SimplicityStudioPinReader());
            PinDataReader.Register(new EaglePinReader());

            cmbPinMapper.Items.AddRange(new object[]{ new DefaultMapper() });
            cmbPinMapper.Items.AddRange(new object[] { new JsonMapper("JSON/NXP MCUXpresso.json") });
            cmbPinMapper.SelectedIndex = 0;
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            pnlPreview.Invalidate();
        }

        private PinMapper SelectedPinMapper => cmbPinMapper.SelectedItem as PinMapper;

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = PinDataReader.Filters;
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            var arranger = new PinArranger(SelectedPinMapper);

            try
            {
                arranger.LoadFromFile(openFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load data from file, is it compatible?\r\n\r\n{ex.Message}", "Failed to Load File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ArrangePins(arranger);
                lstPackages.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong arranging imported pins.\r\n\r\n{ex.Message}", "Arranger error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                var clipboardData = Clipboard.GetDataObject();
                if (clipboardData == null) return;

                var arranger = new PinArranger(SelectedPinMapper);

                if (clipboardData.GetDataPresent(DataFormats.CommaSeparatedValue))
                {
                    var stream = clipboardData.GetData(DataFormats.CommaSeparatedValue) as Stream;
                    arranger.LoadFromStream(stream);
                }
                else if (clipboardData.GetDataPresent(DataFormats.UnicodeText))
                {
                    var text = clipboardData.GetData(DataFormats.UnicodeText) as string;
                    arranger.LoadFromText(text);
                }
                else if (clipboardData.GetDataPresent(DataFormats.Text))
                {
                    var text = clipboardData.GetData(DataFormats.Text) as string;
                    arranger.LoadFromText(text);
                }
                else
                {
                    return;
                }

                ArrangePins(arranger);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Clipboard text not in correct format.", "Incorrect format", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlPreview.Invalidate();

            _formZoom.SetSchLib(_schLib, lstPackages.SelectedIndex);
        }

        private void preview_Resize(object sender, EventArgs e)
        {
            pnlPreview.Invalidate();
        }

        private void preview_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics, e.ClipRectangle.Width, e.ClipRectangle.Height, true);
        }

        private void pnlPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (_renderer == null) return;

            if ((e.Button & MouseButtons.Left) != 0)
            {
                var clientLocation = new Point(
                    Math.Min(Math.Max(e.Location.X, 0), pnlPreview.Width),
                    Math.Min(Math.Max(e.Location.Y, 0), pnlPreview.Height));
                var screenLocation = pnlPreview.PointToScreen(clientLocation);
                var viewCenter = _renderer.WorldFromScreen(clientLocation.X, clientLocation.Y);
                _formZoom.Update(screenLocation.X, screenLocation.Y, viewCenter, _renderer.Scale * 2.0);

                if (!_formZoom.Visible) _formZoom.Show(this);
            }
            else
            {
                _formZoom.Hide();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_schLib == null) return;

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

            string folderName = Path.GetFileName(folderBrowserDialog.SelectedPath);
            var writer = new SchLibWriter();

            var component = _schLib.Items[lstPackages.SelectedIndex];
            var packageName = lstPackages.Items[lstPackages.SelectedIndex];
            string fullFileName = SavePackage(folderName, writer, component, packageName);

            if (chkOpenAltium.Checked)
            {
                ProcessStartInfo start = new ProcessStartInfo(fullFileName);
                Process.Start(start);
            }
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (_schLib == null) return;

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

            string folderName = Path.GetFileName(folderBrowserDialog.SelectedPath);
            var writer = new SchLibWriter();

            for (int i = 0; i < _schLib.Items.Count; ++i)
            {
                var component = _schLib.Items[i];
                var packageName = lstPackages.Items[i];
                string fullFileName = SavePackage(folderName, writer, component, packageName);
            }
        }

        private string SavePackage(string folderName, SchLibWriter writer, AltiumSharp.Records.SchComponent component, object packageName)
        {
            string fileName = $"SCH - {folderName.ToUpper()} - {String.Join(" ", txtManufacturer.Text.ToUpper(), txtPartNumber.Text.ToUpper(), packageName)}.SchLib".Replace("  ", " ");
            var fullFileName = Path.Combine(folderBrowserDialog.SelectedPath, fileName);

            var newLib = new SchLib();

            component.LibReference = string.Join(" ", txtManufacturer.Text, txtPartNumber.Text, packageName);

            newLib.Add(component);

            writer.Write(newLib, fullFileName, true);
            return fullFileName;
        }

        private void UpdateSchLib(SchLib schLib)
        {
            _schLib = schLib;
            _renderer = new SchLibRenderer(_schLib.Header, null)
            {
                BackgroundColor = pnlPreview.BackColor
            };
            pnlPreview.Invalidate();

            _formZoom.SetSchLib(_schLib);
        }

        private void Draw(Graphics target, int width, int height, bool autoZoom)
        {
            if (_renderer == null) return;

            var index = Math.Max(lstPackages.SelectedIndex, 0);
            _renderer.Component = _schLib.Items[index];
            _renderer.Render(target, width, height, autoZoom, false);
        }

        private void ArrangePins(PinArranger arranger)
        {
            lstPackages.Items.Clear();
            gridData.Rows.Clear();

            var symbolDefinitions = arranger.Execute();

            txtManufacturer.Text = symbolDefinitions.FirstOrDefault()?.Manufacturer;
            txtPartNumber.Text = symbolDefinitions.FirstOrDefault()?.PartNumber;

            AltiumOutput altiumOutput = new AltiumOutput();
            var schLib = (SchLib)altiumOutput.GenerateNativeType(symbolDefinitions);

            UpdateSchLib(schLib);

            var rows = new List<DataGridViewRow>();
            foreach (var package in arranger.Symbols)
            {
                lstPackages.Items.Add($"{package.PartNumber} {package.DevicePackage}");

                foreach (var pin in package.Pins)
                {
                    var row = gridData.RowTemplate.Clone() as DataGridViewRow;
                    row.CreateCells(gridData, $"{package.PartNumber} {package.DevicePackage}", pin.Designator, pin.SignalName.Name, pin.Port, pin.PinPosition, pin.ElectricalType);
                    row.Tag = pin;
                    rows.Add(row);
                }
            }
            gridData.Rows.AddRange(rows.ToArray());
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            var batch = new frmBatch();
            batch.ShowDialog();
        }
    }
}
