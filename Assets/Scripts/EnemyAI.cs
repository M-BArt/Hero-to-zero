using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.IO;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform target;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _nextWaypointDistance = 3f;

    private Vector3 _scale;
    private Vector3 _startingPosition;
    private Pathfinding.Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
    private bool _calculatepath = false;
    private Seeker _seeker;
    private Rigidbody2D _rb;
    private bool _gotHit = false;

    private void Awake()
    {
        _scale = transform.localScale;
        var player = FindObjectOfType<Player>();
        target = player.transform;
        _startingPosition = transform.position;
        _canvas = GetComponentInChildren<Canvas>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            _calculatepath = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            _calculatepath = false;
        }
    }
    void UpdatePath()
    {
        if (_seeker.IsDone() && _calculatepath)
        {
            _seeker.StartPath(_rb.position, target.position, OnPathComplete);
        }
        else
        {
            _seeker.StartPath(_rb.position, _startingPosition, OnPathComplete);
        }
        
    }

    void OnPathComplete(Pathfinding.Path p)
    {
        if(!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_path == null)
        {
            return;
        }

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath=true;
            return;
        }
        else
        {
            _reachedEndOfPath = false;
        }


        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 unitVector = new Vector2(0,0);
        if (direction.x !=0 && direction.y != 0)
        {
            unitVector = new Vector2(direction.x / (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))), direction.y / (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2))));
        }
        if (!_gotHit)
        {
            _rb.velocity = unitVector * _speed;
        }
        
        

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < _nextWaypointDistance)
        {
            _currentWaypoint++;
        }


    }
    private void LateUpdate()
    {
        if (_rb.velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
            _canvas.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (_rb.velocity.x <= -0.01f)
        {
            transform.localScale = _scale;
            _canvas.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spell")
        {
            var dir = transform.position - collision.transform.position;
            _rb.velocity = dir.normalized * 2;
            _gotHit = true;
            Invoke(nameof(StopKnockback), 0.6f);
        }
    }

    void StopKnockback()
    {
        _gotHit=false;
    }

}
