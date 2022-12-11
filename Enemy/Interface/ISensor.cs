using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    public interface ISensor
    {
        public Transform CheckEnemeyDetectZoneSensor(Vector3 basePosition);
        public Transform CheckEnemyAttackZoneSensor(Vector3 basePosition);
    }
}