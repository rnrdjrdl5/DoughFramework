using System;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource AudioSource => audioSource;

    [SerializeField] UpdateType updateType;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float delay;
    
    ObjectPoolAbility objectPoolAbility;
    AbilitySet rootAbilitySet;

    float elapsedTime;
    float elapsedDestroyTime;
    float destroyTime;
    bool isPlaying;
    
    
    public void Initialize(AbilitySet rootAbilitySet, AudioClip clip)
    {
        this.rootAbilitySet = rootAbilitySet;
        objectPoolAbility = rootAbilitySet.GetAbility<ObjectPoolAbility>();

        audioSource.clip = clip;
    }

    void Uninitialize()
    {
        
    }

    void OnEnable()
    {
        isPlaying = false;

        elapsedTime = 0.0f;
    }

    void Update()
    {
        elapsedTime += GetUpdateTime();

        if (audioSource.loop)
        {
            return;
        }
        
        if (!isPlaying)
        {
            if (elapsedTime < delay)
            {
                return;
            }
            
            audioSource.Play();
            isPlaying = true;
        }
        else
        {
            elapsedDestroyTime += GetUpdateTime();

            if (elapsedDestroyTime >= destroyTime)
            {
                Release();

                return;
            }
            
            if (!audioSource.loop && !audioSource.isPlaying)
            {
                Release();
            }
        }
    }

    public void Release()
    {
        Uninitialize();
        
        objectPoolAbility.DeallocateGameObject(gameObject);
    }
    
    float GetUpdateTime()
    {
        return updateType switch
        {
            UpdateType.Update => Time.deltaTime,
            UpdateType.UnscaledTime => Time.unscaledTime,
        };
    }

    public enum UpdateType
    {
        Update,
        UnscaledTime
    }
}