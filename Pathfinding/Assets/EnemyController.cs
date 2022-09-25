using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    Vector3 velocity;
    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
