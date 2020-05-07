using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] loadout;
    public Transform weaponParent;

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    void Equip(int weaponID)
    {
        GameObject t_newEquiptment = Instantiate(loadout[weaponID], weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
    }
}
