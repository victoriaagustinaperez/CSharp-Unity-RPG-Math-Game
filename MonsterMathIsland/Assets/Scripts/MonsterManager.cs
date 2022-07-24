using System.Collections;
using System.Collections.Generic;
using TMPro;
using TutorialAssets.Scripts;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _queuePoint;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] Transform _killedPoint;
    [SerializeField] private int _amountOfMonsters = 20;
    [SerializeField] private GameObject[] _monsterPrefabs;
    [SerializeField] private float waveDifficulty;
    [SerializeField] private Transform _healthBarUI;
    [SerializeField] private TMP_Text _roundTextUI;

    private int _currentRound = 0; //0 because increments to 1 on Awake()
    private int _originalAmountOfMonsters;

    public List<GameObject> _monsters;

    // Start is called before the first frame update
    private void Awake()
    {
        _originalAmountOfMonsters = _amountOfMonsters;
        StartWave();
    }

    public void StartWave() 
    {
        _currentRound++;
        _roundTextUI.text = "Wave" + _currentRound;
        _amountOfMonsters = _originalAmountOfMonsters + Mathf.FloorToInt(_currentRound / 2f);

        for (var i = 0; i < _amountOfMonsters; i++)
        {
            InstantiateMonster();
        }

        MonsterAttacks(0);
        MoveNextMonsterToQueue();

        CalculateWaveDifficulty(ref waveDifficulty);
    }

    //Returns a value between 0 to 1 for the difficulty of this monster wave
    private float CalculateWaveDifficulty(ref float difficulty)
    {
        foreach (var monster in _monsters)
        {
            difficulty += monster.GetComponent<MonsterController>().points;
        }

        difficulty /= (_amountOfMonsters * 3); //use 3 as 3 is the maximum points a single monster can yield

        return difficulty;
    }

    private void InstantiateMonster()
    {
        //difficulty logic
        int max = _monsterPrefabs.Length;
        if (_currentRound < 2) max = 1;
        else if (_currentRound > 1 && _currentRound < 4) max = 2;
        else if (_currentRound > 3) max = 3;

        int monsterIndex = Mathf.FloorToInt(Random.Range(0, max));
        var monster = Instantiate(_monsterPrefabs[monsterIndex],_spawnPoint.position,Quaternion.identity).GetComponent<Stats>();
        monster.level = Random.Range(0, _currentRound+1);
        monster.LevelUp();
        
        _monsters.Add(monster.gameObject);
    }

    private void MoveMonsterToQueuePoint(int monsterIndex)
    {
        if (_monsters.Count <= monsterIndex) return;
        
        Transform monster = _monsters[monsterIndex].transform;
        monster.GetComponent<MonsterController>().state = MonsterState.Queue;
        StartCoroutine(LerpToPosition(monster, _queuePoint.position, _queuePoint.rotation, 0.3f));
    }

    private IEnumerator LerpToPosition(Transform t, Vector3 position, Quaternion rotation, float speed) //coroutine to move monsters
    {
        //find distance between 2 posiitions, object and destination
        float distToPos = Vector3.Distance(t.position, position);
        float timer = 0;

        while(distToPos > 0.1f) //coroutines can tell loop to break for a frame and it will continue its execution in the next frame
        {
            if (!t) yield break; //if we don't h ave a transform, exits coroutine

            t.position = Vector3.Lerp(t.position, position, timer*speed); //(lerp from, lerp to, where we are in movement or % between 2 positions)
            t.rotation = rotation;
            timer += Time.deltaTime;

            yield return null; //needed for couroutines, what breaks from frame to reinitiate on next frame
        }

    }

    /*  Adding this UnityEvent allows for logic to be decoupled from
        QuestionManager and brought to this script in MonsterDeath method.*/

    private void KillMonster(int monsterIndex)
    {
        GameObject monster = _monsters[monsterIndex];
        StopAllCoroutines();
        StartCoroutine(LerpToPosition(monster.transform, _killedPoint.position, monster.transform.rotation, 0.5f));
        StartCoroutine(DestroyMonster(monster, 1f));
        _monsters.RemoveAt(monsterIndex);
    }

    private IEnumerator DestroyMonster(GameObject monster, float seconds)
    {
        yield return new WaitForSeconds(seconds); //destroyed after 2 seconds
        Destroy(monster);
    }

    private void MonsterAttacks(int monsterIndex)
    {
        if (_monsters.Count <= monsterIndex) return;
        
        Transform monster = _monsters[monsterIndex].transform;
        monster.GetComponent<MonsterController>().state = MonsterState.Attack;
        StartCoroutine(LerpToPosition(monster, _attackPoint.position, _attackPoint.rotation, 0.3f));

        //assign monster's health to health bar in UI WHEN it steps up to attack
        var monsterHealth = monster.GetComponent<Health>();
        monsterHealth.SetHealthBar(_healthBarUI); //to set to health bar UI in MonsterManager. refreshes render of health bar to show new monster's health
        monsterHealth.onDeath.AddListener(MonsterDeath);
    }

    private void MonsterDeath()
    {
        KillMonster(0);
        MonsterAttacks(0);
        MoveNextMonsterToQueue();
    }

    private void MoveNextMonsterToQueue()
    {
        if (_monsters.Count <= 1) return;
        
        MoveMonsterToQueuePoint(1);
    }

    public bool IsMonsterListEmpty()
    {
        return _monsters.Count == 0;
    }

    public MonsterType GetMonsterType(int monsterIndex)
    {
        return _monsters[monsterIndex].GetComponent<MonsterController>().type;
    }
}
