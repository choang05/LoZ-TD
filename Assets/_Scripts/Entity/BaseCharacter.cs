using UnityEngine;
using System.Collections;

public class BaseCharacter : MonoBehaviour
{
    // Description
    [SerializeField] private string _name;

    // Stats
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float critChance;
    [SerializeField] private float critMultiplier;


    // Actuators and Mutators
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }
    public float CriticalChance
    {
        get { return critChance; }
        set { critChance = value; }
    }
    public float CritMultiplier
    {
        get { return critMultiplier; }
        set { critMultiplier = value; }
    }
}
