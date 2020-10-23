using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class MapModel
    {
        public int MapSize;
        public int ObstacleCount;
        public string Name;
        public List<FloorElementModel> FloorElements;

        public MapModel(int mapSize, int obstacleCount, string name, List<FloorElementModel> floorElements)
        {
            MapSize = mapSize;
            ObstacleCount = obstacleCount;
            Name = name;
            FloorElements = floorElements;
        }

        public MapModel()
        {
            FloorElements = new List<FloorElementModel>();
        }
    }
}
