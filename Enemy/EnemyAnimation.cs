using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
    public class EnemyAnimation : MonoBehaviour, IEnmeyAnimation
    {
        private Animator animator;
        private int emeyAnimationState;

        public EnemyAnimation(Animator animator)
        {
            this.animator = animator;

            emeyAnimationState = Animator.StringToHash("_STATE");
        }

        public void Init(Animator animator)
        {
            this.animator = animator;

            emeyAnimationState = Animator.StringToHash("_STATE");
        }

        public void UpdateAnimationState(EnemyState state)
        {
            animator.SetInteger(emeyAnimationState, (int)state);
        }
    }
}