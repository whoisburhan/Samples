namespace GS.FanstayWorld2D.Enemy
{
    public interface IHealthBarBehaviour:IHealth
    {
        UpdateHealthInUI(float health, float maxHealth);
    }
}