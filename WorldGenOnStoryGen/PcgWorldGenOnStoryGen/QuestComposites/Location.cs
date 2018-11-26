using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcgWorldGenOnStoryGen
{
    public class Location : QuestComposite
    {
        public TileType LocationType { get; set; }
        public Location()
        {
            types = new List<string>
            {
                "Castle",
                "Dungeon",
                "Town",
                "Wild"
            };
            GenerateValue();
            outputComposition = type;
            LocationType = StringToEnumTypes(type);
        }

        public override void GenerateValue()
        {
            rnd = new Random();

            //    new Random().Shuffle<string>(types.ToArray());

            type = types[rnd.Next(0, types.Count)];
        }

        TileType StringToEnumTypes(string type)
        {
            TileType temp;

            if (type == "Castle")
                temp = TileType.CASTLE;
            else if (type == "Dungeon")
                temp = TileType.DUNGEON;
            else if (type == "Town")
                temp = TileType.TOWN;
            else
                temp = TileType.GRASS;
            return temp;
        }
    }
}
