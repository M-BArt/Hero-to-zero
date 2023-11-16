using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject[] _spellPrefab;
    [SerializeField] private GameObject _character;
    [SerializeField] private float _speed;
    [SerializeField] private float _cooldownSpell;

    private Vector2 _movementCharacter;
    [SerializeField] private Vector3 _mousePosition;
    private float _lastFireTime;

    private void Awake(){}
    void Start(){}
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
    
      
            SpellCast();
        }
        if (!Input.GetMouseButton(0)) _animator.ResetTrigger("Attack");
    }
    public void SpellCast()
    {
        if (Time.time - _lastFireTime >= _cooldownSpell)
        {
            Vector3 spellStartPosition = transform.position;

            GameObject spellObject = Instantiate(_spellPrefab[0], spellStartPosition, Quaternion.identity);
                     
            _lastFireTime = Time.time;
        }
    }
}
