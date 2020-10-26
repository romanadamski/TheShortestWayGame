using Assets.Algorithms;
using Assets.Controllers;
using Assets.Enums;
using Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Managers
{
    public static class MainManager
    {
        public static CanvasManager CanvasManager;
        public static CameraManager CameraManager;
        public static MapController mapController;
        public static MapController MapController
        {
            get
            {
                return mapController;
            }
            set
            {
                mapController = value;
            }
        }
        private static GameModeEnum gameMode;
        public static GameModeEnum GameMode
        {
            get
            {
                return gameMode;
            }
            set
            {
                gameMode = value;
            }
        }
        private static AlgothitmEnum algothitmEnum;
        public static AlgothitmEnum AlgothitmEnum
        {
            get
            {
                return algothitmEnum;
            }
            set
            {
                algothitmEnum = value;
                ChooseAlgorithm(value);
            }
        }

        private static void ChooseAlgorithm(AlgothitmEnum value)
        {
            switch (value)
            {
                case AlgothitmEnum.DIJKSTRA:
                    Algorithm = new Dijkstra();
                    break;
                case AlgothitmEnum.BELLMAN_FORD:
                    Algorithm = new BellmanFord();
                    break;
                default:
                    break;
            }
        }
        public static bool ExecuteAlgorithm()
        {
            return Algorithm.FindShortestPath(MapController.ActiveMap);
        }
        private static BaseAlgorithm agorithm;
        public static BaseAlgorithm Algorithm
        {
            get
            {
                return agorithm;
            }
            set
            {
                agorithm = value;
            }
        }

        static GameObject Managers;
        static MainManager()
        {
            Managers = GameObject.Find("Managers");
            LoadManagers();
        }

        private static void LoadManagers()
        {
            CanvasManager = Managers.GetComponent<CanvasManager>();
            CameraManager = Managers.GetComponent<CameraManager>();
            MapController = new MapController();
        }
    }
}
