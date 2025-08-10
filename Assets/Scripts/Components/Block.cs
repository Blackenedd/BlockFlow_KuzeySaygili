using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Kuzey;

public class Block : MonoBehaviour
{
    public Side lockSide;
    private Outline _outline;
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private SpriteRenderer _lock;

    private List<BoxCollider> colliders = new List<BoxCollider>();

    private int _color;

    private bool _accetped = false;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _outline = GetComponentInChildren<Outline>(); _outline.enabled = false;
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<MeshRenderer>();
        _lock = GetComponentInChildren<SpriteRenderer>();
        _lock.enabled = false;
        colliders = GetComponents<BoxCollider>().ToList();
    }
    public void Construct(int color, Vector2 globalPosition, bool l)
    {
        if (l)
        {
            _lock.enabled = true;
            switch (lockSide)
            {
                case Side.horizontal:
                    _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    break;
                case Side.vertical:
                    _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX;
                    break;
            }
        }

        _color = color;
        _renderer.material = Resources.Load<Material>("colors/" + color);
        transform.position = Vector3.forward * globalPosition.y + Vector3.right * globalPosition.x;
    }
    public int GetColor()
    {
        return _color;
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
        transform.position = RoundPosition(transform.position);
    }
    public void OnAccepted(int direction)
    {
        if (_accetped) return;

        _accetped = true;

        Game.instance.OnAccepted(this);
        Vector3 lenght = GetXZCoverage();
        Vector3 targetPoint = Vector3.zero;

        OnRealese();

        gameObject.layer = 0;
        colliders.ForEach(x => x.isTrigger = true);

        switch (direction)
        {
            case 0: transform.DOMoveZ(transform.position.z - (lenght.z + 0.4f), 0.5f).SetEase(Ease.Linear); break;
            case 1: transform.DOMoveZ(transform.position.z + (lenght.z + 0.4f), 0.5f).SetEase(Ease.Linear); break;
            case 2: transform.DOMoveX(transform.position.x - (lenght.x + 0.4f), 0.5f).SetEase(Ease.Linear); break;
            case 3: transform.DOMoveX(transform.position.x + (lenght.x + 0.4f), 0.5f).SetEase(Ease.Linear); break;
        }
    }
    private Vector3 direction;
    private float speed = 5f;

    public void Move(Vector3 position)
    {
        direction = position - transform.position;
        direction.y = 0;
        _rigidbody.velocity = direction * speed;
    }
    public List<Vector3> GetParticlePoints()
    {
        List<Vector3> particlePoints = new List<Vector3>();

        colliders.ForEach(x =>
        {
            particlePoints.Add(x.bounds.center);
        });

        return particlePoints;
    }
    public Vector3 GetXZCoverage()
    {
        if (colliders == null || colliders.Count == 0)
            return Vector2.zero;

        Vector3 min = colliders[0].bounds.min;
        Vector3 max = colliders[0].bounds.max;

        foreach (var col in colliders)
        {
            Bounds b = col.bounds;
            min = Vector3.Min(min, b.min);
            max = Vector3.Max(max, b.max);
        }

        float totalX = max.x - min.x;
        float totalZ = max.z - min.z;

        return new Vector3(totalX, 0,totalZ);
    }

    public Vector3 GetCenter()
    {
        if (colliders == null || colliders.Count == 0)
            return Vector3.zero;

        Vector3 min = colliders[0].bounds.min;
        Vector3 max = colliders[0].bounds.max;

        foreach (var col in colliders)
        {
            Bounds b = col.bounds;
            min = Vector3.Min(min, b.min);
            max = Vector3.Max(max, b.max);
        }

        Vector3 center = (min + max) / 2f;
        center.y = 0f;

        return center;
    }
    private static Vector3 RoundPosition(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x),
            Mathf.Round(v.y),
            Mathf.Round(v.z));
    }
    public enum Side
    {
        horizontal,
        vertical
    }
}
