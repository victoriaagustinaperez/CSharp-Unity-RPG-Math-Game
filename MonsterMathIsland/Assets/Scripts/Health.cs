using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int _hp;
    /* using Transform because there are two children inside the 
     * PlayerHP, Bg, red background, and HPBar, green foreground
     * to manipulate Fill Amount property*/
    [SerializeField] private Transform _healthBarUI;

    public int maxHp = 150;
    public UnityEvent onDeath;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthBarUI();
        //create a takedamage method, don't want to manipulate hp externally

    }

    public void TakeDamage(int damage)
    {
        //remove hp, update health bar, check if you did
        _hp -= damage;
        if(_hp <= 0)
        {
            _hp = 0; //to make sure hp never goes below 0
            UpdateHealthBarUI();
            onDeath.Invoke();
            return; //return out of method if _hp <= 0 and so it doesn't execute twice
        }
        UpdateHealthBarUI();
    }

    public int CalculateDamage(Stats attacker, Stats defender)
    {
        return Mathf.Max(1, attacker.attack - defender.defense); //if lower than 1, defaults to 1 to chip away at health so attack never deals 0 damage
    }

    public void SetHealthBar(Transform healthBarUi)
    {
        _healthBarUI = healthBarUi; //set to health bar UI to reference stored in Monster Manager
        UpdateHealthBarUI();
    }

    private void UpdateHealthBarUI()
    {
        if (!_healthBarUI) return; //to catch any errors from monsters that may not have health bar attached

        //Index(1) of _healthBarUI is HpBar, Image is component where green image is held
        _healthBarUI.GetChild(1).GetComponent<Image>().fillAmount = (float)_hp / maxHp;
        _healthBarUI.GetChild(2).GetComponent<TMP_Text>().text = $"{_hp} / {maxHp}";

    }
}