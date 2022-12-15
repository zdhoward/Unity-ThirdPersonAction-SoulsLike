using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] GameObject weaponCollider;

    public void EnableWeapon()
    {
        weaponCollider.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponCollider.SetActive(false);
    }
}
