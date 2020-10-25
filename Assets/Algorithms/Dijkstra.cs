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
        public override bool FindShortestPath(MapObject mapObject)
        {
            ClearPath(mapObject.FloorElements);
            FloorElementObject[,] predecessors = new FloorElementObject[mapObject.MapSize, mapObject.MapSize];
            if (!DijkstraAlgorithm(mapObject, predecessors))
            {
                CantFindPathShowMessage(mapObject.StartElement, mapObject.FinishElement);
                return false;
            }
            else
            {
                DrawPath(mapObject.FinishElement, predecessors);
                return true;
            }
        }

        private bool DijkstraAlgorithm(MapObject mapObject, FloorElementObject[,] predecessor)
        {
            List<FloorElementObject> floorElementsQueue = new List<FloorElementObject>();
            bool[, ] visitedElements = new bool[mapObject.MapSize, mapObject.MapSize];
            for (int i = 0; i < mapObject.MapSize; i++)
            {
                for (int j = 0; j < mapObject.MapSize; j++)
                {
                    visitedElements[i, j] = false;
                }
            }
            visitedElements[(int)mapObject.StartElement.Location.x, (int)mapObject.StartElement.Location.z] = true;
            floorElementsQueue.Add(mapObject.StartElement);

            while (floorElementsQueue.Count != 0)
            {
                FloorElementObject visitingElement = floorElementsQueue.First();
                floorElementsQueue.RemoveAt(0);

                if (mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z].FloorElementType == Enums.FloorElementTypeEnum.FINISH)
                {
                    return true;
                }
                if ((int)visitingElement.Location.x - 1 >= 0
                    && visitedElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] == false
                    && mapObject.FloorElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(mapObject.FloorElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z]);
                    visitedElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] = true;
                    predecessor[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] = visitingElement;
                }
                if ((int)visitingElement.Location.x + 1 < mapObject.MapSize
                    && visitedElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] == false
                    && mapObject.FloorElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(mapObject.FloorElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z]);
                    visitedElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] = true;
                    predecessor[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] = visitingElement;
                }
                if ((int)visitingElement.Location.z - 1 >= 0
                    && visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] == false
                    && mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                    )
                {
                    floorElementsQueue.Add(mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1]);
                    visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] = true;
                    predecessor[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] = visitingElement;
                }
                if ((int)visitingElement.Location.z + 1 < mapObject.MapSize
                   && visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] == false
                    && mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                   )
                {
                    floorElementsQueue.Add(mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1]);
                    visitedElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] = true;
                    predecessor[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] = visitingElement;
                }
            }
            return false;
        }
    }
}
