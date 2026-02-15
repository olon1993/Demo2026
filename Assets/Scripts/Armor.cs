using System;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int defense;
    [SerializeField] private int stamina;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defense = 10;
        Debug.Log(defense);
        Console.WriteLine(defense);

        stamina = 10;
        Debug.Log(stamina);
        Console.WriteLine(stamina);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
