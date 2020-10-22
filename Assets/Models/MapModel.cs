using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class MapModel : ISerializable
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

        public MapModel(SerializationInfo info, StreamingContext ctxt)
        {
            MapSize = (int)info.GetValue("MapSize", typeof(int));
            ObstacleCount = (int)info.GetValue("ObstacleCount", typeof(int));
            Name = (string)info.GetValue("Name", typeof(string));
            FloorElements = (List<FloorElementModel>)info.GetValue("FloorElements", typeof(List<FloorElementModel>));
        }
        public MapModel()
        {
            FloorElements = new List<FloorElementModel>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MapSize", MapSize);
            info.AddValue("ObstacleCount", ObstacleCount);
            info.AddValue("Name", Name);
            info.AddValue("FloorElements", FloorElements);
        }
    }
}
