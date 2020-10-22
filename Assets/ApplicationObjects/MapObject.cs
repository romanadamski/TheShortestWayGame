using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ApplicationObjects
{
    public class MapObject
    {
        public int MapSize { get; set; }
        public int ObstacleCount { get; set; }
        public string Name { get; set; }
        public List<FloorElementObject> FloorElements;

        public MapObject(int mapSize, int obstacleCount, string name, List<FloorElementObject> floorElements)
        {
            MapSize = mapSize;
            ObstacleCount = obstacleCount;
            Name = name;
            FloorElements = floorElements;
        }
        public MapObject()
        {
            FloorElements = new List<FloorElementObject>();
        }
        public MapObject Clone()
        {
            List<FloorElementObject> floorElementObjects = new List<FloorElementObject>();
            foreach(var floorElement in FloorElements)
            {
                floorElementObjects.Add(floorElement.Clone());
            }
            return new MapObject(MapSize, ObstacleCount, Name, floorElementObjects);
        }
    }
}
