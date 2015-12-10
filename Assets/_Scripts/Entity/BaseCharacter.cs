using UnityEngine;
using System.Collections;

public class BaseCharacter : MonoBehaviour
{
    // Description
    [SerializeField] private string _name;

    // Stats
    [SerializeField] private int _attack;
    [SerializeField] private int _defense;

    // Actuators and Mutators
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public int Attack
    {
        get { return _attack; }
        set { _attack = value; }
    }
    public int Defense
    {
        get { return _defense; }
        set { _defense = value; }
    }
}
