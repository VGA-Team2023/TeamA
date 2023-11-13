//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action2D
{
    public class GameManager
    {
        public static GameManager Instance => _instance;
        public PlayerEnvroment PlayerEnvroment { get; set; }
        private static GameManager _instance = new GameManager();
    }
}
