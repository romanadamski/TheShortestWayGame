using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Managers
{
    public static class MainManager
    {
        public static CanvasManager CanvasManager;
        public static CameraManager CameraManager;
        static GameObject Managers;
        static MainManager()
        {
            Managers = GameObject.Find("Managers");
            loadManagers();
        }

        private static void loadManagers()
        {
            CanvasManager = Managers.GetComponent<CanvasManager>();
            CameraManager = Managers.GetComponent<CameraManager>();
        }
    }
}
