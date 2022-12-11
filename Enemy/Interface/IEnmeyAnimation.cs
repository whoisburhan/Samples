using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    public interface IEnmeyAnimation
    {
        public void Init(Animator animator);
        public void UpdateAnimationState(EnemyState state);
    }
}