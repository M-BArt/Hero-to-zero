using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _healthPoints = 100;
    [SerializeField] private HealthBar _healthBar;

    private void Awake()
    {
        _healthBar.SetMaxHealth(_healthPoints);
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (_healthPoints <= 0)
        {
            _animator.SetTrigger("Death"); 
            Destroy(this.gameObject, 0.6f);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spell")
        {
            _healthPoints--;
            _healthBar.SetHealth(_healthPoints);
            FindObjectOfType<AudioManager>().Play("SlimeHit");
            if (_healthPoints > 0) _animator.SetTrigger("TakeHit");
        }
    }
}
