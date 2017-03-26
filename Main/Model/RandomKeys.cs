using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class RandomKey
    {
        public RandomKey(int key, int positionIndex)
        {
            Key = key;
            PositionIndex = positionIndex;
            ForceVehicleChangeAfterThis = false;
        }

        public RandomKey(int key, int positionIndex, bool forceVehicleChangeAfterThis)
        {
            Key = key;
            PositionIndex = positionIndex;
            ForceVehicleChangeAfterThis = forceVehicleChangeAfterThis;
        }

        /// <summary>
        /// Random Value use to order the Destinations
        /// </summary>
        public int Key { get; set; }

        public int PositionIndex { get; set; }

        public bool ForceVehicleChangeAfterThis { get; set; }
    }
}
