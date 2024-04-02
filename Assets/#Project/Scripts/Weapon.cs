using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    [Space(5)]
    public string _name;
    [Space(10)]
    public int _goldPrice;
    public int _spPrice;
    [Space(10)]
    public float _baseDamage;
    public float _maxDamage;
    public float _baseReloadTime;
    public float _maxReloadTime;
    public float _baseZoom;
    public float _maxZoom;
    public float _baseMagzineSize;
    public float _maxMagzineSize;
    public float _baseNumberOfMagzine;
    public float _maxNumberOfMagzine;
    public float _baseFireRate;
    public float _maxFireRate;
    public float _baseRange;
    public float _maxRange;
    public float _baseAccuracy;
    public float _maxAccuracy;
    public float _baseSpread;
    public float _MaxSpread;
    public float _baseKickUp;
    public float _baseKickSide;
    public float _baseClimbSide;
    public bool _baseViewClimbOff;
    [Space(10)]
    public bool _purchased;
    public bool _equipped;
    public bool isComingSoon = true;
    public int PrefsIndex = 0;
    public string DiscountPriceID;
    public string RealPriceID;
    public string Category = "Default";
    public int OfferIndex = 0;
    public string Desc = "";
    public string Code = "";
    public Color ClassColor;
    public string Class;
    public float perkPoints;
    public int adCount;
}
