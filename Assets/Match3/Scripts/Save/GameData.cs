using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Core;
using UnityEngine;
using System.IO;
using System;

namespace Match3.Scripts.Core
{
    [Serializable] public struct GameData
    {
        public int currentLevel;
        public string dateLastPlayed;
        public int lifesCount;
        public int maxLifesCount;
    }
}
