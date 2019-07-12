
using System.Xml.Linq;

namespace SFB_Profile_Converter
{
    public class CtrlConfAttrs
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool SmoothingEnabled { get; set; }
        public int Smoothness { get; set; }
        public int Intensity { get; set; }
        public int Acceleration { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }

        public XElement toXElem()
        {
            XElement xel = new XElement("ControllerConfig");
            xel.Add(new XAttribute("Id", Id.ToString()),
                    new XAttribute("Type", Type),
                    new XAttribute("SmoothingEnabled", SmoothingEnabled.ToString().ToLower()),
                    new XAttribute("Smoothness", Smoothness.ToString()),
                    new XAttribute("Itensity", Intensity.ToString()),
                    new XAttribute("Acceleration", Acceleration.ToString()),
                    new XAttribute("MinSpeed", MinSpeed.ToString()),
                    new XAttribute("MaxSpeed", MaxSpeed.ToString())
                );
            return xel;
        }

        public void setFromXelem(XElement profileElem)
        {
            Id = 1;
            Type = "SFX";
            SmoothingEnabled = bool.Parse(profileElem.Attribute("OverallSmoothnessEnabled").Value);
        }
    }
}