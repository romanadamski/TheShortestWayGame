using Assets.ApplicationObjects;
using Assets.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Algorithms
{
    public class Dijkstra : BaseAlgorithm
    {
        public override void FindShortestPath(MapObject mapObject)
        {
            FloorElementObject[,] predecessors = new FloorElementObject[mapObject.MapSize, mapObject.MapSize];
            if (bfsAlgorithm(mapObject.FloorElements, mapObject.StartElement, mapObject.FinishElement, mapObject.MapSize, predecessors) == false)
            {
                CantFindPathShowMessage(mapObject.StartElement, mapObject.FinishElement);
            }
            else
            {
                List<FloorElementObject> path = preparPath(mapObject.FinishElement, predecessors);
                DrawPath(path);
            }
        }
        private List<FloorElementObject> preparPath(FloorElementObject finishElement, FloorElementObject[,] predecessors)
        {
            List<FloorElementObject> path = new List<FloorElementObject>();
            FloorElementObject crawl = finishElement;
            path.Add(crawl);
            while (predecessors[(int)crawl.Location.x, (int)crawl.Location.z].FloorElementType != Enums.FloorElementTypeEnum.START)
            {
                path.Add(predecessors[(int)crawl.Location.x, (int)crawl.Location.z]);
                crawl = predecessors[(int)crawl.Location.x, (int)crawl.Location.z];
            }
            path.RemoveAt(0);
            return path;
        }

        private bool bfsAlgorithm(FloorElementObject[,] floorElements, FloorElementObject startElement, FloorElementObject finishElement, int mapSize, FloorElementObject[,] predecessor)
        {
            List<FloorElementObject> floorElementsQueue = new List<FloorElementObject>();
            bool[, ] visitedElements = new bool[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    visitedElements[i, j] = false;
                }
            }
            visitedElements[(int)startElement.Location.x, (int)startElement.Location.z] = true;
            floorElementsQueue.Add(startElement);

            while (floorElementsQueue.Count != 0)
            {
                FloorElementObject p = floorElementsQueue.First();
                floorElementsQueue.RemoveAt(0);

                if (floorElements[(int)p.Location.x, (int)p.Location.z].FloorElementType == Enums.FloorElementTypeEnum.FINISH)
                {
                    return true;
                }
                if ((int)p.Location.x - 1 >= 0
                    && visitedElements[(int)p.Location.x - 1, (int)p.Location.z] == false
                    && floorElements[(int)p.Location.x - 1, (int)p.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(floorElements[(int)p.Location.x - 1, (int)p.Location.z]);
                    visitedElements[(int)p.Location.x - 1, (int)p.Location.z] = true;
                    predecessor[(int)p.Location.x - 1, (int)p.Location.z] = p;
                }
                if ((int)p.Location.x + 1 < mapSize
                    && visitedElements[(int)p.Location.x + 1, (int)p.Location.z] == false
                    && floorElements[(int)p.Location.x + 1, (int)p.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(floorElements[(int)p.Location.x + 1, (int)p.Location.z]);
                    visitedElements[(int)p.Location.x + 1, (int)p.Location.z] = true;
                    predecessor[(int)p.Location.x + 1, (int)p.Location.z] = p;
                }
                if ((int)p.Location.z - 1 >= 0
                    && visitedElements[(int)p.Location.x, (int)p.Location.z - 1] == false
                    && floorElements[(int)p.Location.x, (int)p.Location.z - 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(floorElements[(int)p.Location.x, (int)p.Location.z - 1]);
                    visitedElements[(int)p.Location.x, (int)p.Location.z - 1] = true;
                    predecessor[(int)p.Location.x, (int)p.Location.z - 1] = p;
                }
                if ((int)p.Location.z + 1 < mapSize
                   && visitedElements[(int)p.Location.x, (int)p.Location.z + 1] == false
                    && floorElements[(int)p.Location.x, (int)p.Location.z + 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                   )
                {
                    floorElementsQueue.Add(floorElements[(int)p.Location.x, (int)p.Location.z + 1]);
                    visitedElements[(int)p.Location.x, (int)p.Location.z + 1] = true;
                    predecessor[(int)p.Location.x, (int)p.Location.z + 1] = p;
                }
            }
            return false;
        }
    }
}
