using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    private CapsuleCollider2D _collider;
    private Animator _animator;
    private SpriteRenderer _renderer;

    public int hp = 100;
    private void Awake()
    {
        _collider = GetComponentInChildren<CapsuleCollider2D>(true);
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
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
            Destroy(this.gameObject, 0.6f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyHit")
        {
            StartCoroutine("VisualFeedback");
            hp--;
        }
    }

    private IEnumerator VisualFeedback()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _renderer.color = Color.white;
    }
}
