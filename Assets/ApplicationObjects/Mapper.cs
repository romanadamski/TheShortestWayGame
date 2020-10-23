using Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ApplicationObjects
{
    public static class Mapper
    {
        public static MapModel ConvertMapObjectToMapModel(MapObject mapObject)
        {
            MapModel mapModel = new MapModel();
            mapModel.MapSize = mapObject.MapSize;
            mapModel.ObstacleCount = mapObject.ObstacleCount;
            mapModel.Name = mapObject.Name;
            mapModel.FloorElements = ConvertListFloorElementObjectToListFloorElementModel(mapObject.FloorElements);

            return mapModel;
        }
        public static List<FloorElementModel> ConvertListFloorElementObjectToListFloorElementModel(FloorElementObject[,] floorElementObjects)
        {
            List<FloorElementModel> floorElementModels = new List<FloorElementModel>();

            foreach (var floorElementObject in floorElementObjects)
            {
                floorElementModels.Add(ConvertFloorElementObjectToFloorElementModel(floorElementObject));
            }

            return floorElementModels;
        }
        public static FloorElementModel ConvertFloorElementObjectToFloorElementModel(FloorElementObject floorElementObject)
        {
            FloorElementModel floorElementModel = new FloorElementModel();
            floorElementModel.Location = floorElementObject.Location;
            floorElementModel.FloorElementType = floorElementObject.FloorElementType;

            return floorElementModel;
        }
        public static MapObject ConvertMapModelToMapObject(MapModel mapModel)
        {
            MapObject mapObject = new MapObject();
            mapObject.MapSize = mapModel.MapSize;
            mapObject.ObstacleCount = mapModel.ObstacleCount;
            mapObject.Name = mapModel.Name;
            mapObject.FloorElements = ConvertListFloorElementModelToListFloorElementObject(mapModel.FloorElements, mapObject.MapSize);
            mapObject.StartElement = ConvertFloorElementModelToFloorElementObject(mapModel.FloorElements.First(x => x.FloorElementType == Enums.FloorElementTypeEnum.START));
            mapObject.FinishElement = ConvertFloorElementModelToFloorElementObject(mapModel.FloorElements.First(x => x.FloorElementType == Enums.FloorElementTypeEnum.FINISH));
            return mapObject;
        }
        public static FloorElementObject[,] ConvertListFloorElementModelToListFloorElementObject(List<FloorElementModel> floorElementModels, int count)
        {
            FloorElementObject[,] floorElementObjects = new FloorElementObject[count, count];
            foreach (var floorElementModel in floorElementModels)
                floorElementObjects[(int)floorElementModel.Location.x, (int)floorElementModel.Location.z] = ConvertFloorElementModelToFloorElementObject(floorElementModel);

            return floorElementObjects;
        }
        public static FloorElementObject ConvertFloorElementModelToFloorElementObject(FloorElementModel floorElementModel)
        {
            FloorElementObject floorElementObject = new FloorElementObject();
            floorElementObject.Location = floorElementModel.Location;
            floorElementObject.FloorElementType = floorElementModel.FloorElementType;
            return floorElementObject;
        }
        public static FloorElementObject ConvertFloorElementModelToFloorElementObject(FloorElementModel floorElementModel, GameObject gameObject)
        {
            FloorElementObject floorElementObject = new FloorElementObject();
            floorElementObject.Location = floorElementModel.Location;
            floorElementObject.FloorElementType = floorElementModel.FloorElementType;
            floorElementObject.GameObject = gameObject;
            return floorElementObject;
        }
    }
}
