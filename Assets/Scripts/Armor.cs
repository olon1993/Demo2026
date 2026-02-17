using UnityEngine;
using System;



public class Armor : MonoBehaviour {
      
    public class Defenses {
        [SerializeField]
        public int defenseRating = 0;
        [SerializeField]
        public int fireResistance;
        [SerializeField]
        public int waterResistance;
        [SerializeField]
        public int lightningResistance; 
        [SerializeField]
        public int earthResistance;

    }
 public class Attributes
    {
        [SerializeField] 
      public int intellect;
        [SerializeField]
      public int stamina;
        [SerializeField]
      public int agility;
        [SerializeField]
      public int strength;

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
