using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackTrigger : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField] private float _knockbackVelocity;
    [SerializeField] private int _knockbackDamage;
    private void OnCollisionEnter2D(Collision2D collision) { 

        var Player = collision.gameObject.GetComponent<Player>();
        if (Player != null)
        {
            Player.Knockback(transform, _knockbackVelocity, _knockbackDamage);
        }
    }
}
