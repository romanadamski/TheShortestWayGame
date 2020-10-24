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
    public class BellmanFord : BaseAlgorithm
    {
        public override void FindShortestPath(MapObject mapObject)
        {
            FloorElementObject[,] predecessors = new FloorElementObject[mapObject.MapSize, mapObject.MapSize];
            if (!BellmanFordAlgorithm(mapObject, predecessors))
            {
                CantFindPathShowMessage(mapObject.StartElement, mapObject.FinishElement);
            }
            else
            {
                DrawPath(mapObject.FinishElement, mapObject.FloorElements, predecessors);
            }
        }
        bool BellmanFordAlgorithm(MapObject mapObject, FloorElementObject[,] predecessors)
        {
            int[,] floorDistances = new int[mapObject.MapSize, mapObject.MapSize];
            for (int i = 0; i < mapObject.MapSize; ++i)
            {
                for (int j = 0; j < mapObject.MapSize; ++j)
                {
                    floorDistances[i, j] = int.MaxValue;
                }
            }
            floorDistances[(int)mapObject.StartElement.Location.x, (int)mapObject.StartElement.Location.z] = 0;
            for (int i = 1; i < mapObject.MapSize; i++)
            {
                foreach (var visitingElement in mapObject.FloorElements)
                {
                    if (visitingElement.FloorElementType == Enums.FloorElementTypeEnum.OBSTACLE)
                        continue;
                    if ((int)visitingElement.Location.x - 1 >= 0
                       && mapObject.FloorElements[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                       )
                    {
                        if (floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] != int.MaxValue
                            && floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] < floorDistances[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z])
                        {
                            floorDistances[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] = floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                            predecessors[(int)visitingElement.Location.x - 1, (int)visitingElement.Location.z] = mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                        }
                    }
                    if ((int)visitingElement.Location.x + 1 < mapObject.MapSize
                        && mapObject.FloorElements[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                        )
                    {
                        if (floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] != int.MaxValue
                           && floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] < floorDistances[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z])
                        {
                            floorDistances[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] = floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                            predecessors[(int)visitingElement.Location.x + 1, (int)visitingElement.Location.z] = mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                        }
                    }
                    if ((int)visitingElement.Location.z - 1 >= 0
                        && mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                        )
                    {
                        if (floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] != int.MaxValue
                           && floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] < floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1])
                        {
                            floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] = floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                            predecessors[(int)visitingElement.Location.x, (int)visitingElement.Location.z - 1] = mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                        }
                    }
                    if ((int)visitingElement.Location.z + 1 < mapObject.MapSize
                        && mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1].FloorElementType != Enums.FloorElementTypeEnum.OBSTACLE
                       )
                    {
                        if (floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] != int.MaxValue
                           && floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z] < floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1])
                        {
                            floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] = floorDistances[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                            predecessors[(int)visitingElement.Location.x, (int)visitingElement.Location.z + 1] = mapObject.FloorElements[(int)visitingElement.Location.x, (int)visitingElement.Location.z];
                        }
                    }
                }
            }
            if (predecessors[(int)mapObject.FinishElement.Location.x, (int)mapObject.FinishElement.Location.z] == null)
                return false;
            return true;
        }
    }
}
