using UnityEngine;

public class Unit : MonoBehaviour
{
    private const string _attackAnim = "isAttack";
    private const string _healAnim = "isHeal";
    private const string _spawnAnim = "isSpawn";
    
    [SerializeField] private string _name;
    [SerializeField] private int _level;
    [SerializeField] private int _damage;
    [SerializeField] private int _currentHp;
    [SerializeField] private int _maxHp;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _healParticle;

    private Transform _transform;
    private Bird _bird;

    public string Name => _name;
    public int Level => _level;
    public int Damage => _damage;
    public int MaxHp => _maxHp;
    
    public int CurrentHp
    {
        get => _currentHp;
        
        set { if (value >= 0)
        {
            _currentHp = value;
        }}
    }
    public bool TakeDamage(int dmg)
    {
        _currentHp -= dmg;

        return _currentHp <= 0;
    }
    
    public void EnableAttackAnim()
    {
        _animator.SetBool(_attackAnim, true);
        if(_bird != null)
        {
            _bird.CheckSoundAttack();
        }
    }
    
    public void DisableAttackAnim()
    {
        _animator.SetBool(_attackAnim, false);
        if (_bird != null)
        {
            _bird.CheckSoundAttack();
        }
    }
    
    public void EnableHealAnim()
    {
        var spawnTransform = new Vector3(_transform.position.x, _transform.position.y + 3f, _transform.position.z);
        var healParticle = Instantiate(_healParticle, spawnTransform, Quaternion.identity);
        healParticle.transform.Rotate(90,0,0);
        _animator.SetBool(_healAnim, true);
        if (_bird != null)
        {
            _bird.CheckSoundHeal();
        }
        Destroy(healParticle, 3f);

    }
    
    public void DisableHealAnim()
    {
        _animator.SetBool(_healAnim, false);
        if (_bird != null)
        {
            _bird.CheckSoundHeal();
        }
    }
    
    public void EnableSpawnAnim()
    {
        _animator.SetBool(_spawnAnim, true);
        if (_bird != null)
        {
            _bird.CheckSoundBratia();
        }
    }
    
    public void DisableSpawnAnim()
    {
        _animator.SetBool(_spawnAnim, false);
        if (_bird != null)
        {
            _bird.CheckSoundBratia();
        }
    }

    public void EnableFatallityAnim()
    {
        _animator.SetBool("Fat", true);
        if (_bird != null)
        {
            _bird.CheckSoundFatality();
        }
    }
    
    private void Awake()
    {
        _animator.GetComponent<Animator>();
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        try
        {
            _bird = gameObject.GetComponent<Bird>();
            print("’з, не получилось XD");
        }
        catch { }
    }
}
