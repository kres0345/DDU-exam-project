using FactoryGame.Placements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
namespace FactoryGame.SaveGame
{
    [Serializable]
    public class GameData
    {
        public Hashtable Placements;

        public GameData()
        {
            Placements = new Hashtable();
        }
    }
}
