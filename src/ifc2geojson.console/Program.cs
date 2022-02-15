using CommandLine;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using ifc2geojson.core;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using Xbim.Ifc;
using System.Collections.Generic;

namespace ifc2geojson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("tool ifc2geojson");
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                string inputpath = "C:\\Users\\mm1004\\OneDrive - LINK Arkitektur\\Dokument\\1_LinkIO\\Papers\\ML Classification\\Strandboligerne\\";
                string filename = "Strandboligerne.ifc";
                string co2name = "CO2TV_Byen.json";

                string inputfile = inputpath + filename;
                string co2file = inputpath + co2name;

                Console.WriteLine("Input file: " + inputfile);

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var model = IfcStore.Open(inputfile);

                var project = IfcParser.ParseModel(model);
                Console.WriteLine("IFC parsed: " + project.Name);

                Filter filter = new Filter();
                project = filter.RunFilter(project);
                Console.WriteLine("Filter ran: " + project.Name);

                project = PopulateCO2.ParseCO2(project, co2file);
                Console.WriteLine("CO2 calculated: " + project.Name);

                List<QuickWall> quickResult = new List<QuickWall>();

                quickResult = filter.QuickExport(project);

                var JsonSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };

                var serializedProject = JsonConvert.SerializeObject(quickResult, JsonSettings);
                File.WriteAllText($"{inputpath}{project.Name}.json", serializedProject);
                Console.WriteLine("File created: " + $"{project.Name}.json");

                stopwatch.Stop();

                Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            });
        }

        private static FeatureCollection ToGeoJson(Storey storey)
        {
            var fc = new FeatureCollection();

            foreach(var space in storey.Spaces) {

                // var poly = space.Location;
                var point = new Point(space.Location);
                var f = new Feature(point);
                f.Properties.Add("name", space.LongName);
                fc.Features.Add(f);
            }

            return fc;
        }
    }
}
