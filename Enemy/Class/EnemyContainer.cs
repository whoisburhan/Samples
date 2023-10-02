using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    public class EnemyContainer : MonoBehaviour
    {
        public static EnemyContainer Instance {get; private set;}
        public List<EnemyInfo> enemyList;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public EnemyInfo GetEnemyInfo(EnemySpecies enemySpecies)
        {
            foreach(var enemyInfo in enemyList)
            {
                if(enemyInfo.enemySpecies == enemySpecies)
                {
                    return enemyInfo;
                }
            }
            
            return null;
        }
    }
}