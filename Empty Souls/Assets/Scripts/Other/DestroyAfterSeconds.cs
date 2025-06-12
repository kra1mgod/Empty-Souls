using UnityEngine;

public class DestroyAfterParticle : MonoBehaviour
{
    private ParticleSystem ps;
    void Start() { ps = GetComponent<ParticleSystem>(); }
    void Update()
    {
        if (ps && !ps.IsAlive())
            Destroy(gameObject);
    }
}