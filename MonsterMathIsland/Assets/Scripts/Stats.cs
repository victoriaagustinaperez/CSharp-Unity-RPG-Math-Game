using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Stats : MonoBehaviour
{
    public int level = 1;
    public int exp;

    public int baseMaxHp = 54;
    public int baseAttack = 10;
    public int baseDefense = 10;

    public int maxHp;
    public int attack;
    public int defense;

    public Image expbar; //blue bar under player Health stat
    public GameObject levelUpWindow;

    private int nextLvUp; //exp remaining to get to next level
    private int prevLvUp; //exp it took to get to current level

    //calculate stats for each level. test with a for loop using level to the power value to scale exp, attack, defense to not manually add stats on a per level basis

    // Start is called before the first frame update
    void Start()
    {
        CalculateStats();
        exp = prevLvUp; //exp when level starts
    }

    private void CalculateStats()
    {
        prevLvUp = (int)Mathf.Pow(level, 4f);
        nextLvUp = (int)Mathf.Pow(level +1, 4f);
        maxHp = baseMaxHp + ((int)Mathf.Pow(level, 1.2f) * 10) + (int)Random.Range(0, 10);
        attack = baseAttack + (int)Mathf.Pow(level, 1.15f);
        defense = baseDefense + (int)Mathf.Pow(level, 1.15f);

    }

    public void GainExp(int amount)
    {
        int prevExp = exp;
        exp += amount;

        StartCoroutine(LerpExpBar(prevExp));

        LerpExpBar(prevExp);

        if (exp >= nextLvUp)
        {
            LevelUp();
        }

    }

    private IEnumerator LerpExpBar(int prevExp)
    {
        if (!expbar) yield break; //breaks out of coroutine. return would run code below on next frame
       
        float t = 0f;
        while(t < 1)
        {
        
            t += Time.deltaTime;
            float currentLevelExp = exp - prevLvUp; //current level
            float prevLevelExp = prevExp - prevLvUp;

            float totalLevelExp = nextLvUp - prevLvUp; //range from level to level

            float mappedExp = currentLevelExp / totalLevelExp;
            float mappedPrevExp = prevLevelExp / totalLevelExp;

            expbar.fillAmount = Mathf.Lerp(mappedPrevExp, mappedExp, 0.5f);
            yield return null;
        }
    }

    public void LevelUp()
    {
        //variables to have reference to stats before level up
        float prevMaxHp = maxHp;
        float prevAttack = attack;
        float prevDefense = defense;


        level++;
        CalculateStats();

        if (!levelUpWindow) return;

        levelUpWindow.SetActive(true);
        levelUpWindow.transform.GetChild(1).GetComponent<TMP_Text>().text = $"Level: {level}\r\n + Max HP: {maxHp}(+{maxHp - prevMaxHp})\r\n + Attack: {attack}(+{attack - prevAttack})\r\n + Defense: {defense}(+{defense - prevDefense})";

        Invoke(nameof(CloseLevelUpWindow), 6f); //close level up window in 6 seconds
    }

    private void CloseLevelUpWindow()
    {
        levelUpWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

