using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{

    private Wall.Side _side;

    public void Construct(Vector3 globalPos,Wall.Side side)
    {
        transform.position = globalPos;
        _side = side;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            Block b = other.GetComponent<Block>();
            SpawnParticle(Resources.Load<Material>("colors/" + b.GetColor()));
        }
    }

    private void SpawnParticle(Material mat)
    {
        Transform pt = Instantiate(Resources.Load<GameObject>("particles/destory")).transform;
        pt.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = mat;
        pt.position = transform.position + Vector3.up + SideToVector() * -0.8f;
        pt.rotation = SideToQuatrenion();
    }

    public Vector3 SideToVector()
    {
        switch (_side)
        {
            case Wall.Side.down: return Vector3.back;
            case Wall.Side.up: return Vector3.forward;
            case Wall.Side.left: return Vector3.left;
            case Wall.Side.right: return Vector3.right;
        }

        return Vector3.zero;
    }
    public Quaternion SideToQuatrenion()
    {
        switch (_side)
        {
            case Wall.Side.down: return Quaternion.Euler(0, 180, 0);
            case Wall.Side.up: return Quaternion.identity;
            case Wall.Side.left: return Quaternion.Euler(0, -90, 0);
            case Wall.Side.right: return Quaternion.Euler(0, 90, 0);
        }

        return Quaternion.identity;
    }
}
