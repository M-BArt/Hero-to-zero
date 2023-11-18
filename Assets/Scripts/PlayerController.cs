using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject[] _spellPrefab;
    [SerializeField] private GameObject _character;
    [SerializeField] private float _speed;
    [SerializeField] private float _cooldownSpell;

    public ActiveSkill[] skills;
    public ManaBar manaBar;
    public int maxMana = 100;
    public int currentMana;

    private Vector2 _movementCharacter;
    [SerializeField] private Vector3 _mousePosition;
    private float _lastFireTime;

    private void Awake() { }
    void Start()
    {
        skills[0].SetActiveSkill();
        InvokeRepeating("RestoreMana", 0, 1);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }
    void Update()
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
    }
    public void SpellCast(int manaCost)
    {
        if (Time.time - _lastFireTime >= _cooldownSpell && currentMana > 0)
        {
            currentMana -= manaCost;

            manaBar.SetMana(currentMana);
            Vector3 spellStartPosition = transform.position;

            GameObject spellObject = Instantiate(_spellPrefab[0], spellStartPosition, Quaternion.identity);

            _lastFireTime = Time.time;
        }
    }

    void RestoreMana()
    {
        if (currentMana < maxMana)
        {
            currentMana = currentMana + 5;
            manaBar.SetMana(currentMana);
        }
    }
}
