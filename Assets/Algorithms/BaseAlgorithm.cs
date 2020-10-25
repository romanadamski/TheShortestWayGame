using Assets.ApplicationObjects;
using Assets.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Algorithms
{
    public class BaseAlgorithm
    {
        public virtual bool FindShortestPath(MapObject mapObject)
        {
            return false;
        }

        protected void CantFindPathShowMessage(FloorElementObject start, FloorElementObject finish)
        {
            MainManager.CanvasManager.ShowMessage(string.Format("{0}\n({1}, {2}) a ({3}, {4})", "Nie da się utworzyć scieżki między", start.Location.x, start.Location.z, finish.Location.x, finish.Location.z));
        }

        protected void DrawPath(FloorElementObject finishElement, FloorElementObject[,] predecessors)
        {
            List<FloorElementObject> path = PreparPath(finishElement, predecessors);
            DrawPathOnScene(path);
        }

        private void DrawPathOnScene(List<FloorElementObject> path)
        {
            foreach (var floorPath in path)
            {
                floorPath.FloorElementType = Enums.FloorElementTypeEnum.PATH;
                MainManager.MapController.SetMaterialAccordingToFloorType(floorPath);
            }
        }

        public void ClearPath(FloorElementObject[,] floorElementObjects)
        {
            foreach (var floorElement in floorElementObjects)
            {
                if (floorElement.FloorElementType == Enums.FloorElementTypeEnum.PATH)
                {
                    floorElement.FloorElementType = Enums.FloorElementTypeEnum.NORMAL;
                    MainManager.MapController.SetMaterialAccordingToFloorType(floorElement);
                }
            }
        }
        private List<FloorElementObject> PreparPath(FloorElementObject finishElement, FloorElementObject[,] predecessors)
        {
            List<FloorElementObject> path = new List<FloorElementObject>();
            FloorElementObject floor = finishElement;
            path.Add(floor);
            var test = predecessors[(int)floor.Location.x, (int)floor.Location.z];
            while (test.FloorElementType != Enums.FloorElementTypeEnum.START)
            {
                path.Add(predecessors[(int)floor.Location.x, (int)floor.Location.z]);
                floor = predecessors[(int)floor.Location.x, (int)floor.Location.z];
                test = predecessors[(int)floor.Location.x, (int)floor.Location.z];
            }
            path.RemoveAt(0);
            path.Reverse();
            return path;
        }
    }
}
