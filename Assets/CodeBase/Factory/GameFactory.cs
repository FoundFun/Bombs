using System;
using System.Collections.Generic;
using CodeBase.BombLogic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Factory
{
    public class GameFactory
    {
        public List<Bomb> PoolBombs;
        
        public void CreatePoolBomb(Bomb typeBomb, Transform container, int count)
        {
            PoolBombs = new List<Bomb>();
            
            for (int i = 0; i < count; i++)
            {
                Bomb bomb = Object.Instantiate(typeBomb, container.position, Quaternion.identity, container);
                bomb.gameObject.SetActive(false);
                PoolBombs.Add(bomb);
            }
        }
    }
}
