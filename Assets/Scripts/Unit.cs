using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string _name;
    public int _level;

    public int _damage;
    public int _currentHP;
    public int _maxHP;

    public bool TakeDamage(int dmg)
    {
        _currentHP -= dmg;

        if (_currentHP <= 0) return true;
        else return false;
    }
}
