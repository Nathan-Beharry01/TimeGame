using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loottable : MonoBehaviour
{
    public int[] table = {
        60,
        30,
        10
    };
    // Start is called before the first frame update
    public int total;
    private void Start()
    {
        /*foreach (var item in table)
        {
            total += item;
        }*/
    }
}