using System.Linq;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DefaultNamespace
{
    [RequireComponent(typeof(StarterAssetsInputs))]
    public class WeaponInventory : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] weapons;

        [SerializeField]
        private RigBuilder rigBuilder;

        private IWeapon[] _weapons;
        private int _currentSelected = 0;

        private StarterAssetsInputs inputs;

        private void Start()
        {
            inputs = GetComponent<StarterAssetsInputs>();
            inputs.OnNextWeapon.AddListener(ChooseNext);
            inputs.OnPrevWeapon.AddListener(ChoosePrev);
            _weapons = weapons.Select(x => x.GetComponent<IWeapon>()).ToArray();
        }


        private void ChooseNext()
        {
            weapons[_currentSelected].SetActive(false);
            rigBuilder.layers[_currentSelected].active = false;

            _currentSelected = GetIndex(++_currentSelected);
            var weapon = _weapons[_currentSelected];
            while (!weapon.HasAmmo())
            {
                _currentSelected = GetIndex(++_currentSelected);
                weapon = _weapons[_currentSelected];
            }

            weapons[_currentSelected].SetActive(true);
            rigBuilder.layers[_currentSelected].active = true;
        }

        private void ChoosePrev()
        {
            weapons[_currentSelected].SetActive(false);
            rigBuilder.layers[_currentSelected].active = false;

            _currentSelected = GetIndex(--_currentSelected);
            var weapon = _weapons[_currentSelected];
            while (!weapon.HasAmmo())
            {
                _currentSelected = GetIndex(--_currentSelected);
                weapon = _weapons[_currentSelected];
            }

            weapons[_currentSelected].SetActive(true);
            rigBuilder.layers[_currentSelected].active = true;
        }

        private int GetIndex(int index)
        {
            if (index > weapons.Length - 1)
            {
                return 0;
            }

            if (index < 0)
            {
                return weapons.Length - 1;
            }

            return index;
        }

        public void AddAmmo(string weaponName, int amount)
        {
            var weapon = _weapons.First(x => x.WeaponName == weaponName);
            weapon.RefillAmmo(amount);
        }
    }
}