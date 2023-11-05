using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.5f;

    // References
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // Movement
    private Vector2 _movement;
    private bool _isFacingRight = true;

    // Attack
    private bool _isAttacking;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_isAttacking == false)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            _movement = new Vector2(horizontalInput, verticalInput);

            // Flip character
            if (horizontalInput < 0f && _isFacingRight == true)
            {
                Flip();
            }
            else if (horizontalInput > 0f && _isFacingRight == false)
            {
                Flip();
            }
        }

        if (Input.GetButtonDown("Fire1") == true && _isAttacking == false)
        {
            _movement = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetTrigger("Attack");
            _isAttacking = true;
        }
    }

    private void FixedUpdate()
    {
        if ( _isAttacking == false)
        {
            float horizontalVelocity = _movement.normalized.x * speed;
            float verticalVelocity = _movement.normalized.y * speed;
            _rigidbody.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        }
    }

    void LateUpdate()
    {
        _animator.SetBool("Idle", _movement == Vector2.zero);

        // Animator
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _isAttacking = true;
        }
        else
        {
            _isAttacking = false;
        }
    }


    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
