using UnityEngine;

public class SoundTrait : Trait
{
    public static string BgmSource = nameof(BgmSource);
    public static string SfxSource = nameof(SfxSource);
    
    ObjectPoolTrait objectPoolTrait;
    SoundPlayer bgmSound;
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        objectPoolTrait = Actor.RootTraitSet.GetTrait<ObjectPoolTrait>();
    }
    
    public void PlaySfx(AudioClip clip)
    {
        var sfxPrefab = GetSfxPrefab();
        var sfxObject = objectPoolTrait.AllocateGameObject(sfxPrefab);
        
        var sfxSound = sfxObject.GetComponent<SoundPlayer>();
        sfxSound.AudioSource.loop = false;
        
        sfxSound.Initialize(Actor.RootTraitSet, clip);
    }
    
    public void PlayBgm(AudioClip clip)
    {
        var bgmPrefab = GetBgmPrefab();
        var bgmObject = objectPoolTrait.AllocateGameObject(bgmPrefab);
        
        bgmSound = bgmObject.GetComponent<SoundPlayer>();
        bgmSound.AudioSource.loop = true;
        
        bgmSound.Initialize(Actor.RootTraitSet, clip);
    }

    public void ResumeBgm()
    {
        bgmSound.AudioSource.UnPause();
    }

    public void PauseBgm()
    {
        bgmSound.AudioSource.Pause();
    }

    public void StopBgm()
    {
        bgmSound.Release();
        bgmSound = null;
    }

    GameObject GetBgmPrefab()
    {
        return Realm.LoadResources<GameObject>(BgmSource);
    }
    
    GameObject GetSfxPrefab()
    {
        return Realm.LoadResources<GameObject>(SfxSource);
    }
}
