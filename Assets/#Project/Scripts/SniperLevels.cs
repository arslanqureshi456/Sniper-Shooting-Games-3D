using UnityEngine;

[CreateAssetMenu(fileName = "Sniper Level", menuName = "ScriptableObjects/SniperLevel")]

public class SniperLevels : ScriptableObject
{
    public enum StartStates{
        isStatic,
        isWalking,
        isSitting,
        isDelayedRetaliate,
        isRunning
    }
    public enum BulletReactions
    {
        alert,
        retaliate,
        run,
        none
    }
    public StartStates start;
    public BulletReactions firstBullet, secondBullet;
    public float retaliateDelay;
}
