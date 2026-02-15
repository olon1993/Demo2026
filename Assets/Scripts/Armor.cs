using System;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int defense;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defense = 10;
        Debug.Log(defense);
        Console.WriteLine(defense);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
