using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Assault Level", menuName = "ScriptableObjects/AssaultLevel")]
public class AssaultLevels : ScriptableObject
{
    public enum LevelConidtions
    {
        isHeadShot,
        isMinHealth,
        isBarrelExplosion,
        isMinKills,
        isGrenadeKill,
        isMinBullets,
        isSniperKill,
        isCollectDrop,
        isDestroyChopper,
        isAccuracy,
        isObjective,
        isTime,
        totalCount
    }
    [Serializable]
    public class ConditionContainer
    {
        public LevelConidtions condition;
        public float value,multipliyer;
        public bool isSmaller = false;
    }
    public ConditionContainer[] Conditions;
    public int minKills;
    public int[] GunsIndexes;
    
}
