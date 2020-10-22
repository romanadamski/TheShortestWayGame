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
        public static List<FloorElementModel> ConvertListFloorElementObjectToListFloorElementModel(List<FloorElementObject> floorElementObjects)
        {
            List<FloorElementModel> floorElementModels = new List<FloorElementModel>();
            foreach (var floorElementModel in floorElementObjects)
                floorElementModels.Add(ConvertFloorElementObjectToFloorElementModel(floorElementModel));

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
            mapModel.FloorElements = ConvertListFloorElementModelToListFloorElementObject(mapObject.FloorElements);

            return mapModel;
        }
        public static List<FloorElementObject> ConvertListFloorElementModelToListFloorElementObject(List<FloorElementModel> floorElementObjects)
        {
            List<FloorElementObject> floorElementModels = new List<FloorElementObject>();
            foreach (var floorElementModel in floorElementObjects)
                floorElementModels.Add(ConvertFloorElementModelToFloorElementObject(floorElementModel));

            return floorElementModels;
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
