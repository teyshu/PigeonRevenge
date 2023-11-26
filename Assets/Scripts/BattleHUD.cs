using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text _nameText;
    public Text _levelText;
    public Slider _hpSlider;

    public void SetHUD(Unit unit)
    {
        _nameText.text = unit._name;
        _levelText.text = "Óð. " + unit._level.ToString();
        _hpSlider.maxValue = unit._maxHP;
        _hpSlider.value = unit._currentHP;
    }

    public void SetHP(int hp)
    {
        _hpSlider.value = hp;
    }
}
