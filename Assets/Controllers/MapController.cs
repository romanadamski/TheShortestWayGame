using Assets.ApplicationObjects;
using Assets.Enums;
using Assets.Models;
using Assets.IOData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Managers;

namespace Assets.Controllers
{
    public class MapController
    {
        public MapObject ActiveMap;
        public MapObject LoadedMap;
        public List<MapObject> SavedMaps;
        GameObject Floor;
        System.Random Random;
        FloorController FloorController;
        public bool IsPathFound = false;
        public MapController()
        {
            SetObjects();
            PrepareMap();
        }
        public void LoadMapByName(string mapName)
        {
            PrepareMap(mapName);
        }
        void PrepareMap(string mapName = null)
        {
            if (mapName == null)
                ActiveMap = new MapObject();
            else
                LoadedMap = SavedMaps.Find(x => x?.Name == mapName).Clone();
        }

        private void SetObjects()
        {
            ActiveMap = new MapObject();
            LoadedMap = new MapObject();
            Floor = GameObject.Find("Floor");
            FloorController = Floor.GetComponent<FloorController>();
            SavedMaps = new List<MapObject>();
            Random = new System.Random();
        }

        public void GetSavedMaps()
        {
            MakeMainMapFile();
            List<string> mapList = IODataManager.GetMapsNames();
            SavedMaps = IODataManager.LoadMapsByMapNames(mapList);
        }

        private void MakeMainMapFile()
        {
            IODataManager.CreateMainFile();
        }

        public void GenerateMap()
        {
            IsPathFound = false;
            ClearFloorElements();
            AddNewFloorToScene();
            AddObstacles();
            ActiveMap.GetFloorElementsNormal();
            SetStartAndFinish();
            SetMaterialsAccordingToFloorType();
        }
        public void GenerateLoadedMap()
        {
            IsPathFound = false;
            ClearFloorElements();
            ActiveMap = LoadedMap.Clone();
            LoadFloorToScene();
            SetMaterialsAccordingToFloorType();
        }
        private void SetMaterialsAccordingToFloorType()
        {
            foreach(var floorElement in ActiveMap.FloorElements)
            {
                SetMaterialAccordingToFloorType(floorElement);
            }
        }

        private void SetStartAndFinish()
        {
            if(ActiveMap.FloorElementsNormal.Count < 2)
            {
                MainManager.CanvasManager.ShowMessage("Zbyt dużo przeszkód, utworzenie mapy nie jest możliwe.");
            }
            else
            {
                List<FloorElementObject> normalFloors = new List<FloorElementObject>();
                normalFloors.AddRange(ActiveMap.FloorElementsNormal);
                ActiveMap.StartElement = normalFloors[Random.Next(0, normalFloors.Count)];
                ActiveMap.StartElement.FloorElementType = FloorElementTypeEnum.START;
                normalFloors.Remove(ActiveMap.StartElement);
                ActiveMap.FinishElement = normalFloors[Random.Next(0, normalFloors.Count)];
                ActiveMap.FinishElement.FloorElementType = FloorElementTypeEnum.FINISH;
            }
        }
        
        private void ClearFloorElements()
        {
            foreach (var floorElement in ActiveMap.FloorElements)
            {
                GameObject.Destroy(floorElement.GameObject);
            }
            ActiveMap.FloorElements = new FloorElementObject[ActiveMap.MapSize, ActiveMap.MapSize];
        }

        private void AddNewFloorToScene()
        {
            for (int i = 0; i < ActiveMap.MapSize; i++)
            {
                for (int j = 0; j < ActiveMap.MapSize; j++)
                {
                    var prefabFloor = GameObject.Instantiate(FloorController.FloorElement);
                    prefabFloor.transform.parent = Floor.transform;
                    prefabFloor.transform.localPosition = new Vector3(i, 0, j);
                    FloorElementObject floorElement = new FloorElementObject(prefabFloor);
                    floorElement.FloorElementType = FloorElementTypeEnum.NORMAL;
                    ActiveMap.FloorElements[i, j] = floorElement;
                }
            }
        }
        private void LoadFloorToScene()
        {
            foreach(var floorElement in ActiveMap.FloorElements)
            {
                var prefabFloor = GameObject.Instantiate(FloorController.FloorElement);
                prefabFloor.transform.parent = Floor.transform;
                prefabFloor.transform.localPosition = floorElement.Location;
                floorElement.GameObject = prefabFloor;
                if(floorElement.FloorElementType == FloorElementTypeEnum.PATH)
                    IsPathFound = true;
            }
        }
        private void AddObstacles()
        {
            for (int i = 0; i < ActiveMap.ObstacleCount; i++)
            {
                int obstacleWidth = Random.Next(1, 3);
                int obstacleHeight = Random.Next(1, 3);
                Vector3 obstacleLocation = new Vector3(Random.Next(0, ActiveMap.MapSize), 0, Random.Next(ActiveMap.MapSize));
                ActiveMap.FloorElements[(int)obstacleLocation.x, (int)obstacleLocation.z].FloorElementType = FloorElementTypeEnum.OBSTACLE;
                if(obstacleWidth == 2 
                    && obstacleLocation.x < ActiveMap.MapSize - 1
                )
                {
                    ActiveMap.FloorElements[(int)obstacleLocation.x + 1, (int)obstacleLocation.z].FloorElementType = FloorElementTypeEnum.OBSTACLE;
                }
                if (obstacleHeight == 2
                    && obstacleLocation.z < ActiveMap.MapSize - 1
                )
                {
                    ActiveMap.FloorElements[(int)obstacleLocation.x, (int)obstacleLocation.z + 1].FloorElementType = FloorElementTypeEnum.OBSTACLE;
                }
                if (obstacleHeight == 2
                    && obstacleLocation.z < ActiveMap.MapSize - 1
                    && obstacleWidth == 2
                    && obstacleLocation.x < ActiveMap.MapSize - 1)
                {
                    ActiveMap.FloorElements[(int)obstacleLocation.x + 1, (int)obstacleLocation.z + 1].FloorElementType = FloorElementTypeEnum.OBSTACLE;
                }
            }
        }

        public void SetMaterialAccordingToFloorType(FloorElementObject floorElement)
        {
            switch (floorElement.FloorElementType)
            {
                case FloorElementTypeEnum.NORMAL:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorController.NormalMaterial;
                    break;
                case FloorElementTypeEnum.OBSTACLE:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorController.ObstacleMaterial;
                    break;
                case FloorElementTypeEnum.START:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorController.StartMaterial;
                    break;
                case FloorElementTypeEnum.FINISH:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorController.FinishMaterial;
                    break;
                case FloorElementTypeEnum.PATH:
                    floorElement.GameObject.GetComponent<Renderer>().material = FloorController.PathMaterial;
                    break;
                default:
                    break;
            }
        }
    }
}
