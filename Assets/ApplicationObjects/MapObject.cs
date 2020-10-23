using Assets.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public FloorElementObject[,] FloorElements;
        public FloorElementObject StartElement;
        public FloorElementObject EndElement;

        public MapObject(int mapSize, int obstacleCount, string name, FloorElementObject[,] floorElements)
        {
            MapSize = mapSize;
            ObstacleCount = obstacleCount;
            Name = name;
            FloorElements = floorElements;
        }
        public MapObject()
        {
            FloorElements = new FloorElementObject[MapSize, MapSize];
        }
        public MapObject Clone()
        {
            FloorElementObject[,] floorElementObjects = new FloorElementObject[MapSize, MapSize];
            for(int i = 0; i < MapSize; i++)
            {
                for(int j = 0; j < MapSize; j++)
                {
                    floorElementObjects[i, j] = new FloorElementObject();
                    floorElementObjects[i, j] = FloorElements[i, j].Clone();
                }
            }
            return new MapObject(MapSize, ObstacleCount, Name, floorElementObjects);
        }
    }
}
