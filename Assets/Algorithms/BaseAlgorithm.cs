using Assets.ApplicationObjects;
using Assets.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Algorithms
{
    public class BaseAlgorithm
    {
        public virtual void FindShortestPath(MapObject mapObject)
        {

        }
        protected void CantFindPathShowMessage(FloorElementObject start, FloorElementObject finish)
        {
            EditorUtility.DisplayDialog("Połączenie niemożliwe", string.Format("{0} {1} a {2}", "Nie da się utworzyć scieżki między", start.Location, finish.Location), "OK");
        }
        protected void DrawPath(FloorElementObject finishElement, FloorElementObject[,] floorElementObjects, FloorElementObject[,] predecessors)
        {
            ClearPath(floorElementObjects);
            List<FloorElementObject> path = preparPath(finishElement, predecessors);
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

        private void ClearPath(FloorElementObject[,] floorElementObjects)
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
        private List<FloorElementObject> preparPath(FloorElementObject finishElement, FloorElementObject[,] predecessors)
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
