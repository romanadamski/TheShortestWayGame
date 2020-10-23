using Assets.ApplicationObjects;
using Assets.Managers;
using System;
using System.Collections.Generic;
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
        protected void DrawPath(List<FloorElementObject> path)
        {
            for (int i = path.Count - 1; i >= 0; i--)
            {
                path[i].FloorElementType = Enums.FloorElementTypeEnum.PATH;
                MainManager.MapController.SetMaterialAccordingToFloorType(path[i]);
            }
        }
    }
}
