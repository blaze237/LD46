using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSoundController : MonoBehaviour
{
    public GameObject[] particles, sounds;
    private int[] curParticleCount = new int[4];
    private int[] newParticleCount = new int[4];
    public AK.Wwise.Event MyEvent;

    void Start()
    {
        Tower.instance.damageEventHandler += UpdateParticleSettings;
    }

    void Update()
    {
        for(int i = 0; i < 3; i++)
        {
            newParticleCount[i] = particles[i].GetComponent<ParticleSystem>().particleCount;
            if(curParticleCount[i] < newParticleCount[i])
            {
                MyEvent.Post(sounds[i]);
                Debug.Log("Play Sound");
                curParticleCount[i] = newParticleCount[i];
            }
        }
    }

    void UpdateParticleSettings()
    {
        ParticleSystem.Burst burst1 = new ParticleSystem.Burst(0, 0.15f * (100 - Tower.instance.m_health));
        burst1.probability = 0.01f * (100 - Tower.instance.m_health);
        ParticleSystem.Burst burst2 = new ParticleSystem.Burst(2, 0.3f * (100 - Tower.instance.m_health));
        burst2.probability = 0.006f * (100 - Tower.instance.m_health);
        ParticleSystem.Burst burst3 = new ParticleSystem.Burst(4, 0.15f * (100 - Tower.instance.m_health));
        burst3.probability = 0.01f * (100 - Tower.instance.m_health);
        ParticleSystem.Burst burst4 = new ParticleSystem.Burst(6, 0.3f * (100 - Tower.instance.m_health));
        burst4.probability = 0.006f * (100 - Tower.instance.m_health);
        ParticleSystem.Burst burst5 = new ParticleSystem.Burst(8, 0.15f * (100 - Tower.instance.m_health));
        burst5.probability = 0.01f*(100-Tower.instance.m_health);

        ParticleSystem.Burst[] bursts = { burst1, burst2, burst3, burst4, burst5 };

        for (int i = 0; i < 3; i++)
        {
            GetComponent<ParticleSystem>().emission.SetBursts(bursts);
        }
    }
}
