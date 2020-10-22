using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MapObject
    {
        public int Size { get; set; }
        public int ObstacleCount { get; set; }
        public List<Obstacle> Obstacles;

        public MapObject()
        {
        }

        public MapObject(int size, int obstacleCount)
        {
            Size = size;
            ObstacleCount = obstacleCount;
            GenerateObstacles();
        }
        void GenerateObstacles()
        {
            Obstacles = new List<Obstacle>();
            Random random = new Random();
            for (int i = 0; i < ObstacleCount; i++)
            {
                Console.WriteLine(random.Next(1, 2));

            }
        }
    }
}
