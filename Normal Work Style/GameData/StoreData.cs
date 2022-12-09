using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.FanstayWorld2D
{
     [CreateAssetMenu(fileName = "Store Data", menuName = "GS/Store Data", order = 0)]
    public class StoreData : ScriptableObject
    {
        public List<ItemData> Outfits;
        public List<ItemData> Swords;
        public List<ItemData> Bows;
        public List<ItemData> Wands;
        public List<ItemData> Mermaids;
    }
}