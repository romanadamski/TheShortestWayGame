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
            mapModel.FloorElements = ConvertListFloorElementObjectToListFloorElementModel(mapObject.FloorElements, mapModel.MapSize);

            return mapModel;
        }
        public static FloorElementModel[,] ConvertListFloorElementObjectToListFloorElementModel(FloorElementObject[,] floorElementObjects, int count)
        {
            FloorElementModel[,] floorElementModels = new FloorElementModel[count, count];
            foreach (var floorElementObject in floorElementObjects)
                floorElementModels[(int)floorElementObject.Location.x, (int)floorElementObject.Location.z] = ConvertFloorElementObjectToFloorElementModel(floorElementObject);

            return floorElementModels;
        }
        public static FloorElementModel ConvertFloorElementObjectToFloorElementModel(FloorElementObject floorElementObject)
        {
            FloorElementModel floorElementModel = new FloorElementModel();
            floorElementModel.Location = floorElementObject.Location;
            floorElementModel.FloorElementType = floorElementObject.FloorElementType;

            return floorElementModel;
        }
        public static MapObject ConvertMapModelToMapObject(MapModel mapObject)
        {
            MapObject mapModel = new MapObject();
            mapModel.MapSize = mapObject.MapSize;
            mapModel.ObstacleCount = mapObject.ObstacleCount;
            mapModel.Name = mapObject.Name;
            mapModel.FloorElements = ConvertListFloorElementModelToListFloorElementObject(mapObject.FloorElements, mapModel.MapSize);

            return mapModel;
        }
        public static FloorElementObject[,] ConvertListFloorElementModelToListFloorElementObject(FloorElementModel[,] floorElementModels, int count)
        {
            FloorElementObject[,] floorElementObjects = new FloorElementObject[count, count];
            foreach (var floorElementModel in floorElementModels)
                floorElementObjects[(int)floorElementModel.Location.x, (int)floorElementModel.Location.z] = ConvertFloorElementModelToFloorElementObject(floorElementModel);

            return floorElementObjects;
        }
        public static FloorElementObject ConvertFloorElementModelToFloorElementObject(FloorElementModel floorElementObject)
        {
            FloorElementObject floorElementModel = new FloorElementObject();
            floorElementModel.Location = floorElementObject.Location;
            floorElementModel.FloorElementType = floorElementObject.FloorElementType;
            return floorElementModel;
        }
        public static FloorElementObject ConvertFloorElementModelToFloorElementObject(FloorElementModel floorElementObject, GameObject gameObject)
        {
            FloorElementObject floorElementModel = new FloorElementObject();
            floorElementModel.Location = floorElementObject.Location;
            floorElementModel.FloorElementType = floorElementObject.FloorElementType;
            floorElementModel.GameObject = gameObject;
            return floorElementModel;
        }
    }
}
