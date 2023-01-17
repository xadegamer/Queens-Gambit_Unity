using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action<Collectable> OnPumpkinCollected;

    public enum Type { Health, Pumpkin}
    public Type type;

    [SerializeField] GameObject visual;
    [SerializeField] ParticleSystem vfx;
    [SerializeField] int amount;

    private float destroyTime;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        switch (type)
        {
            case Type.Health:
                if (Player.Instance.GetDamageable().HealthFull()) return;
                Player.Instance.GetDamageable().Heal(amount);
                break;
            case Type.Pumpkin:
                GameManager.Instance.CollectPumpkin(amount);
                OnPumpkinCollected?.Invoke(this);
                break;
        }

        Destroy();
    }

    public void Destroy()
    {
        destroyTime = (vfx.main.duration > audioSource.clip.length) ? vfx.main.duration : audioSource.clip.length;

        GetComponent<BoxCollider2D>().enabled = false;
        visual.SetActive(false);
        vfx.Play();
        audioSource.Play();
        Destroy(gameObject, destroyTime);
    }
}
