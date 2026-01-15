using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon {
    public string Name;
    public bool IsGoted;
    public GameObject WeaponObj;
    public GameObject WeaponPanel;
}

public class InventoryController : MonoBehaviour {
    [SerializeField] private int index = 0;
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    private Weapon currentWeapon;

    private void Start() {
        if (weapons is not { Count: > 0 }) return;
        if (weapons[index].IsGoted) ActivateWeapon(index);
        else SelectFirstAvailable();
    }

    public void SwitchWeapon(int _index) {
        if (_index < 0 || _index >= weapons.Count || _index == index) index = 0;
        if (!weapons[_index].IsGoted) {
            Debug.Log($"{weapons[_index].Name} didn't goted!");
            return;
        }
        
        ActivateWeapon(_index);
    }

    private void ActivateWeapon(int _index) {
        if (currentWeapon != null) {
            if (currentWeapon.WeaponObj) currentWeapon.WeaponObj.SetActive(false);
            if (currentWeapon.WeaponPanel) currentWeapon.WeaponPanel.SetActive(false);
        }

        if (weapons[_index] is { } _selected) {
            if (_selected.WeaponObj) _selected.WeaponObj.SetActive(true);
            if (_selected.WeaponPanel) _selected.WeaponPanel.SetActive(true);
            
            currentWeapon = _selected;
            index = _index;
        }
    }

    private void SelectFirstAvailable() {
        for (int i = 0; i < weapons.Count; i++) {
            if (weapons[i].IsGoted) {
                ActivateWeapon(i);
                break;
            }
        }
    }
}
