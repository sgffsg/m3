using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts.Core
{
    public class Loader : MonoBehaviour
    {
        public MatchManager gameManager;
        private void Awake()
        {
            if (MatchManager.Instance == null)
            {
                Instantiate(gameManager);
            }
        }
    }
}
