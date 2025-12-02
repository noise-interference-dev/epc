using UnityEngine;

public interface IWeapon
{
    string WeaponName { get; }
    Sprite WeaponSprite { get; }
    float FireRate { get; }
    float Distance { get; }
    int Damage { get; }
    AmmoType AmmoType { get; }
    int CurrentAmmo { get; }
    int MaxAmmo { get; }
    Vector3 DefaultPosition { get; }
    void Shoot();
    void Reload();
}