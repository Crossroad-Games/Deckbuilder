using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleTunnel : MonoBehaviour
{
    private Vector3 thisPosition;
    public ParticleSystem mySystem;
    private Particle[] particles;
    [SerializeField] private float speed=.1f;
    [SerializeField] private List<Vector2> ZvsTunnel;
    private void Awake()
    { 
        thisPosition = transform.position;
        //StartCoroutine("MoveParticles");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        particles = new ParticleSystem.Particle[mySystem.main.maxParticles];
        var AmountofParticlesAlive = mySystem.GetParticles(particles);
        for (int i = 0; i < AmountofParticlesAlive; i++)
        {
            float x = particles[i].position.x;
            foreach(Vector2 ZvsT in ZvsTunnel)
            {
                if (particles[i].position.z > ZvsT.x)
                    speed = ZvsT.y;
            }
            x -= x * speed;
            particles[i].position = new Vector3(x, particles[i].position.y, particles[i].position.z);
        }
        mySystem.SetParticles(particles);
    }
    IEnumerator MoveParticles()
    {
        while(true)// Never stop doing it
        {
            
            yield return new WaitForSeconds(.25f);// Apply force every .25f
        }
    }
}
