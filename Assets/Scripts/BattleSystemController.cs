using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystemController : MonoBehaviour
{
    public BattleState state;

    public GameObject _player;
    public GameObject _enemy;

    public Transform _playerPos;
    public Transform _enemyPos;

    private Unit _enemyUnit;
    private Unit _playerUnit;

    public BattleHUD _playerHUD;
    public BattleHUD _enemyHUD;

    public Text _dialogueText;

    private void Start()
    {
        Debug.Log("Start");
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

     IEnumerator SetupBattle()
     {
        Debug.Log("Units created");
        GameObject _playerGO = Instantiate(_player, _playerPos);
        _playerUnit = _playerGO.GetComponent<Unit>();

        GameObject _enemyGO = Instantiate(_enemy, _enemyPos);
        _enemyUnit = _enemyGO.GetComponent<Unit>();

        _dialogueText.text = "Страшный и ужасный... " + _enemyUnit._name + "! появился у вас на пути...";

        _playerHUD.SetHUD(_playerUnit);
        _enemyHUD.SetHUD(_enemyUnit);

        yield return new WaitForSeconds(2);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
     }

    IEnumerator PlayerAttack()
    {
        bool _isEnemyDead = _enemyUnit.TakeDamage(_playerUnit._damage);

        _enemyHUD.SetHP(_enemyUnit._currentHP);
        _dialogueText.text = "Клюнул прямо в лоб!";

        if (_isEnemyDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        yield return new WaitForSeconds(2);
    }

    private void EndBattle()
    {
        if(state == BattleState.WON)
        {
            _dialogueText.text = "Голубь отомстил повару! Победа!";
        }
        else if (state == BattleState.LOST)
        {
            _dialogueText.text = "Повар порезал голубя на шаурму!!!";
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2);
        _dialogueText.text = _enemyUnit._name + " атакует...";

        yield return new WaitForSeconds(2);

        bool _isPlayerDead = _playerUnit.TakeDamage(_enemyUnit._damage);

        _playerHUD.SetHP(_playerUnit._currentHP);

        yield return new WaitForSeconds(1);

        if (_isPlayerDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator PlayerHeal()
    {
        _dialogueText.text = "Голубь клюет семки...";
        yield return new WaitForSeconds(1);
        _playerUnit._currentHP = _playerUnit._maxHP;
        _dialogueText.text = "Голубь восстановил здоровье!";
        _playerHUD.SetHP(_playerUnit._currentHP);
        yield return new WaitForSeconds(1);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    private void PlayerTurn()
    {
        _dialogueText.text = "Курлык? (Выбери действие)";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;

        StartCoroutine (PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerHeal());
    }
}
