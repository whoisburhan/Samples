namespace GS.FanstayWorld2D.Enemy
{
    public interface IHealth
    {
        public void Init(HealthBarBehaviour healthUI);
        public void SetHealth(int health, int maxHealth);
        public bool UpdateHealth(int amount);
    }
}