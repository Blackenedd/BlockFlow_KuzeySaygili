using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    [SerializeField] private int colorIndex;
    private Outline _outline;
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private void Awake()
    {
        Construct();
    }
    private void Construct()
    {
        _outline = GetComponentInChildren<Outline>(); _outline.enabled = false;
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<Renderer>();
        _renderer.material = Resources.Load<Material>("colors/" + colorIndex);
    }
    public void OnSelected()
    {
        _outline.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
    }
    public void OnRealese()
    {
        _outline.enabled = false;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
        transform.position = RoundPosition(transform.position);
    }
    private Vector3 direction;
    private float speed = 5f;
    public void Move(Vector3 position)
    {
        direction = position - transform.position;
        direction.y = 0;
        _rigidbody.velocity = direction * speed;
    }
    private static Vector3 RoundPosition(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x),
            Mathf.Round(v.y),
            Mathf.Round(v.z));
    }
}
