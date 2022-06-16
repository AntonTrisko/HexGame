using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance;
    [SerializeField]
    private List<ParticleSystem> _particles;

    private void OnEnable()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public void PlayParticle(Particles particle, Vector3 position)
    {
        ParticleSystem particleSystem = _particles[(int)particle];
        particleSystem.transform.position = position;
        particleSystem.Play();
    }
}

public enum Particles
{
    Build,
    Death
}