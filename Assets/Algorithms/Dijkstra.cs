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
            if (!bfsAlgorithm(mapObject.FloorElements, mapObject.StartElement, mapObject.FinishElement, mapObject.MapSize, predecessors))
            {
                CantFindPathShowMessage(mapObject.StartElement, mapObject.FinishElement);
            }
            else
            {
                DrawPath(mapObject.FinishElement, mapObject.FloorElements, predecessors);
            }
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
                FloorElementObject visitingElement = floorElementsQueue.First();
                floorElementsQueue.RemoveAt(0);

                if (floorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z].FloorElementType == Enums.FloorElementTypeEnum.FINISH)
                {
                    return true;
                }
                if ((int)visitingElement.Location.x - 1 >= 0
                    && visitedElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] == false
                    && floorElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(floorElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z]);
                    visitedElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] = true;
                    predecessor[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] = visitingElement;
                }
                if ((int)visitingElement.Location.x + 1 < mapSize
                    && visitedElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] == false
                    && floorElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(floorElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z]);
                    visitedElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] = true;
                    predecessor[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] = visitingElement;
                }
                if ((int)visitingElement.Location.z - 1 >= 0
                    && visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] == false
                    && floorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(floorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1]);
                    visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] = true;
                    predecessor[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] = visitingElement;
                }
                if ((int)visitingElement.Location.z + 1 < mapSize
                   && visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] == false
                    && floorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                   )
                {
                    floorElementsQueue.Add(floorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1]);
                    visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] = true;
                    predecessor[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] = visitingElement;
                }
            }
            return false;
        }
    }
}
