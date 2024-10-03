using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingHackable : Hackable
{
    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    Transform explosionPoint;

    public bool explodeSelf;
    public float selfExplosionForce = 500f;
    public float selfTorqueForce = 100f;
    public override void Hack(Entity player)
    {
        Instantiate(explosionPrefab, explosionPoint.position,Quaternion.identity,null);
        if (explodeSelf)
        {
            if (GetComponent<Rigidbody>())
            {
                Vector3 domeDirection = GetRandomDirectionInDome();
                GetComponent<Rigidbody>().AddForce(domeDirection * selfExplosionForce, ForceMode.Impulse);
                Vector3 randomTorque = GetRandomTorque();
                GetComponent<Rigidbody>().AddTorque(randomTorque * selfTorqueForce, ForceMode.Impulse);

                AudioManager.instance.PlayCachedSound(AudioManager.instance.ExplosionSounds, transform.position, 0.3f);
            }
        }
    }
    private Vector3 GetRandomDirectionInDome()
    {
        // Generate a random point on the unit sphere
        Vector3 randomDirection = Random.onUnitSphere;

        // Force the y-component to be positive (above the XZ plane) to simulate a dome
        if (randomDirection.y < 0)
        {
            randomDirection.y *= -1;
        }

        return randomDirection.normalized;
    }
    private Vector3 GetRandomTorque()
    {
        // Generate random torque by creating a random vector
        return new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }
}
