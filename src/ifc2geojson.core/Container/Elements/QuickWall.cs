using System;
using System.Collections.Generic;
using System.Text;

namespace ifc2geojson.core
{
    public class QuickWall
    {
        public double Width { get; set; }
        public double Volume { get; set; }

        public string Material { get; set; }
        public string ElementId { get; set; }

        public double Emisson { get; set; }
    }
}
