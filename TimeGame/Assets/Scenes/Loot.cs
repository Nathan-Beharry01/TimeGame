using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Loot : MonoBehaviour
{
    [System.Serializable]
    public class DropCurrency
    {
        public string name;
        public GameObject item;
        public int dropRarity;
    }

    public List <DropCurrency> LootTable = new List<DropCurrency>

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
