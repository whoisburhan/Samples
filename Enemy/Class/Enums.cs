namespace GS.FanstayWorld2D.Enemy
{
    public enum EnemyAttackType
    {
        SHORT_RANGE, LONG_RANGE, BOTH
    }

    public enum EnemySpecies
    {
        SKELTON, VAMPIRE_MALE, VAMPIRE_MALE_2
    }

    public enum EnemyState
    {
        NONE, IDLE, PATROL, CHASE, ATTACK_SHORT_RANGE, ATTACK_LONG_RANGE, HURT, DIE, JUMP, FLY
    }
}