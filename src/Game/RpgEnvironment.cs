namespace Game;

public interface IRpgEnvironment
{
    void Exit();
}

public class RpgEnvironment: IRpgEnvironment
{
    public void Exit() => Environment.Exit(0);
}