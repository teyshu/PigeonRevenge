using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemController : MonoBehaviour
{
    public BattleState state;

    public GameObject _player;
    public GameObject _enemy;

    public Transform _playerPosOne;
    public Transform _playerPosTwo;
    public Transform _playerPosThree;
    public Transform _enemyPos;

    public BattleHUD _playerHUD;
    public BattleHUD _enemyHUD;

    public Text _dialogueText;

    private bool _canHelp = true;
    private Unit _enemyUnit;
    private Unit _playerUnit;
    private List<Unit> _playerUnits;
    private List<GameObject> _playersGameObjects;

    private void Start()
    {
        state = BattleState.Start;
        _playerUnits = new List<Unit>();
        _playersGameObjects = new List<GameObject>();
        StartCoroutine(SetupBattle());
    }
    
    public void OnAttackButton()
    {
        if (state != BattleState.Playerturn) return;
        
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.Playerturn) return;
        
        StartCoroutine(PlayerHeal());
    }
    
    public void OnHelpButton()
    {
        if (state != BattleState.Playerturn) return;

        if (_canHelp)
        {
            StartCoroutine(PlayerHelp());
        }
        else
        {
            _dialogueText.text = "Братанов больше нет, теперь только ты и он!";
        }
    }
    
    private IEnumerator SetupBattle()
     {
       GameObject _playerGO = Instantiate(_player, _playerPosOne);
        _playerUnit = _playerGO.GetComponent<Unit>();

        GameObject _enemyGO = Instantiate(_enemy, _enemyPos);
        _enemyUnit = _enemyGO.GetComponent<Unit>();

        _dialogueText.text = "Страшный и ужасный... " + _enemyUnit.Name + "! появился у вас на пути...";
        
        _playerUnits.Add(_playerUnit);
        _playersGameObjects.Add(_playerGO);
        
        _playerHUD.SetHUD(_playerUnit);
        _enemyHUD.SetHUD(_enemyUnit);

        yield return new WaitForSeconds(2);

        state = BattleState.Playerturn;
        PlayerTurn();
     }

    private void EndBattle()
    {
        if(state == BattleState.Won)
        {
            _dialogueText.text = "Голубь отомстил повару! Победа!";
            _playerUnit.EnableFatallityAnim();
        }
        else if (state == BattleState.Lost)
        {
            _dialogueText.text = "Повар порезал голубя на шаурму!!!";
        }
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2);
        _dialogueText.text = _enemyUnit.Name + " атакует...";
        _enemyUnit.EnableAttackAnim();

        yield return new WaitForSeconds(2);

        bool _isPlayerDead = true;
        
        foreach (var player in _playerUnits)
        {
            _isPlayerDead = player.TakeDamage(_enemyUnit.Damage);
            break;
        }

        _playerHUD.SetHp(_playerUnit.CurrentHp);
        
        _enemyUnit.DisableAttackAnim();

        yield return new WaitForSeconds(1);

        if (_isPlayerDead)
        {
            BirdDie();

            if (_playersGameObjects.Count == 0)
            {
                state = BattleState.Lost;
                EndBattle();
            }
            else
            {
                state = BattleState.Playerturn;
                PlayerTurn();
            }
        }
        else
        {
            state = BattleState.Playerturn;
            PlayerTurn();
        }
    }

    private void BirdDie()
    {
        for (int i = 0; i < _playersGameObjects.Count; i++)
        {
            Destroy(_playersGameObjects[i]);
            _playersGameObjects.RemoveAt(i);
            break;
        }
            
        for (int i = 0; i < _playerUnits.Count; i++)
        {
            Destroy(_playerUnits[i]);
            _playerUnits.RemoveAt(i);
            break;
        }
            
        for (int i = 0; i < _playerUnits.Count; i++)
        {
            _playerHUD.SetHp(_playerUnits[i].CurrentHp);
            break;
        }
    }

    private IEnumerator PlayerAttack()
    {
        bool isEnemyDead = false;
        
        foreach (var player in _playerUnits)
        {
            player.EnableAttackAnim();
            isEnemyDead = _enemyUnit.TakeDamage(player.Damage);
        }
        
        _enemyHUD.SetHp(_enemyUnit.CurrentHp);
        _dialogueText.text = "Клюнул прямо в лоб!";

        yield return new WaitForSeconds(1);
        
        foreach (var player in _playerUnits)
        {
            player.DisableAttackAnim();
        }
        
        
        if (isEnemyDead)
        {
            state = BattleState.Won;
            EndBattle();
        }
        else
        {
            state = BattleState.Enemyturn;

            if (Random.Range(0, 15) == 7)
            {
                StartCoroutine(EnemyHeal()); ;
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }
        }

        yield return new WaitForSeconds(2);
    }

    private IEnumerator PlayerHeal()
    {
        _dialogueText.text = "Голубь клюет семки...";
        yield return new WaitForSeconds(1);
        
        foreach (var player in _playerUnits)
        {
            player.EnableHealAnim();
            player.CurrentHp = player.MaxHp;
        }
        
        _dialogueText.text = "Голубь восстановил здоровье!";
        _playerHUD.SetHp(_playerUnit.CurrentHp);
        yield return new WaitForSeconds(1);

        state = BattleState.Enemyturn;
        
        foreach (var player in _playerUnits)
        {
            player.DisableHealAnim();
        }
        
        StartCoroutine(EnemyTurn());
    }
    
    private IEnumerator PlayerHelp()
    {
        _canHelp = false;
        _dialogueText.text = "Голубь зовет братанов...";
        yield return new WaitForSeconds(1);
        
        GameObject player2GO = Instantiate(_player, _playerPosTwo);
        Unit player2Unit = player2GO.GetComponent<Unit>();

        GameObject player3GO = Instantiate(_player, _playerPosThree);
        Unit player3Unit = player3GO.GetComponent<Unit>();

        _playerUnits.Add(player2Unit);
        _playerUnits.Add(player3Unit);
        _playersGameObjects.Add(player2GO);
        _playersGameObjects.Add(player3GO);
        
        player2Unit.EnableSpawnAnim();
        player3Unit.EnableSpawnAnim();
        
        yield return new WaitForSeconds(1.7f);
        
        var position = _playerPosOne.position;
        
        player2Unit.DisableSpawnAnim();
        _playerPosTwo.position = new Vector3(position.x + 0.6f, position.y, position.z);
        
        player3Unit.DisableSpawnAnim();
        _playerPosThree.position = new Vector3(position.x - 0.6f, position.y, position.z);
        
        state = BattleState.Enemyturn;
        StartCoroutine(EnemyTurn());
    }
    
    private IEnumerator EnemyHeal()
    {
        _dialogueText.text = "Повар ест колбасу...";
        yield return new WaitForSeconds(1);
        _enemyUnit.EnableHealAnim();
        _enemyUnit.CurrentHp += _enemyUnit.MaxHp;
        _dialogueText.text = "Повар восстановил здоровье!";
        _enemyHUD.SetHp(_enemyUnit.CurrentHp);
        yield return new WaitForSeconds(1);

        state = BattleState.Playerturn;
        _enemyUnit.DisableHealAnim();
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        _dialogueText.text = "Курлык? (Выбери действие)";
    }
}
