using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletDisplay : MonoBehaviour
{
    public TMP_Text CurentCount;
    public TMP_Text InventoryCount;
    public Weapon Weapon;

    // Update is called once per frame
    void Update()
    {
        CurentCount.text = Weapon.BulletsInMagazine.ToString();
        InventoryCount.text = Weapon.BulletsInInventory.ToString();
    }
}
