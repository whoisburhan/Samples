using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GS.AudioAsset;
using GS.FanstayWorld2D.Projectile;


namespace GS.FanstayWorld2D.Enemy
{
    public enum EnemyState
    {
        NONE, IDLE, PATROL, CHASE, ATTACK_SHORT_RANGE, ATTACK_LONG_RANGE, HURT, DIE
    }

    [RequireComponent(typeof(Animator), typeof(AudioSourceScript), typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy Attributes")]
        private EnemySpecies enemySpecies;
        private ISensor enemySensor;
        private IPatrol enemyPetrol;
        private IEnmeyAnimation enmeyAnimation;
        private IHealth enemeyHealth;
        private Animator animator;
        private EnemyState currentState;
        private EnemyAttackType enemyAttackType;
        private ProjectileType projectileType;
        private bool canCheckState = true;
        private bool canPetrol = true, canChase;
        private int attackPower = 10; // just for taste purpose
        private Vector2 projectileSpawnPointOffset;

        #region Unity Default Functions
        private void Awake()
        {
            enemeyHealth = GetComponent<IHealth>();
            enemeyHealth.Init(GetComponentInChildren<HealthBarBehaviour>());
            animator = GetComponent<Animator>();
            enemySensor = GetComponent<ISensor>();
            enemyPetrol = GetComponent<IPatrol>();
            enmeyAnimation = GetComponent<IEnmeyAnimation>();
            enmeyAnimation.Init(animator);

            OnAwakeCall();
        }

        private void Start()
        {
            OnStartCall();
            Init();
        }

        private void Update()
        {
            OnUpdateCall();
        }

        #endregion

        #region Unity Deafult's Alternatives Fuctions
        protected virtual void OnAwakeCall() { }
        protected virtual void OnStartCall()
        {
            enemyPetrol.UpdatePatrolPoints(transform.position);
        }
        protected virtual void OnEnableCall() { }
        protected virtual void OnUpdateCall()
        {
            if (canCheckState)
            {
                var target = enemySensor.CheckEnemyAttackZoneSensor(transform.position);
                if (target != null)
                {
                    /* Attack */
                    currentState = EnemyState.ATTACK_SHORT_RANGE;
                    enmeyAnimation.UpdateAnimationState(currentState);
                }
                else
                {
                    target = enemySensor.CheckEnemeyDetectZoneSensor(transform.position);
                    if (target != null)
                    {
                        if (enemyAttackType == EnemyAttackType.LONG_RANGE || enemyAttackType == EnemyAttackType.BOTH)
                        {
                            // Attack Long Range
                            currentState = EnemyState.ATTACK_LONG_RANGE;
                            enmeyAnimation.UpdateAnimationState(currentState);
                        }
                        else if (canChase)
                        {
                            // chase enemy
                            if (currentState != EnemyState.CHASE)
                            {
                                currentState = EnemyState.CHASE;
                                enmeyAnimation.UpdateAnimationState(currentState);
                            }

                            var targetPosition = enemySensor.CheckEnemeyDetectZoneSensor(transform.position);
                            
                            if (targetPosition != null)
                                transform.position = enemyPetrol.Chasing(transform.position, targetPosition.position);
                        }
                        else if (canPetrol)
                        {
                            // Patrol
                            if (currentState != EnemyState.PATROL)
                            {
                                currentState = EnemyState.PATROL;
                                enmeyAnimation.UpdateAnimationState(currentState);
                            }

                            transform.position = enemyPetrol.Patrolling(transform.position);
                        }
                    }

                    else if (canPetrol)
                    {
                        // Patrol
                        if (currentState != EnemyState.PATROL)
                        {
                            currentState = EnemyState.PATROL;
                            enmeyAnimation.UpdateAnimationState(currentState);
                        }

                        transform.position = enemyPetrol.Patrolling(transform.position);
                    }
                }
            }
        }
        protected virtual void OnDisableCall() { }
        #endregion

        #region Function's that will use by the animator/animation
        public void UpdateCheckStatePermission(int permission)
        {
            canCheckState = permission > 0;
        }

        public void GiveShortRangeDamage()
        {
            GetOpponent()?.SendMessage("Damage", attackPower);
        }
        private Transform GetOpponent()
        {
            return enemySensor.CheckEnemyAttackZoneSensor(transform.position);
        }

        public void GiveLongRangeDamage()
        {
            GameObject go = ProjectileController.Instance.GetProjectile(projectileType);
            go.SetActive(false);
            go.transform.position = transform.position + (Vector3)projectileSpawnPointOffset;
            go.SetActive(true);
        }

        protected void DeactivateEnemy()
        {
            gameObject.SetActive(false);
        }
        #endregion


        #region Received Data by Send Message Call
        public void Damage(int damageAmount)
        {
            var isStilAlive = enemeyHealth.UpdateHealth(damageAmount);

            switch (isStilAlive)
            {
                case true:
                    currentState = EnemyState.HURT;
                    enmeyAnimation.UpdateAnimationState(currentState);
                    break;
                case false:
                    if (currentState != EnemyState.DIE)
                    {
                        currentState = EnemyState.DIE;
                        enmeyAnimation.UpdateAnimationState(currentState);
                        canCheckState = false;
                    }
                    break;
            }
        }

        #endregion

        private void Init()
        {
            // Dummy Value For the test
            enemeyHealth.SetHealth(20, 20);
            canCheckState = true;
            canPetrol = true;
            canChase = true;
        }
    }





    public enum EnemyAttackType
    {
        SHORT_RANGE, LONG_RANGE, BOTH
    }

    public enum EnemySpecies
    {
        SKELTON
    }
}