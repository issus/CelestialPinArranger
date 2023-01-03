using InfineonDaveTranslator.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace InfineonDaveTranslator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        List<MCU> mcus;
        Regex nameExtract = new Regex("^XMC[14]\\.\\d+\\.\\d+\\.(?<Package>[QFT])(?<PinCount>\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Regex peripheralExtract = new Regex("peripheral/(?<per>\\w+)/(?<iter>\\d+)/(?:(?<fn>[\\w_]+)|[^/]+/(?:(?<ch>\\d+)/)?(?<fn>[\\w_]+))\\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private void btnDataRefresh_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtDaveDir.Text))
                return;

            var versionDir = GetSubDirStartingWith("4.", txtDaveDir.Text);
            if (versionDir == null) return;

            var storeDir = GetSubDirStartingWith("D_LibraryStore", versionDir);
            if (storeDir == null) return;

            var idx = Path.Combine(storeDir, "1.idx");
            if (!File.Exists(idx)) return;


            using StreamReader indexFile = new StreamReader(idx);
            XmlDocument indexDoc = new XmlDocument();
            indexDoc.Load(indexFile);

            var xmlDoc = indexDoc.DocumentElement;
            if (xmlDoc == null) return;

            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(indexDoc.NameTable);
            xmlnsManager.AddNamespace("com.ifx.davex.softwareid", "com.ifx.davex.softwareid");
            xmlnsManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xmlnsManager.AddNamespace("ResourceModel", "http://www.infineon.com/Davex/Resource.ecore");

            var softwareIds = xmlDoc.ChildNodes.Cast<XmlNode>().Where(n => nameExtract.IsMatch(n.Attributes["id"].Value));

            mcus = new List<MCU>();

            foreach (XmlNode mcu in softwareIds)
            {
                string mcuId = mcu.Attributes["id"].Value;
                var nameMatch = nameExtract.Match(mcuId);

                var pack = nameMatch.Groups["Package"].Value;
                var pinCount = nameMatch.Groups["PinCount"].Value;
                string mcuName = mcuId.Replace(".ALL", "").Replace(".", "");

                Package package = Package.VQFN;
                switch (pack)
                {
                    case "F":
                        package = Package.LQFP;
                        break;
                    case "T":
                        package = Package.TSSOP;
                        break;
                    default:
                        break;
                }

                MCU micro = new MCU(mcuId, mcuName, package);

                var dataList = mcu.ChildNodes.Cast<XmlNode>().Where(n => n.Attributes["location"].Value.EndsWith(".dd"));

                string deviceDef = dataList.FirstOrDefault(n => n.Attributes["uri"].Value.StartsWith("device/"))?.Attributes["location"].Value;
                if (deviceDef == null)
                    continue;

                using StreamReader deviceFile = new StreamReader(Path.Combine(storeDir, deviceDef));
                XmlDocument deviceDoc = new XmlDocument();
                deviceDoc.Load(deviceFile);
                var deviceXml = deviceDoc.DocumentElement;
                var connectionNodes = deviceXml.SelectNodes("//connections").Cast<XmlNode>();

                string devicePackageDef = dataList.FirstOrDefault(n => n.Attributes["uri"].Value.StartsWith("devicepackage/"))?.Attributes["location"].Value;

                using StreamReader devicePackageFile = new StreamReader(Path.Combine(storeDir, devicePackageDef));
                XmlDocument devicePackageDoc = new XmlDocument();
                devicePackageDoc.Load(devicePackageFile);
                var devicePackageXml = devicePackageDoc.DocumentElement;

                List<Resource> packagePins = new List<Resource>();
                packagePins.AddRange(devicePackageXml.SelectNodes("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager).Cast<XmlNode>()
                        .Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value)));

                var powerDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/power_port") || n.Attributes["uri"].Value.StartsWith("port/analogpower_port"));
                foreach (var powerDef in powerDefs)
                {
                    using StreamReader powerFile = new StreamReader(Path.Combine(storeDir, powerDef.Attributes["location"].Value));
                    XmlDocument powerDoc = new XmlDocument();
                    powerDoc.Load(powerFile);

                    var powerXml = powerDoc.DocumentElement;
                    if (powerXml == null) continue;

                    var resourceDef = powerXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager);
                    var resources = resourceDef.ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value));

                    foreach (var port in resources)
                    {
                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));
                        
                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        micro.AddMcuPin(pin.Name, port.Name);
                    }
                }

                var crystalDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/crystal_port"));
                foreach (var crystalDef in crystalDefs)
                {
                    using StreamReader crystalFile = new StreamReader(Path.Combine(storeDir, crystalDef.Attributes["location"].Value));
                    XmlDocument crystalDoc = new XmlDocument();
                    crystalDoc.Load(crystalFile);

                    var crystalXml = crystalDoc.DocumentElement;
                    if (crystalXml == null) continue;

                    var resources = crystalXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager)
                        .ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value));

                    foreach (var port in resources)
                    {
                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));

                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        micro.AddMcuPin(pin.Name, port.Name);
                    }
                }

                var usbDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/usb_port"));
                foreach (var usbDef in usbDefs)
                {
                    using StreamReader usbFile = new StreamReader(Path.Combine(storeDir, usbDef.Attributes["location"].Value));
                    XmlDocument usbDoc = new XmlDocument();
                    usbDoc.Load(usbFile);

                    var usbXml = usbDoc.DocumentElement;
                    if (usbXml == null) continue;

                    var resources = usbXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager)
                        .ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value));

                    foreach (var port in resources)
                    {
                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));

                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        micro.AddMcuPin(pin.Name, port.Name);
                    }
                }

                var resetDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/reset_port"));
                foreach (var resetDef in resetDefs)
                {
                    using StreamReader resetFile = new StreamReader(Path.Combine(storeDir, resetDef.Attributes["location"].Value));
                    XmlDocument resetDoc = new XmlDocument();
                    resetDoc.Load(resetFile);

                    var resetXml = resetDoc.DocumentElement;
                    if (resetXml == null) continue;

                    var resources = resetXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager)
                        .ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value));

                    foreach (var port in resources)
                    {
                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));

                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        micro.AddMcuPin(pin.Name, port.Name);
                    }
                }

                var debugDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/debug_port"));
                foreach (var debugDef in debugDefs)
                {
                    using StreamReader debugFile = new StreamReader(Path.Combine(storeDir, debugDef.Attributes["location"].Value));
                    XmlDocument debugDoc = new XmlDocument();
                    debugDoc.Load(debugFile);

                    var debugXml = debugDoc.DocumentElement;
                    if (debugXml == null) continue;

                    var resources = debugXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager)
                        .ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value));

                    foreach (var port in resources)
                    {
                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));

                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        micro.AddMcuPin(pin.Name, port.Name);
                    }
                }

                var portDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/p"));
                foreach (XmlNode portDef in portDefs)
                {
                    using StreamReader portFile = new StreamReader(Path.Combine(storeDir, portDef.Attributes["location"].Value));
                    XmlDocument portDoc = new XmlDocument();
                    portDoc.Load(portFile);

                    var portXml = portDoc.DocumentElement;
                    if (portXml == null) continue;

                    var resourcesXml = portXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager)
                        .ChildNodes.Cast<XmlNode>();

                    foreach (var p in resourcesXml)
                    {
                        var port = new Resource(p.Attributes["name"].Value, p.Attributes["URI"].Value);

                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));

                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        var mcuPin = micro.AddMcuPin(pin.Name, port.Name);

                        //List<string> signalUris = new List<string>();
                        var altRes = p.ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["requiredResourceUri"].Value.Replace("pad", "pad/").Replace("/_", "/")));

                        var signalUris = altRes.SelectMany(r => 
                            connectionNodes
                                .Where(n => (n.Attributes["sourceSignal"].Value == r.Uri && n.Attributes["targetSignal"].Value.Contains("peripheral")) || 
                                            (n.Attributes["targetSignal"].Value == r.Uri) && n.Attributes["sourceSignal"].Value.Contains("peripheral"))
                                .Select(n => n.Attributes["sourceSignal"].Value == r.Uri ? 
                                    AlternatePeripheralExtraction(n.Attributes["targetSignal"].Value) : 
                                    AlternatePeripheralExtraction(n.Attributes["sourceSignal"].Value))
                        ).Where(s => !string.IsNullOrWhiteSpace(s?.Trim()));

                        mcuPin.AlternativeFunctions.AddRange(signalUris);
                    }
                }

                var rtcDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/rtc_port"));
                foreach (var rtcDef in rtcDefs)
                {
                    using StreamReader rtcFile = new StreamReader(Path.Combine(storeDir, rtcDef.Attributes["location"].Value));
                    XmlDocument rtcDoc = new XmlDocument();
                    rtcDoc.Load(rtcFile);

                    var rtcXml = rtcDoc.DocumentElement;
                    if (rtcXml == null) continue;

                    var resources = rtcXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager)
                        .ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value));

                    foreach (var port in resources)
                    {
                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));

                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        micro.AddMcuPin(pin.Name, port.Name);
                    }
                }

                var hcuDefs = dataList.Where(n => n.Attributes["uri"].Value.StartsWith("port/hcu_port"));
                foreach (var hcuDef in hcuDefs)
                {
                    using StreamReader hcuFile = new StreamReader(Path.Combine(storeDir, hcuDef.Attributes["location"].Value));
                    XmlDocument hcuDoc = new XmlDocument();
                    hcuDoc.Load(hcuFile);

                    var hcuXml = hcuDoc.DocumentElement;
                    if (hcuXml == null) continue;

                    var resources = hcuXml.SelectSingleNode("//provided[@xsi:type='ResourceModel:ResourceGroup']", xmlnsManager)
                        .ChildNodes.Cast<XmlNode>().Select(n => new Resource(n.Attributes["name"].Value, n.Attributes["URI"].Value));

                    foreach (var port in resources)
                    {
                        // <connections sourceSignal="http://resources/0.0.0/port/power_port/0/pad/0/pad" targetSignal="http://resources/LQFP100.0.0/devicepackage/0/17/pin" />
                        var connections = deviceXml.SelectNodes($"//connections[@sourceSignal='{port.Uri}/pad']")
                            .Cast<XmlNode>().Where(n => n.Attributes["targetSignal"].Value.EndsWith("pin"));

                        if (connections.Count() == 0 || connections.Count() > 1)
                            continue;

                        var conn = connections.First();
                        var pin = packagePins.FirstOrDefault(p => $"{p.Uri}/pin".ToUpper() == conn.Attributes["targetSignal"].Value.ToUpper());
                        micro.AddMcuPin(pin.Name, port.Name);
                    }
                }

                mcus.Add(micro);
                micro.Save($"C:/temp/common/{micro.Name} - {micro.Package}{micro.Pins.Count()}.cljson");
            }
        }

        private string? AlternatePeripheralExtraction(string name)
        {
            var match = peripheralExtract.Match(name);
            if (!match.Success)
                return null;

            string? peripheral = match.Groups["per"]?.Value;
            string? number = match.Groups["iter"]?.Value ?? "";
            string? function = match.Groups["fn"]?.Value;
            string? channel = match.Groups["ch"]?.Value ?? "";

            if (peripheral == null || function == null)
                return null;

            function = function.Replace($"{peripheral}_", "");

            return $"{peripheral}{number}_{function}{channel}".ToUpper();
        }


        private string? GetSubDirStartingWith(string startsWith, string directory)
        {
            var subdirs = Directory.GetDirectories(directory).Where(d => Path.GetFileName(d).StartsWith(startsWith));
            return subdirs.FirstOrDefault();
        }

        private void bntDaveDir_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "DAVE Directory";
            dlg.ShowNewFolderButton = false;

            if (dlg.ShowDialog() == DialogResult.OK && Directory.Exists(dlg.SelectedPath))
            {
                txtDaveDir.Text = dlg.SelectedPath;
                btnDataRefresh_Click(sender, e);
            }
        }

        private void btnBrowseExport_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Export Directory";
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog() == DialogResult.OK && Directory.Exists(dlg.SelectedPath))
            {
                txtExport.Text = dlg.SelectedPath;
            }
        }
    }
}