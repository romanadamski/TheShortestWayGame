using Assets.ApplicationObjects;
using Assets.Enums;
using Assets.Models;
using Assets.IOData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Managers;
using System.Threading;
using System.Collections;

namespace Assets.Controllers
{
    public class MapController : MonoBehaviour
    {
        public MapObject ActiveMap;
        public MapObject LoadedMap;
        public List<MapObject> SavedMaps;
        public GameObject Floor;
        System.Random Random;
        FloorController FloorController;
        public bool IsPathFound = false;
        public int LoadingProgress;
        public bool IsMapInstantiated = false;
        public bool IsMapReset = false;
        public bool IsMapGenerated = false;

        private void Awake()
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

        public float GetShortestDistance()
        {
            return Vector3.Distance(ActiveMap.FloorElements[ActiveMap.MapSize / 2, ActiveMap.MapSize - 1].GameObject.transform.position, Camera.main.transform.position);
        }

        private void MakeMainMapFile()
        {
            IODataManager.CreateMainFile();
        }

        public IEnumerator GenerateMap(CanvasManager canvasManager)
        {
            ResetMap(canvasManager);
            yield return new WaitUntil(() => IsMapReset);

            StartCoroutine(canvasManager.LoadProgressBar(AddNewFloorToScene(), "Trwa ładowanie mapy..."));
            yield return new WaitUntil(() => IsMapInstantiated);

            AddObstacles();
            ActiveMap.GetFloorElementsNormal();
            SetStartAndFinish();
            SetMaterialsAccordingToFloorType();

            IsMapGenerated = true;
        }
        public IEnumerator GenerateLoadedMap(CanvasManager canvasManager)
        {
            ResetMap(canvasManager);
            yield return new WaitUntil(() => IsMapReset);

            ActiveMap = LoadedMap.Clone();
            StartCoroutine(canvasManager.LoadProgressBar(LoadFloorToScene(), "Trwa ładowanie mapy..."));
            yield return new WaitUntil(() => IsMapInstantiated);

            SetMaterialsAccordingToFloorType();

            IsMapGenerated = true;
        }

        public void ResetMap(CanvasManager canvasManager)
        {
            IsPathFound = false;
            IsMapReset = false;
            IsMapInstantiated = false;
            IsMapGenerated = false;
            StartCoroutine(canvasManager.LoadProgressBar(ClearFloorElements(), "Trwa czyszczenie poprzedniej mapy..."));
        }

        private IEnumerable ClearFloorElements()
        {
            int count = 1;
            foreach (var floorElement in ActiveMap.FloorElements)
            {
                if (floorElement.GameObject != null)
                    GameObject.Destroy(floorElement.GameObject);

                count++;
                LoadingProgress = SetProgressBarValue(count, ActiveMap.FloorElements.Length);
                if (LoadingProgress % 10 == 0)
                {
                    yield return LoadingProgress;
                }
            }
            ActiveMap.FloorElements = new FloorElementObject[ActiveMap.MapSize, ActiveMap.MapSize];
            IsMapReset = true;
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

        public IEnumerable AddNewFloorToScene()
        {
            int count = 1;
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
                    LoadingProgress = SetProgressBarValue(count, ActiveMap.FloorElements.Length);
                    if (LoadingProgress % 10 == 0)
                    {
                        yield return LoadingProgress;
                    }
                    count++;
                }
            }
            IsMapInstantiated = true;
        }

        private int SetProgressBarValue(int count, int mapSize)
        {
            return (int)(((float)count / mapSize) * 100);
        }
        public IEnumerable LoadFloorToScene()
        {
            int count = 1;
            foreach(var floorElement in ActiveMap.FloorElements)
            {
                var prefabFloor = GameObject.Instantiate(FloorController.FloorElement);
                prefabFloor.transform.parent = Floor.transform;
                prefabFloor.transform.localPosition = floorElement.Location;
                floorElement.GameObject = prefabFloor;
                if(floorElement.FloorElementType == FloorElementTypeEnum.PATH && !IsPathFound)
                    IsPathFound = true;
                LoadingProgress = SetProgressBarValue(count, ActiveMap.FloorElements.Length);
                if (LoadingProgress % 10 == 0)
                {
                    yield return LoadingProgress;
                }
                count++;
            }
            IsMapInstantiated = true;
        }

        private void AddObstacles()
        {
            for (int i = 0; i < ActiveMap.ObstacleCount; i++)
            {
                Vector3 obstacleLocation = new Vector3(Random.Next(0, ActiveMap.MapSize), 0, Random.Next(ActiveMap.MapSize));
                ActiveMap.FloorElements[(int)obstacleLocation.x, (int)obstacleLocation.z].FloorElementType = FloorElementTypeEnum.OBSTACLE;
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
