using Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ApplicationObjects
{
    public class FloorElementObject
    {
        public GameObject GameObject { get; set; }
        public Vector3 Location { get; set; }
        public FloorElementType FloorElementType { get; set; }
        public FloorElementObject(GameObject gameObject)
        {
            GameObject = gameObject;
            Location = gameObject.transform.localPosition;
        }

        public FloorElementObject(GameObject gameObject, Vector3 location, FloorElementType floorElementType)
        {
            GameObject = gameObject;
            Location = location;
            FloorElementType = floorElementType;
        }

        public FloorElementObject()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", FloorElementType.ToString(), Location.ToString());
        }
        public FloorElementObject Clone()
        {
            return new FloorElementObject(GameObject, Location, FloorElementType);
        }
    }
}
