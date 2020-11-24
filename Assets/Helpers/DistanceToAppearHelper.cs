using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Helpers
{
    public static class DistanceToAppearHelper
    {
        static float distanceToAppear;
        public static float DistanceToAppear
        {
            get
            {
                return distanceToAppear;
            }
            set
            {
                distanceToAppear = value + 100;
            }
        }
    }
}
