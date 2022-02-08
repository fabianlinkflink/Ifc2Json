using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ifc2geojson.core
{
    public class WallFilter
    {
        public Project FilterWalls( Project project)
        {
            foreach (Wall wall in project.Walls)
            {
                wall.ObjectType = "Wall";
                foreach ( KeyValuePair<string,object> properties in wall.Properties)
                {   
                    if(properties.Key == "Net Surface Area on the Outside Face"){
                        Xbim.Ifc4.MeasureResource.IfcAreaMeasure surface = (Xbim.Ifc4.MeasureResource.IfcAreaMeasure)properties.Value;
                        wall.Surface = surface;
                    }
                    else if(properties.Key == "Building Material / Composite / Profile / Fill")
                    {
                        wall.Material = (string)properties.Value;
                    }
                    else if(properties.Key == "Width")
                    {
                        Xbim.Ifc4.MeasureResource.IfcLengthMeasure width = (Xbim.Ifc4.MeasureResource.IfcLengthMeasure)properties.Value;
                        wall.Width = width;
                    }
                    else if(properties.Key == "Element ID")
                    {
                        string id = (string)properties.Value;
                        int index = id.IndexOf(" ");
                        if (index >= 0)
                            id = id.Substring(0, index);

                        wall.ElementId = id;
                    }
                }

                if (wall.ElementId == "YVP10-2")
                    wall.ElementId = "YV1";
                /*
                if(wall.ElementId == "YVxx")
                {
                    if(wall.Width == 0.125)
                    {
                        wall.ElementId = "YV01";
                    }
                    else if(wall.Width == 0.138)
                    {
                        wall.ElementId = "YV02";
                    }
                    else if(wall.Width == 0.15)
                    {
                        wall.ElementId = "YV03";
                    }
                    else if(wall.Width == 0.2)
                    {
                        wall.ElementId = "YV04";
                    }
                    else if(wall.Width == 0.39)
                    {
                        wall.ElementId = "YV05";
                    }
                    else if(wall.Width == 0.1)
                    {
                        wall.ElementId = "YV101";
                    }
                    else if(wall.Width == 0.12)
                    {
                        wall.ElementId = "YV102";
                    }
                }*/

                if (wall.ElementId.EndsWith("T"))
                {
                    wall.ElementId = wall.ElementId.Remove(wall.ElementId.Length-1, 1);
                }

                wall.Volume = wall.Surface*wall.Width;
            }

            project.Walls.ForEach(wall => wall.Properties.Clear());

            return project;
        }

        public static void RenameKey(Dictionary<string, object> dic, string fromKey, string toKey)
        {
            object value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }

    }
}
