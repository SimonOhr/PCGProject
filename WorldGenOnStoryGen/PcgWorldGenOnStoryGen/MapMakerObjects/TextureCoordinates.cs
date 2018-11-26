using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public static class TextureCoordinates
    {     
        public static Vector2 Grass { get { return new Vector2(0, 32); } }
        public static Vector2 Wood { get { return new Vector2(0, 192); } }
        public static Vector2 Castle { get { return new Vector2(192, 32); } }
        public static Vector2 Dungeon { get { return new Vector2(128, 32); } }
        public static Vector2 Town { get { return new Vector2(512,32); } }
        public static Vector2 Miner { get { return new Vector2(224, 32); } }        
    }
}
