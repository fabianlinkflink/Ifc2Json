using System;
using System.Collections.Generic;
using System.Text;

namespace ifc2geojson.core
{
    public class Filter
    {
        public Project RunFilter(Project project)
        {
            WallFilter wallFilter = new WallFilter();

            project = wallFilter.FilterWalls(project);

            return project;
        }

        public List<QuickWall> QuickExport(Project project)
        {
            List<QuickWall> result = new List<QuickWall>();

            foreach (Wall wall in project.Walls)
            {
                QuickWall wallFilter = new QuickWall();

                if( wall.Emisson != 0)
                {
                    wallFilter.Width = wall.Width;
                    wallFilter.Material = wall.Material;
                    wallFilter.Volume = wall.Volume;
                    wallFilter.Emisson = wall.Emisson;
                    wallFilter.ElementId = wall.ElementId;

                    result.Add(wallFilter);
                }
            }

            return result;
        }
        
    }
}
