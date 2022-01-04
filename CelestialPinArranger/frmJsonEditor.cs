using SymbolBuilder.Mappers;
using SymbolBuilder.Model;
using SymbolBuilder.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CelestialPinArranger
{
    public partial class frmJsonEditor : Form
    {
        enum Columns
        {
            Name,
            PinClass,
            Regex,
            Side,
            Alignment,
            Priority,
            GroupSpacing
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private bool isDraggingRow = false;

        public frmJsonEditor()
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
        }

        private void lnkRegexRef_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://regexstorm.net/reference");
        }

        private void btnNewMapper_Click(object sender, EventArgs e)
        {
            if (!CheckBeforeLoad())
                return;

            dgFunctions.Rows.Clear();
        }

        private void btnDefaultMapper_Click(object sender, EventArgs e)
        {
            if (!CheckBeforeLoad())
                return;

            dgFunctions.Rows.Clear();

            var mapper = new DefaultMapper();
            mapper.LoadMappings();
            AddMappingRows(mapper);
        }

        private void AddMappingRows(PinMapper mapper)
        {
            foreach (var function in mapper.Functions)
            {
                dgFunctions.Rows.Add(function.Name, function.PinClass.ToString(), function.PinNameRegex.ToString(), function.Position.Side.ToString(), function.Position.Alignment.ToString(), function.OrderPriority.ToString(), function.GroupSpacing.ToString());
            }
        }

        private void btnLoadJson_Click(object sender, EventArgs e)
        {
            if (!CheckBeforeLoad())
                return;

            dgFunctions.Rows.Clear();

            OpenFileDialog file = new OpenFileDialog()
            {
                Filter = "JSON (*.json)|*.json",
                Title = "Open Mapper JSON File",
                CheckFileExists = true
            };

            if (file.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = file.FileName;

            var mapper = new JsonMapper(fileName);
            mapper.LoadMappings();
            AddMappingRows(mapper);
        }

        private bool CheckBeforeLoad()
        {
            if (dgFunctions.RowCount > 1 && !String.IsNullOrEmpty((string)dgFunctions.Rows[0].Cells[0].Value))
            {
                if (MessageBox.Show("Are you sure you wish to erase the current mapper?", "Load Mapper", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgFunctions.Rows)
            {
                string name = (string)row.Cells[(int)Columns.Name].Value;
                string regex = (string)row.Cells[(int)Columns.Regex].Value;

                if (String.IsNullOrEmpty(name) && string.IsNullOrEmpty(regex))
                    continue;

                try
                {
                    Regex rex = new Regex(regex);
                    row.Cells[(int)Columns.Regex].Style.BackColor = Color.LightGreen;
                    row.Cells[(int)Columns.Regex].Style.SelectionBackColor = Color.DarkGreen;
                    row.Cells[(int)Columns.Regex].ToolTipText = "OK";
                }
                catch (Exception err)
                {
                    row.Cells[(int)Columns.Regex].Style.BackColor = Color.LightPink;
                    row.Cells[(int)Columns.Regex].Style.SelectionBackColor = Color.DarkRed;
                    row.Cells[(int)Columns.Regex].ToolTipText = err.Message;

                    MessageBox.Show(err.Message, $"{name} has errors", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void dgFunctions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0 && e.ColumnIndex == 2)
            {
                dgFunctions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightYellow;
                dgFunctions.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.DarkGoldenrod;
            }
        }

        #region Drag/Drop moving of rows
        private void dgFunctions_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X > 40)
                return;

            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dgFunctions.HitTest(e.X, e.Y).RowIndex;

            if (rowIndexFromMouseDown != -1)
            {
                isDraggingRow = true;

                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(
                          new Point(
                            e.X - (dragSize.Width / 2),
                            e.Y - (dragSize.Height / 2)),
                      dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dgFunctions_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingRow)
                return;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dgFunctions.DoDragDrop(
                          dgFunctions.Rows[rowIndexFromMouseDown],
                          DragDropEffects.Move);
                }
            }
        }

        private void dgFunctions_DragOver(object sender, DragEventArgs e)
        {
            if (isDraggingRow)
                e.Effect = DragDropEffects.Move;
        }

        private void dgFunctions_DragDrop(object sender, DragEventArgs e)
        {
            if (!isDraggingRow)
                return;

            isDraggingRow = false;
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dgFunctions.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = dgFunctions.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                dgFunctions.Rows.RemoveAt(rowIndexFromMouseDown);
                dgFunctions.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
            }
        }
        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(1);

            string fileName = txtTestFile.Text;

            if (String.IsNullOrEmpty(fileName))
            {
                fileName = SelectIngestFile();
                if (fileName == null) return;
            }

            var pinData = PinDataReader.Load(fileName);

            var pinMapper = GeneratePinMapper();

            List<PinDefinition> failedMappings = new List<PinDefinition>();
            List<PinDefinition> sucessfulMappings = new List<PinDefinition>();

            foreach (var symbol in pinData)
            {
                foreach (var pin in symbol.Pins)
                {
                    if (!pinMapper.Map(symbol.DevicePackage, pin))
                    {
                        failedMappings.Add(pin);
                    }
                    else
                    {
                        sucessfulMappings.Add(pin);
                    }
                }
            }

            StringBuilder failed = new StringBuilder();
            foreach (var pin in failedMappings)
            {
                failed.AppendLine(pin.FullNameSlashed);
            }

            foreach (var pin in sucessfulMappings)
            {
                dgSuccessResults.Rows.Add(pin.FullNameSlashed, pin.MappingFunction.Name, pin.Port, pin.PortBit);
            }
        }

        private void btnTestFilePath_Click(object sender, EventArgs e)
        {
            txtTestFile.Text = SelectIngestFile();
        }

        private string SelectIngestFile()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select Pin Data Ingest File",
                Filter = PinDataReader.Filters
            };

            if (ofd.ShowDialog() != DialogResult.OK) return null;

            return ofd.FileName;
        }

        private ProgrammaticPinMapper GeneratePinMapper()
        {
            ProgrammaticPinMapper mapper = new ProgrammaticPinMapper();

            if (dgFunctions.RowCount <= 1)
                return mapper;

            foreach (DataGridViewRow row in dgFunctions.Rows)
            {
                if (string.IsNullOrEmpty((string)row.Cells[(int)Columns.Name].Value) || string.IsNullOrEmpty((string)row.Cells[(int)Columns.Regex].Value))
                    continue;

                string name = (string)row.Cells[(int)Columns.Name].Value;
                PinClass pinClass = (PinClass)Enum.Parse(typeof(PinClass), (string)row.Cells[(int)Columns.PinClass].Value);
                string regex = (string)row.Cells[(int)Columns.Regex].Value;
                PinSide pinSide = (PinSide)Enum.Parse(typeof(PinSide), (string)row.Cells[(int)Columns.Side].Value);
                PinAlignment pinAlignment = (PinAlignment)Enum.Parse(typeof(PinAlignment), (string)row.Cells[(int)Columns.Alignment].Value);
                Int32.TryParse((string)row.Cells[(int)Columns.Priority].Value, out var priority);
                Int32.TryParse((string)row.Cells[(int)Columns.GroupSpacing].Value, out var groupSpacing);

                mapper.AddFunction(string.Empty, pinClass, name, regex, PinPosition.From(pinSide, pinAlignment), priority, groupSpacing: groupSpacing);
            }

            return mapper;
        }

        private void btnSaveMapper_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Save JSON Mapping File",
                Filter = "JSON File (*.json)|*.json",
                AddExtension = true
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            StringBuilder json = new StringBuilder();
            var mapper = GeneratePinMapper();

            json.Append("{\r\n  \"\": [");

            foreach (var fn in mapper.Functions)
            {
                if (mapper.Functions.First() != fn)
                {
                    json.Append(",");
                }

                json.Append("\r\n    {\r\n");
                ExportJsonAppendField(json, "functionName", fn.Name); json.Append(",\r\n");
                if (fn.PinClass != PinClass.Generic) { ExportJsonAppendField(json, "pinClass", fn.PinClass); json.Append(",\r\n"); }
                ExportJsonAppendField(json, "pinNamePattern", fn.PinNameRegex.ToString().Replace("\\", "\\\\")); json.Append(",\r\n");
                ExportJsonAppendField(json, "position", fn.Position); 
                if (fn.OrderPriority != 1) { json.Append(",\r\n"); ExportJsonAppendField(json, "priority", fn.OrderPriority);  }
                if (fn.GroupSpacing != 1) { json.Append(",\r\n"); ExportJsonAppendField(json, "groupSpacing", fn.GroupSpacing); }

                json.Append("\r\n    }");
            }

            // add default mapper
            json.Append(",\r\n    {\r\n      \"functionName\": null,\r\n      \"pinNamePattern\": \"\",\r\n      \"position\": \"Right-Middle\"\r\n    }");
            json.Append("\r\n  ]\r\n}");

            File.WriteAllText(sfd.FileName, json.ToString());
        }

        private void ExportJsonAppendField(StringBuilder json, string field, object value)
        {
            json.Append($"      \"{field}\": ");
            if (value is int)
            {
                json.Append(value);
            }
            else if (value is string)
            {
                json.Append($"\"{value}\"");
            }
            else
            {
                json.Append($"\"{value}\"");
            }
        }
    }
}
