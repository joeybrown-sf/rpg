namespace Game;

public class RpgConfiguration
{
    public int HeroHealth { get; set; }
    public int HeroMinAttack { get; set; }
    public int HeroMaxAttack { get; set; }
    public int MonsterHealth { get; set; }
    public int MonsterMinAttack { get; set; }
    public int MonsterMaxAttack { get; set; }
    public TimeSpan AttackDelay { get; set; }
}
