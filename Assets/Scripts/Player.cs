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
    [SerializeField] private float _cooldownSpell;
    [SerializeField] private float _lastFireTime;
    [SerializeField] private ActiveSkill[] skills;

    [Header("Knockback")]
    [SerializeField] private bool _knockbacked;
    [SerializeField] private float _knockbackTime = 0.5f;

    [SerializeField] private Vector3 _mousePosition;


    private void Awake() { }
    void Start()
    {
        skills[0].SetActiveSkill();
        InvokeRepeating("RestoreMana", 0, 1);
        _currentMana = _maxMana;
        manaBar.SetMaxMana(_maxMana);

        _currentHp = _healthPoints;
        healthBar.SetMaxHealth(_healthPoints);
    }
    void Update()
    {

        if (!_knockbacked) { 
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
            skills[0].SetActiveSkill();
            skills[1].ResetActiveSkill();
            skills[2].ResetActiveSkill();
            skills[3].ResetActiveSkill(); 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skills[0].ResetActiveSkill();
            skills[1].SetActiveSkill();
            skills[2].ResetActiveSkill();
            skills[3].ResetActiveSkill();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skills[0].ResetActiveSkill();
            skills[1].ResetActiveSkill();
            skills[2].SetActiveSkill();
            skills[3].ResetActiveSkill();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            skills[0].ResetActiveSkill();
            skills[1].ResetActiveSkill();
            skills[2].ResetActiveSkill();
            skills[3].SetActiveSkill();
        }
        if (_currentHp <= 0)
        {
            _animator.SetTrigger("Death");
            Destroy(this.gameObject, 1.25f);
        }
    }

    public void SpellCast(int manaCost)
    {
        if (Time.time - _lastFireTime >= _cooldownSpell && _currentMana > 0)
        {
            _currentMana -= manaCost;

            manaBar.SetMana(_currentMana);
            Vector3 spellStartPosition = transform.position;

            GameObject spellObject = Instantiate(_spellPrefab[0], spellStartPosition, Quaternion.identity);

            _lastFireTime = Time.time;
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
            _currentMana = _currentMana + 5;
            manaBar.SetMana(_currentMana);
        }
    }

    void TakeDamage(int damage)
    {
        _currentHp -= damage;

        healthBar.SetHealth(_currentHp);
    }
}
