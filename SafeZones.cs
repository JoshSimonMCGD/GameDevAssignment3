namespace MohawkGame2D;

public class SafeZone
{
    public float X;
    public float Width;

    public SafeZone(float x, float width)
    {
           X = x;
           Width = width;
    }

    public bool IsPlayerSafe(float playerX)
    {
        return playerX >= X && playerX <= X + Width;
    }
}