public static class GameGlobalVar
{
    public static float stormRadius;

    static GameGlobalVar()
    {
        Reset();
    }

    public static void Reset()
    {
        stormRadius = 30;
    }
}
