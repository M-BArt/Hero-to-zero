using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _character;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ManaBar manaBar;
    [SerializeField] private BoxCollider2D _boxCollider;

    [Header("Character variables")]
    [SerializeField] private int _healthPoints = 100;
    [SerializeField] private int _currentHp = 100;
    [SerializeField] private int _maxMana = 100;
    [SerializeField] private int _currentMana;

    [Header("Movement variables")]
    [SerializeField] private Vector2 _movementCharacter;
    [SerializeField] private float _speed;

    [Header("Spell variables")]
    [SerializeField] private GameObject[] _spellPrefab;
    [SerializeField] private float _cooldownSpellFire;
    [SerializeField] private float _cooldownSpellIce;
    [SerializeField] private float _cooldownSpellThunder;
    [SerializeField] private float _cooldownSpellHeal;
    [SerializeField] private int _activeSkill;
    [SerializeField] private float _lastFireTimeFire;
    [SerializeField] private float _lastFireTimeIce;
    [SerializeField] private float _lastFireTimeThunder;
    [SerializeField] private float _lastFireTimeHeal;
    [SerializeField] private ActiveSkill[] _skills;
    [SerializeField] private CooldownCounter[] _cooldowns;

    [Header("Knockback")]
    [SerializeField] private bool _knockbacked;
    [SerializeField] private float _knockbackTime = 0.5f;

    [SerializeField] private Vector3 _mousePosition;
    private bool _isDead = false;

    private void Awake() { }
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _activeSkill = 1;
        _skills[0].SetActiveSkill();
        InvokeRepeating("RestoreMana", 0, 1);
        _currentMana = _maxMana;
        manaBar.SetMaxMana(_maxMana);

        _currentHp = _healthPoints;
        healthBar.SetMaxHealth(_healthPoints);
    }
    void Update()
    {
        if (!PauseMenu._gameIsPause && !GameOverMenu._gameEnds && !_isDead)
        {
            if (!_knockbacked)
            {
                _movementCharacter = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                _rigidbody.velocity = _movementCharacter.normalized * _speed;

                _animator.SetFloat("MovementX", _movementCharacter.x);
                _animator.SetFloat("MovementY", _movementCharacter.y);
                _animator.SetFloat("Speed", _movementCharacter.sqrMagnitude);

                if (_movementCharacter != Vector2.zero)
                {
                    _animator.SetFloat("PositionX", _movementCharacter.x);
                    _animator.SetFloat("PositionY", _movementCharacter.y);
                }
            }
            if (Input.GetMouseButton(0))
            {
                _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _character.transform.position;
                Vector2 _mousePositionV2 = new Vector2(_mousePosition.x, _mousePosition.y);

                _rigidbody.velocity = _movementCharacter.normalized * _speed;

                _animator.SetFloat("mousePositionX", _mousePositionV2.normalized.x);
                _animator.SetFloat("mousePositionY", _mousePositionV2.normalized.y);
                _animator.SetFloat("PositionX", _mousePositionV2.normalized.x);
                _animator.SetFloat("PositionY", _mousePositionV2.normalized.y);
                _animator.SetTrigger("Attack");


                SpellCast(10);
            }
            if (!Input.GetMouseButton(0)) _animator.ResetTrigger("Attack");

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _activeSkill = 1;
                _skills[0].SetActiveSkill();
                _skills[1].ResetActiveSkill();
                _skills[2].ResetActiveSkill();
                _skills[3].ResetActiveSkill();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _activeSkill = 2;
                _skills[0].ResetActiveSkill();
                _skills[1].SetActiveSkill();
                _skills[2].ResetActiveSkill();
                _skills[3].ResetActiveSkill();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _activeSkill = 3;
                _skills[0].ResetActiveSkill();
                _skills[1].ResetActiveSkill();
                _skills[2].SetActiveSkill();
                _skills[3].ResetActiveSkill();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _activeSkill = 4;
                _skills[0].ResetActiveSkill();
                _skills[1].ResetActiveSkill();
                _skills[2].ResetActiveSkill();
                _skills[3].SetActiveSkill();
            }
            if (_currentHp <= 0 && !_isDead)
            {
                FindObjectOfType<AudioManager>().Play("Death");
                _isDead = true;
                _rigidbody.velocity = Vector3.zero;
                _boxCollider.enabled = false;
                _animator.SetTrigger("Death");
                Destroy(this.gameObject, 1.25f);
                Invoke(nameof(EndGame),1f);
            }
        }
        
    }

    void EndGame()
    {
        FindObjectOfType<AudioManager>().Stop("Theme");
        FindObjectOfType<AudioManager>().Play("GameOver");
        GameOverMenu._gameEnds = true;
    }

    public void SpellCast(int manaCost)
    {
        switch (_activeSkill)
        {
            case 1:
                if (Time.time - _lastFireTimeFire >= _cooldownSpellFire && _currentMana > 0)
                {
                    _currentMana -= manaCost;

                    manaBar.SetMana(_currentMana);
                    Vector3 spellStartPosition = transform.position;

                    GameObject spellObject = Instantiate(_spellPrefab[0], spellStartPosition, Quaternion.identity);
                    FindObjectOfType<AudioManager>().Play("FireBallAwake");
                    _lastFireTimeFire = Time.time;
                    _cooldowns[0].SetCooldown(_cooldownSpellFire);
                    _cooldowns[0].StartCounting();
                }
                break;
            case 2:
                if (Time.time - _lastFireTimeIce >= _cooldownSpellIce && _currentMana > 0)
                {
                    _currentMana -= manaCost;

                    manaBar.SetMana(_currentMana);
                    Vector3 spellStartPosition = transform.position;

                    GameObject spellObject = Instantiate(_spellPrefab[1], spellStartPosition, Quaternion.identity);
                    FindObjectOfType<AudioManager>().Play("IceBallAwake");
                    _lastFireTimeIce = Time.time;
                    _cooldowns[1].SetCooldown(_cooldownSpellIce);
                    _cooldowns[1].StartCounting();
                }
                break;
            case 3:
                if (Time.time - _lastFireTimeThunder >= _cooldownSpellThunder && _currentMana > 0)
                {
                    _currentMana -= manaCost;

                    manaBar.SetMana(_currentMana);
                    Vector3 spellStartPosition = transform.position;

                    GameObject spellObject = Instantiate(_spellPrefab[2], spellStartPosition, Quaternion.identity);
                    FindObjectOfType<AudioManager>().Play("ThunderBallAwake");
                    _lastFireTimeThunder = Time.time;
                    _cooldowns[2].SetCooldown(_cooldownSpellThunder);
                    _cooldowns[2].StartCounting();
                }
                break;
            case 4:
                if (Time.time - _lastFireTimeHeal >= _cooldownSpellHeal && _currentMana > 0)
                {
                    _currentMana -= manaCost;

                    manaBar.SetMana(_currentMana);
                    Vector3 spellStartPosition = transform.position;

                    GameObject spellObject = Instantiate(_spellPrefab[0], spellStartPosition, Quaternion.identity);

                    _lastFireTimeHeal = Time.time;
                    _cooldowns[3].SetCooldown(_cooldownSpellHeal);
                    _cooldowns[3].StartCounting();
                }
                break;

        }
       
    }
    public void Knockback(Transform t, float _knockbackVelocity, int _knockbackDamage) { 
        var dir = transform.position - t.transform.position;
        _knockbacked = true;
        _rigidbody.velocity = dir.normalized * _knockbackVelocity;
        StartCoroutine("VisualFeedback");
        TakeDamage(_knockbackDamage);
        StartCoroutine(UnKnockback());
    }
    private IEnumerator UnKnockback()
    {
        yield return new WaitForSeconds(_knockbackTime);
        _knockbacked = false;
    }
    private IEnumerator VisualFeedback()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _renderer.color = Color.white;
    }

    void RestoreMana()
    {
        if (_currentMana < _maxMana)
        {
            _currentMana = _currentMana + 15;
            manaBar.SetMana(_currentMana);
        }
    }

    void TakeDamage(int damage)
    {
        _currentHp -= damage;

        healthBar.SetHealth(_currentHp);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("PlayerHit");
        }
    }
}
