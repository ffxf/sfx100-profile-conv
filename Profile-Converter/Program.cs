using System;
using System.IO;
using System.Xml.Linq;
using CommandLine;
using System.Globalization;

namespace SFB_Profile_Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            var opts = new Options().parseOpts(args);
            Globals.co.quiet = opts.Quiet;

            var filePath = Path.Combine(opts.SrcDir, opts.Input);

            XDocument xmldoc = XDocument.Load($"{filePath}");
            Globals.co.write($"Converting from v8 profile '{filePath}'.");

            // Profile node contains the settings we need to convert in attributes
            XElement profileNode = xmldoc.Element("Profile");

            // Container class for values we need to touch in the v9 profile xml
            CtrlConfAttrs ctrlconf = new CtrlConfAttrs();
            ctrlconf.setFromXelem(profileNode);

            // This one gets represented per controller. We have the value safed in ctrlconf
            profileNode.Attribute("OverallSmoothnessEnabled").Remove();

            // These have changed case
            profileNode.Element("author").Name = "Author";
            profileNode.Element("info").Name = "Info";
            profileNode.Element("version").Name = "Version";
            profileNode.Element("bannerImage").Name = "BannerImage";

            // Build the new ControllerConfigList node and add after BannerImage node
            // Seems required it is here. SFB parsing seems to break, at least when it appears towards the bottom
            profileNode.Element("BannerImage").AddAfterSelf(
                new XElement("ControllerConfigList")
                );
            XElement ctrlConfListNode = profileNode.Element("ControllerConfigList");

            var s = new ScaleParams();

            // Convert the float and integer values of the old format to the new 0-100 ranges
            ctrlconf.Smoothness = getAttrIntVal(xmldoc, "OverallSmoothness", s.scaleToNewSmoothness, true);
            ctrlconf.MaxSpeed = getAttrIntVal(xmldoc, "maxSpeed", s.scaleToNewSpeed, false);
            ctrlconf.MinSpeed = getAttrIntVal(xmldoc, "minSpeed", s.scaleToNewSpeed, false);
            ctrlconf.Acceleration = getAttrIntVal(xmldoc, "acceleration", s.scaleToNewAccel, false);

            // In the v8 profiles we only have overall intensity.
            // We use it twice in v9 profiles - as overall intensity in the Profile node and for the SFX controller config
            ctrlconf.Intensity = getAttrIntVal(xmldoc, "overallIntensity", s.scaleToNewIntensity, true);
            xmldoc.Element("Profile").Add(new XAttribute("OverallIntensity", ctrlconf.Intensity.ToString()));

            // Write to XElement and add a child of ControllerConfigList
            ctrlConfListNode.Add(ctrlconf.toXElem());

            filePath = Path.Combine(opts.DestDir, opts.Output);
            xmldoc.Save($"{filePath}");
            Globals.co.write($"Success: v9 profile written to '{filePath}'.");
        }
        

        static int getAttrIntVal(XDocument xmldoc, string elemName, Func<int, int> convFunc, bool isFloat)
        {
            int val = 0;
            decimal dval = 0;
            CultureInfo ci = new CultureInfo("en-US", true);

            XAttribute itemElement = xmldoc.Element("Profile").Attribute(elemName);

            try
            {
                dval = Decimal.Parse(itemElement.Value, ci);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{itemElement.Value}'. Exiting.");
                Environment.Exit(1);
            }

            if (isFloat)
            {
                // Floats are x.xx format in SFB --> scale by 100 allows to treat them as ints
                val = (int)(dval * 100);
            }
            else
            {
                val = (int)dval;
            }

            Globals.co.write($"Original int val for {elemName}: {val.ToString()}");
            val = convFunc(val);
            Globals.co.write($"Converted int val for {elemName}: {val.ToString()}");

            // We remove all integer and float attributes (will need to re-add overallIntensity as OverallIntensity elsewhere)
            itemElement.Remove();
            
            return val;
        }
    }
}