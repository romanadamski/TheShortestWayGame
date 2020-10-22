using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Models
{
    [Serializable()]
    public class FloorElementModel
    {
        public Vector3 Location;
        public FloorElementType FloorElementType;
        public FloorElementModel(GameObject gameObject)
        {
            Location = gameObject.transform.localPosition;
        }

        public FloorElementModel()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", FloorElementType.ToString(), Location.ToString());
        }
    }
}
