using Assets.ApplicationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Algorithms
{
    public abstract class BaseAlgorithm
    {
        public abstract void FindShortestPath(MapObject mapObject);
    }
}
