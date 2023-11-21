using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FireballController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fireballSpeed = 10f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private float _cooldawnSpell = 2f;
    [SerializeField] private float _forcePunch = 20;
    
    
    private Rigidbody2D _SpellRigidbody;
    private Collider2D _collider;
    private float delayTime = 0.5f;
    private bool _readyToCast;

    void Start()
    {
        
        _SpellRigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();   
        _readyToCast = false;
        StartCoroutine(StartMovementAfterDelay());
    }
    IEnumerator StartMovementAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 unitVector = new Vector2(direction.x/(Mathf.Sqrt(Mathf.Pow(direction.x,2) + Mathf.Pow(direction.y, 2))), direction.y/ (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))));

        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);    
        
        _readyToCast = true;
        _SpellRigidbody.velocity = unitVector * _fireballSpeed;
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "wall")
        {

            Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(_SpellRigidbody.velocity / _fireballSpeed * _forcePunch, ForceMode2D.Impulse);
                StartCoroutine(StopEnemy(enemyRb, 0.1f));
            }

            _SpellRigidbody.velocity = Vector2.zero;
            _collider.enabled = false;
            _animator.SetTrigger("Destroy");
            FindObjectOfType<AudioManager>().Play("SpellDestory");
            Destroy(gameObject, 1f);
        }
    }

    private IEnumerator StopEnemy(Rigidbody2D enemyRb, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyRb.velocity = Vector2.zero;
    }

    void Update(){

        if (_readyToCast == false)
        {
            _SpellRigidbody.transform.position = GameObject.Find("Player").transform.position; 
        }
    
    }
}

