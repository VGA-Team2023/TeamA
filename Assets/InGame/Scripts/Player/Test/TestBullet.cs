//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [TagName]
    [SerializeField] private string _groundTag;

    private Vector2 _playerForward;

    public void SetShotDirection(Vector2 direction) 
    {
        _playerForward = direction;
        Move();
    }

    private void Move()
    {
        _rb.AddForce(_playerForward * _speed, ForceMode2D.Force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_groundTag)) 
        {
            Destroy(gameObject);
        }

        Destroy(gameObject, 2f);
    }
}
