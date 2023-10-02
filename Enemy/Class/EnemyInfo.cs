using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Info", menuName = "GS/Enemy Info", order = 6)]
    public class EnemyInfo : ScriptableObject
    {
        public EnemySpecies enemySpecies;
        public AnimatorOverrideController controller;
        public EnemyAttackType enemyAttackType;
        public int enemyAtkPow;
        public float enemySpeed;
        public float enemySpeedAtChase;
        public int enemyMaxHealth;
        [Header("Abilities")]
        public bool canPerol;
        public bool canChase;
    }
}