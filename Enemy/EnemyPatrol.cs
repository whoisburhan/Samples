using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    public class EnemyPatrol : MonoBehaviour, IPatrol
    {
        [Header("Enemey Petrol Attribute")]
        [SerializeField] private Vector3[] patrolPoints;
        [SerializeField] private bool isPatrolRandomPoint;
        private bool isDirectionRight = true;

        private int currentPatrolPoint = 0;

        private float patrolSpeed = 2f;

        public void UpdatePatrolPoints(Vector3 basePosition)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                var temp = patrolPoints[i];
                patrolPoints[i] = basePosition + temp;
            }
        }

        public void SetPatrolSpeed(float patrolSpeed)
        {
            this.patrolSpeed = patrolSpeed;
        }

        public Vector3 Patrolling(Vector3 position)
        {

            if (Vector3.Distance(position, patrolPoints[currentPatrolPoint]) <= 0.3f)
            {
                currentPatrolPoint = UpdateNewPatrolPoint();
            }

            CheckAndSetDirection(patrolPoints[currentPatrolPoint]);
            return Vector2.MoveTowards(position, patrolPoints[currentPatrolPoint], patrolSpeed * Time.deltaTime);
        }

        public Vector3 Chasing(Vector3 position, Vector3 targetPosition)
        {
            CheckAndSetDirection(targetPosition);
            var yAxisNormalizedPosition = new Vector2(targetPosition.x,position.y);
            return Vector2.MoveTowards(position, yAxisNormalizedPosition, patrolSpeed * Time.deltaTime);
        }

        public int UpdateNewPatrolPoint()
        {
            if (!isPatrolRandomPoint)
            {
                return (currentPatrolPoint + 1) % patrolPoints.Length;
            }
            else
            {
                int _temp = UnityEngine.Random.Range(0, patrolPoints.Length);

                while (_temp == currentPatrolPoint && patrolPoints.Length > 1)
                {
                    _temp = UnityEngine.Random.Range(0, patrolPoints.Length);
                }

                return _temp;
            }
        }

        public void CheckAndSetDirection(Vector3 target)
        {
            bool isTargetDirectionRight = target.x - transform.position.x > 0f;

            if (isDirectionRight != isTargetDirectionRight)
            {
                transform.Rotate(0f, 180f, 0f);
                isDirectionRight = isTargetDirectionRight;
            }
        }

#if UNITY_EDITOR
        #region  Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0.3f, 0f); // Orange Color

            if (Application.isPlaying)
            {
                foreach (var patrolPoint in patrolPoints)
                {
                    Gizmos.DrawSphere(patrolPoint, 0.3f);
                }
            }
            else
            {
                foreach (var petrolPoint in patrolPoints)
                {
                    Gizmos.DrawSphere(transform.position + petrolPoint, 0.3f);
                }
            }
        }

        #endregion
#endif
    }
}