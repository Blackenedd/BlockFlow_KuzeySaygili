using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    private Outline outline;
    private Rigidbody rb;
    private Collider collider;

    private void Awake()
    {
        Construct();
    }
    private void Construct()
    {
        outline = GetComponentInChildren<Outline>(); outline.enabled = false;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    public void OnSelected()
    {
        outline.enabled = true;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
    }
    public void OnRealese()
    {
        outline.enabled = false;
        rb.isKinematic = true; 
        rb.velocity = Vector3.zero;
        transform.position = RoundPosition(transform.position);
    }
    private Vector3 direction;
    [SerializeField] private float speed = 5f;
    public void Move(Vector3 position)
    {
        direction = position - transform.position;
        direction.y = 0;
        rb.velocity = direction * Time.deltaTime * speed;
    }
    private static Vector3 RoundPosition(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x),
            Mathf.Round(v.y),
            Mathf.Round(v.z));
    }
}
