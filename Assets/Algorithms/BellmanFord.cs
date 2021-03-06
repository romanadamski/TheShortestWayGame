﻿using Assets.ApplicationObjects;
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
        public override bool FindShortestPath(MapObject mapObject)
        {
            ClearPath(mapObject.FloorElements);
            FloorElementObject[,] predecessors = new FloorElementObject[mapObject.MapSize, mapObject.MapSize];
            if (!BellmanFordAlgorithm(mapObject, predecessors))
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

            List<FloorElementObject> visitingElements = new List<FloorElementObject>();
            mapObject.GetFloorElementsNormal();
            foreach (var floorElement in mapObject.FloorElementsNormal)
            {
                visitingElements.Add(floorElement);
            }
            visitingElements.Add(mapObject.StartElement);
            visitingElements.Add(mapObject.FinishElement);

            for (int i = 0; i < visitingElements.Count; i++)
            {
                foreach (var visitingElement in visitingElements)
                {
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
