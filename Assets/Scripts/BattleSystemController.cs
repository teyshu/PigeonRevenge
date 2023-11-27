using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemController : MonoBehaviour
{
    public BattleState state;

    public GameObject _player;
    public GameObject _enemy;

    public Transform _playerPos;
    public Transform _enemyPos;

    public BattleHUD _playerHUD;
    public BattleHUD _enemyHUD;

    public Text _dialogueText;

    
    private Unit _enemyUnit;
    private Unit _playerUnit;

    private void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
     {
         GameObject _playerGO = Instantiate(_player, _playerPos);
        _playerUnit = _playerGO.GetComponent<Unit>();

        GameObject _enemyGO = Instantiate(_enemy, _enemyPos);
        _enemyUnit = _enemyGO.GetComponent<Unit>();

        _dialogueText.text = "Страшный и ужасный... " + _enemyUnit.Name + "! появился у вас на пути...";

        _playerHUD.SetHUD(_playerUnit);
        _enemyHUD.SetHUD(_enemyUnit);

        yield return new WaitForSeconds(2);

        state = BattleState.Playerturn;
        PlayerTurn();
     }

    private IEnumerator PlayerAttack()
    {
        _playerUnit.EnableAttackAnim();
        bool isEnemyDead = _enemyUnit.TakeDamage(_playerUnit.Damage);

        _enemyHUD.SetHp(_enemyUnit.CurrentHp);
        _dialogueText.text = "Клюнул прямо в лоб!";

        yield return new WaitForSeconds(1);
        
        _playerUnit.DisableAttackAnim();
        
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

    private void EndBattle()
    {
        if(state == BattleState.Won)
        {
            _dialogueText.text = "Голубь отомстил повару! Победа!";
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

        bool _isPlayerDead = _playerUnit.TakeDamage(_enemyUnit.Damage);

        _playerHUD.SetHp(_playerUnit.CurrentHp);
        
        _enemyUnit.DisableAttackAnim();

        yield return new WaitForSeconds(1);

        if (_isPlayerDead)
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

    private IEnumerator PlayerHeal()
    {
        _dialogueText.text = "Голубь клюет семки...";
        yield return new WaitForSeconds(1);
        _playerUnit.EnableHealAnim();
        _playerUnit.CurrentHp = _playerUnit.MaxHp;
        _dialogueText.text = "Голубь восстановил здоровье!";
        _playerHUD.SetHp(_playerUnit.CurrentHp);
        yield return new WaitForSeconds(1);

        state = BattleState.Enemyturn;
        _playerUnit.DisableHealAnim();
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

    public void OnAttackButton()
    {
        if (state != BattleState.Playerturn) return;

        StartCoroutine (PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.Playerturn) return;

        StartCoroutine(PlayerHeal());
    }
}
