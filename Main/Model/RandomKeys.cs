using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class RandomKey
    {
        public RandomKey(int key, int position)
        {
            Key = key;
            Position = position;
        }

        public int Key { get; set; }

        public int Position { get; set; }
    }
}
