using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();
    public int equipedIndex = 0;

    private GameObject equipedWeapon;
    private TankWeapon equipedWeaponComp;

    void Start() {
        equip(equipedIndex);
    }
    public void equip(int newIndex) {
        newIndex = (int)Mathf.Clamp(equipedIndex, 0, weapons.Count - 1);

        if (equipedWeapon != null && equipedWeapon.activeInHierarchy) {
            weapons[equipedIndex].SetActive(false);
            equipedWeapon = null;
            equipedWeaponComp = null;
        }

        equipedIndex = newIndex;
        weapons[equipedIndex].SetActive(true);
        equipedWeapon = weapons[equipedIndex];
        equipedWeaponComp = equipedWeapon.GetComponent<TankWeapon>();
    }
    public void triggerOn() {
        equipedWeaponComp.triggerOn();
    }
    public void triggerOff() {
        equipedWeaponComp.triggerOff();
    }
    public void equipNext() {
        equipedIndex += 1;
        if (equipedIndex >= weapons.Count) equipedIndex = 0;
        equip(equipedIndex);
    }
    public void equipPrevious() {
        equipedIndex -= 1;
        if (equipedIndex < 0) equipedIndex = weapons.Count - 1;
        equip(equipedIndex);
    }
}
