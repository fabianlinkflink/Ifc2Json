using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace ifc2geojson.core
{
    public static class PopulateCO2
    {
        public static Project ParseCO2 (Project project, string path)
        {
            StreamReader r = new StreamReader(path);
            
            string json = r.ReadToEnd();
            dynamic co2Items = JsonConvert.DeserializeObject(json);

            List<CO2> emissionList = new List<CO2>();

            foreach (var co2project in co2Items)
            {
                if(co2project.Name == "Walls")
                {
                    foreach (var element in co2project)
                    {
                        foreach (var wall in element)
                        {
                            CO2 co2 = new CO2();
                            co2.Name = wall.Name;
                            co2.Thickness = wall.Thickness;
                            co2.CO2m3 = wall.CO2 / wall.Thickness;

                            emissionList.Add(co2);
                        }
                    }
                }
            }
            if (project.Exporter == "Revit")
            {
                foreach (Wall wall in project.Walls)
                {

                    foreach (CO2 co2 in emissionList)
                    {
                        if (co2.Thickness == wall.Width)
                        {
                            wall.CO2 = co2.CO2m3;

                            wall.Emisson = wall.CO2 * wall.Volume;
                        }
                    }
                }
            }
            else
            {
                foreach (Wall wall in project.Walls)
                {

                    foreach (CO2 co2 in emissionList)
                    {
                        if (co2.Name == wall.ElementId)
                        {
                            wall.CO2 = co2.CO2m3;

                            wall.Emisson = wall.CO2 * wall.Volume;
                        }
                    }
                }
            }

            return project;
        }
    }
}
