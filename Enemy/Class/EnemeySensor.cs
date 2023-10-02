using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    public class EnemeySensor : MonoBehaviour, ISensor
    {
        [SerializeField] private LayerMask sensorMask;

        [Header("Enemy Chase Zone Detector Attritube")]
        [SerializeField] private Vector2 chaseZone;
        [SerializeField] private Vector2 chaseZoneOffeset;
        [SerializeField] private float chaseZoneAngle;

        [Header("Enemy Attack Zone Detector Attritube")]
        [SerializeField] private Vector2 attakZone;
        [SerializeField] private Vector2 attakZoneOffset;
        [SerializeField] private float attakZoneAngle;

        public Transform CheckEnemyAttackZoneSensor(Vector3 basePosition)
        {
            return CheckSensore((Vector2)basePosition + attakZoneOffset, attakZone, attakZoneAngle, sensorMask);
        }

        public Transform CheckEnemeyDetectZoneSensor(Vector3 basePosition)
        {
            return CheckSensore((Vector2)basePosition + chaseZoneOffeset, chaseZone, chaseZoneAngle, sensorMask);
        }

        private Transform CheckSensore(Vector2 point, Vector2 size, float angle, LayerMask mask)
        {
            Collider2D[] detectedObjs = Physics2D.OverlapBoxAll(point, size, angle, mask);
            return detectedObjs.Length > 0 ? detectedObjs[0].transform : null;
        }

#if UNITY_EDITOR
        #region  Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + attakZoneOffset, attakZone);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((Vector2)transform.position + chaseZoneOffeset, chaseZone);
        }

        #endregion
#endif
    }
}