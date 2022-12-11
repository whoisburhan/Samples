using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    public interface IPatrol
    {
        public Vector3 Patrolling(Vector3 position);
        public Vector3 Chasing(Vector3 position, Vector3 tagetPosition);
        public void UpdatePatrolPoints(Vector3 basePosition);
        public void SetPatrolSpeed(float patrolSpeed);
        public int UpdateNewPatrolPoint();
        public void CheckAndSetDirection(Vector3 target);
    }
}