

using System.Collections.Generic;

namespace ifc2geojson.core
{
    public abstract class Element
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string GlobalId { get; set; }

        public double Surface { get; set; }
        public double Width { get; set; }
        public double Volume { get; set; }

        public string Material { get; set; }
        public string ElementId { get; set; }

        public double CO2 { get; set; }
        public double Emisson { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        /* Removed boundingboxes and global xyz
        public double BoundingBoxLength { get; set; }
        public double BoundingBoxWidth { get; set; }
        public double BoundingBoxHeight { get; set; }

        public double GlobalX { get; set; }
        public double GlobalY { get; set; }
        public double GlobalZ { get; set; }
        
        public bool HasOwnGeometry { get; set; }
        */
    }
}
