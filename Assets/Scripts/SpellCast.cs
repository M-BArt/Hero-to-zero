using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FireballController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fireballSpeed = 10f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private float _cooldawnSpell = 2f;
    [SerializeField] private float _forcePunch = 20;
    Rigidbody2D rb2d;
    float delayTime = 0.5f;

    Vector2 directionOne;
    void Start()
    {
        rb2d = gameObject.AddComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.freezeRotation = true;

        
        StartCoroutine(StartMovementAfterDelay());
    }
    IEnumerator StartMovementAfterDelay()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;

        Vector2 directionOne = new Vector2(direction.x/(Mathf.Sqrt(Mathf.Pow(direction.x,2) + Mathf.Pow(direction.y, 2))), direction.y/ (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))));

        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        yield return new WaitForSeconds(delayTime);

        rb2d.velocity = directionOne * _fireballSpeed;

        Destroy(gameObject, _lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
           
            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(rb2d.velocity/_fireballSpeed * _forcePunch, ForceMode2D.Impulse);
                StartCoroutine(StopEnemy(enemyRb, 0.1f));
            }
            
            rb2d.velocity = Vector2.zero;
            _animator.SetTrigger("Destroy");
            Destroy(gameObject, 1f);
        }
    }
    private IEnumerator StopEnemy(Rigidbody2D enemyRb, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyRb.velocity = Vector2.zero;
    }

    void Update(){  }
}

