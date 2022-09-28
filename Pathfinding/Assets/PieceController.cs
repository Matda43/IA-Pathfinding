using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PieceController : MonoBehaviour
{
    public float speed;
    Vector3 m_EulerAngleVelocity;
    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        m_EulerAngleVelocity = new Vector3(0, 0, speed);
    }

    public void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
    }

    public Vector3 getPosition()
    {
        return rigidbody.position;
    }
}
