using System.Linq;
using TMPro;
using UnityEngine;

public class BulletDisplay : MonoBehaviour
{
    public TMP_Text InventoryCount;
    public Weapon[] Weapons;

    // Update is called once per frame
    void Update()
    {
        var weapon = Weapons.First(x => x.isActiveAndEnabled);
        var ammo = weapon.Bullets == Constants.InfiniteAmmo
            ? "∞"
            : weapon.Bullets.ToString();
        InventoryCount.text = ammo;
    }
}