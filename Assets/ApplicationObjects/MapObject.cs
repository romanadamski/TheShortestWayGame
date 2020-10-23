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
        public List<FloorElementObject> FloorElementsNormal;
        public FloorElementObject StartElement;
        public FloorElementObject FinishElement;

        public MapObject(int mapSize, int obstacleCount, string name, FloorElementObject[,] floorElements, List<FloorElementObject> floorElementsNormal, FloorElementObject startElement, FloorElementObject endElement)
        {
            MapSize = mapSize;
            ObstacleCount = obstacleCount;
            Name = name;
            FloorElements = floorElements;
            StartElement = startElement;
            FinishElement = endElement;
            GetFloorElementsNormal();
        }
        public MapObject()
        {
            FloorElements = new FloorElementObject[MapSize, MapSize];
            FloorElementsNormal = new List<FloorElementObject>();
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
            if (FloorElementsNormal.Count == 0)
                GetFloorElementsNormal();

            return new MapObject(MapSize, ObstacleCount, Name, floorElementObjects, FloorElementsNormal, StartElement, FinishElement);
        }
        public void GetFloorElementsNormal()
        {
            FloorElementsNormal = new List<FloorElementObject>();
            foreach (var floorElement in FloorElements)
            {
                if (floorElement != null && floorElement.FloorElementType == Enums.FloorElementTypeEnum.NORMAL)
                    FloorElementsNormal.Add(floorElement);
            }
        }
    }
}
