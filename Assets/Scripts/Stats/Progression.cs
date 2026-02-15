using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;
    private Dictionary<CharacterClass, Dictionary<Stat, int[]>> lookupTable = null;

    public int GetStat(Stat stat, CharacterClass characterClass, int level)
    {
        BuildLookup();

        int[] levels = lookupTable[characterClass][stat];

        if(levels.Length < level)
        {
            return 0;
        }

        return levels[level - 1];
    }

    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        BuildLookup();
        int[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }

    private void BuildLookup()
    {
        if(lookupTable != null)
        {
            return;
        }

        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, int[]>>();

        foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
        {
            Dictionary<Stat, int[]> statLookupTable = new Dictionary<Stat, int[]>();

            foreach (ProgressionStat stat in progressionCharacterClass.stats)
            {
                statLookupTable.Add(stat.Stat, stat.Levels);
            }

            lookupTable.Add(progressionCharacterClass.characterClassName, statLookupTable);
        }
    }

    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClassName;
        public ProgressionStat[] stats;
    }

    [System.Serializable]
    class ProgressionStat
    {
        public Stat Stat;
        public int[] Levels;
    }
}
