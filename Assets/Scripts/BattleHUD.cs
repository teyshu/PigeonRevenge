using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text _nameText;
    public Text _levelText;
    public Slider _hpSlider;

    public void SetHUD(Unit unit)
    {
        _nameText.text = unit.Name;
        _levelText.text = "Ур. " + unit.Level.ToString();
        _hpSlider.maxValue = unit.MaxHp;
        _hpSlider.value = unit.CurrentHp;
    }

    public void SetHp(int hp)
    {
        _hpSlider.value = hp;
    }
}
