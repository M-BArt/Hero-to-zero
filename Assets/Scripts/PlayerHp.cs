using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    private CapsuleCollider2D _collider;
    private Animator _animator;
    private SpriteRenderer _renderer;

    public HealthBar healthBar;


    public int maxHp = 100;
    public int currentHp;


    private void Awake()
    {
        _collider = GetComponentInChildren<CapsuleCollider2D>(true);
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        currentHp = maxHp;
        healthBar.SetMaxHealth(maxHp);
    }
    void Update()
    {
        if (currentHp <= 0)
        {
            _animator.SetTrigger("Death");
            Destroy(this.gameObject, 1.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyHit")
        {
            StartCoroutine("VisualFeedback");
            TakeDamage(1);
        }
    }
    private IEnumerator VisualFeedback()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _renderer.color = Color.white;
    }

    void TakeDamage(int damage)
    {
        currentHp -= damage;

        healthBar.SetHealth(currentHp);
    }
}
