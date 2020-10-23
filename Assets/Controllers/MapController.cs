using Assets.ApplicationObjects;
using Assets.Enums;
using Assets.Models;
using Assets.SavedMaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Controllers
{
    public class MapController
    {
        public MapObject ActiveMap;
        public MapObject LoadedMap;
        public List<MapObject> SavedMaps;
        GameObject Floor;
        System.Random Random;
        FloorModel FloorModel;
        public MapController()
        {
            setObjects();
            prepareMap();
        }
        public void LoadMapByName(string mapName)
        {
            prepareMap(mapName);
        }
        void prepareMap(string mapName = null)
        {
            if (mapName == null)
                ActiveMap = new MapObject();
            else
                LoadedMap = SavedMaps.Find(x => x.Name == mapName).Clone();
        }

        private void setObjects()
        {
            ActiveMap = new MapObject();
            LoadedMap = new MapObject();
            Floor = GameObject.Find("Floor");
            FloorModel = Floor.GetComponent<FloorModel>();
            SavedMaps = new List<MapObject>();
            GetSavedMaps();
            Random = new System.Random();
        }

        public void GetSavedMaps()
        {
            makeMainMapFile();
            List<string> mapList = IODataManager.GetMapsNames();
            SavedMaps = IODataManager.LoadMapsByMapNames(mapList);
        }

        private void makeMainMapFile()
        {
            IODataManager.CreateMainFile();
        }

        public void GenerateMap()
        {
            var startTime = DateTime.Now;
            clearFloorElements();
            addNewFloorToScene();
            setStartAndFinish();
            addObstacles();
            setMaterialsAccordingToFloorType();
            var totalTime = (DateTime.Now - startTime).TotalSeconds;
            //EditorUtility.DisplayDialog("Czas wczytywania mapy", totalTime.ToString(), "OK");
        }
        public void GenerateLoadedMap()
        {
            var startTime = DateTime.Now;
            clearFloorElements();
            ActiveMap = LoadedMap.Clone();
            loadFloorToScene();
            setMaterialsAccordingToFloorType();
            var totalTime = (DateTime.Now - startTime).TotalSeconds;
            //EditorUtility.DisplayDialog("Czas wczytywania mapy", totalTime.ToString(), "OK");
        }
        private void setMaterialsAccordingToFloorType()
        {
            foreach(var floorElement in ActiveMap.FloorElements)
            {
                setMaterialAccordingToFloorType(floorElement);
            }
        }

        private void setStartAndFinish()
        {
            ActiveMap.StartElement = ActiveMap.FloorElements[Random.Next(0, ActiveMap.MapSize), Random.Next(0, ActiveMap.MapSize)];
            ActiveMap.StartElement.FloorElementType = FloorElementTypeEnum.START;
            Vector3 endLocation = getRandomNumberWithoutRepeating(ActiveMap.StartElement.Location);
            ActiveMap.EndElement = ActiveMap.FloorElements[(int)endLocation.x, (int)endLocation.z];
            ActiveMap.EndElement.FloorElementType = FloorElementTypeEnum.FINISH;
        }
        Vector3 getRandomNumberWithoutRepeating(params Vector3 []occupiedLocations)
        {
            int x = Random.Next(0, ActiveMap.MapSize);
            int z = Random.Next(0, ActiveMap.MapSize);
            if (occupiedLocations.Where(item => item.x == x && item.z == z).Count() > 0)
                getRandomNumberWithoutRepeating(occupiedLocations);
            return new Vector3(x, 0, z);
        }
        private void clearFloorElements()
        {
            foreach (var floorElement in ActiveMap.FloorElements)
            {
                GameObject.Destroy(floorElement.GameObject);
            }
            ActiveMap.FloorElements = new FloorElementObject[ActiveMap.MapSize, ActiveMap.MapSize];
        }

        private void addNewFloorToScene()
        {
            for (int i = 0; i < ActiveMap.MapSize; i++)
            {
                for (int j = 0; j < ActiveMap.MapSize; j++)
                {
                    var prefabFloor = GameObject.Instantiate(FloorModel.FloorElement);
                    prefabFloor.transform.parent = Floor.transform;
                    prefabFloor.transform.localPosition = new Vector3(i, 0, j);
                    FloorElementObject floorElement = new FloorElementObject(prefabFloor);
                    floorElement.FloorElementType = FloorElementTypeEnum.NORMAL;
                    ActiveMap.FloorElements[i, j] = floorElement;
                }
            }
        }
        private void loadFloorToScene()
        {
            foreach(var floorElement in ActiveMap.FloorElements)
            {
                var prefabFloor = GameObject.Instantiate(FloorModel.FloorElement);
                prefabFloor.transform.parent = Floor.transform;
                prefabFloor.transform.localPosition = floorElement.Location;
                floorElement.GameObject = prefabFloor;
            }
        }
        private void addObstacles()
        {
            for (int i = 0; i < ActiveMap.ObstacleCount; i++)
            {
                int obstacleWidth = Random.Next(1, 3);
                int obstacleheight = Random.Next(1, 3);
                Vector3 obstacleLocation = getRandomNumberWithoutRepeating(ActiveMap.StartElement.Location, ActiveMap.EndElement.Location);

                var floorElement = ActiveMap.FloorElements[(int)obstacleLocation.x, (int)obstacleLocation.z];
                floorElement.FloorElementType = FloorElementTypeEnum.OBSTACLE;
            }
        }

        void setMaterialAccordingToFloorType(FloorElementObject floorElement)
        {
            switch (floorElement.FloorElementType)
            {
                case FloorElementTypeEnum.NORMAL:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorModel.NormalMaterial;
                    break;
                case FloorElementTypeEnum.OBSTACLE:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorModel.ObstacleMaterial;
                    break;
                case FloorElementTypeEnum.START:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorModel.StartMaterial;
                    break;
                case FloorElementTypeEnum.FINISH:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorModel.FinishMaterial;
                    break;
                case FloorElementTypeEnum.PATH:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorModel.PathMaterial;
                    break;
                default:
                    break;
            }
        }
    }
}
