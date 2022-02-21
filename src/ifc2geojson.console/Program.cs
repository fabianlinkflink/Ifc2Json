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
using System.Threading;

namespace ifc2geojson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("tool ifc2geojson");
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                string inputpath = "C:\\Users\\mm1004\\OneDrive - LINK Arkitektur\\Dokument\\1_LinkIO\\Papers\\ML Classification\\Lojobacken\\";
                string filename = "Lojobacken_IFC.ifc";
                string co2name = "CO2Lojo.json";
                    
                string inputfile = inputpath + filename;
                string co2file = inputpath + co2name;

                // Cool spinner
                var spinner = new Spinner(1, 2);
                Console.WriteLine("Input file: " + inputfile);

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                spinner.Start();
                var model = IfcStore.Open(inputfile);
                var project = IfcParser.ParseModel(model);

                Console.WriteLine("IFC parsed: " + project.Name);

                Filter filter = new Filter();
                project = filter.RunFilter(project);

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

                spinner.Stop();
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

        public class Spinner : IDisposable
        {
            private const string Sequence = @"/-\|";
            private int counter = 0;
            private readonly int left;
            private readonly int top;
            private readonly int delay;
            private bool active;
            private readonly Thread thread;

            public Spinner(int left, int top, int delay = 100)
            {
                this.left = left;
                this.top = top;
                this.delay = delay;
                thread = new Thread(Spin);
            }

            public void Start()
            {
                active = true;
                if (!thread.IsAlive)
                    thread.Start();
            }

            public void Stop()
            {
                active = false;
                Draw(' ');
            }

            private void Spin()
            {
                while (active)
                {
                    Turn();
                    Thread.Sleep(delay);
                }
            }

            private void Draw(char c)
            {
                Console.SetCursorPosition(left, top);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(c);
            }

            private void Turn()
            {
                Draw(Sequence[++counter % Sequence.Length]);
            }

            public void Dispose()
            {
                Stop();
            }
        }
    }
}
