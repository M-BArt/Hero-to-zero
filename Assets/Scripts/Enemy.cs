using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _healthPoints = 100;

    private void Awake(){}
    void Start(){}
    void Update()
    {
        if (_healthPoints <= 0)
        {
            _animator.SetTrigger("Death"); 
            Destroy(this.gameObject, 0.6f);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if(_collider.gameObject.tag == "Spell")
        {
            _healthPoints--;

            if (_healthPoints > 0) _animator.SetTrigger("TakeHit");
        }
    }
}
