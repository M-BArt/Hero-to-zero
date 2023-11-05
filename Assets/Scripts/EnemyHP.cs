using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    private CapsuleCollider2D _collider;
    private Animator _animator;
    private bool _isDead = false;
    private bool _endAnimation = false;

    public int hp = 3;
    private void Awake()
    {
        _collider = GetComponentInChildren<CapsuleCollider2D>(true);
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            _animator.SetTrigger("Death"); 
            Destroy(this.gameObject,0.6f);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "HitBox")
        {
            _animator.SetTrigger("TakeHit");
            hp--;
        }
    }
}
