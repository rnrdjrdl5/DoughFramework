using UnityEngine;

public class SoundAbility : Ability
{
    public static string BgmSource = nameof(BgmSource);
    public static string SfxSource = nameof(SfxSource);
    
    ObjectPoolAbility objectPoolAbility;
    SoundPlayer bgmSound;
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        objectPoolAbility = Entity.RootAbilitySet.GetAbility<ObjectPoolAbility>();
    }
    
    public void PlaySfx(AudioClip clip)
    {
        var sfxPrefab = GetSfxPrefab();
        var sfxObject = objectPoolAbility.AllocateGameObject(sfxPrefab);
        
        var sfxSound = sfxObject.GetComponent<SoundPlayer>();
        sfxSound.AudioSource.loop = false;
        
        sfxSound.Initialize(Entity.RootAbilitySet, clip);
    }
    
    public void PlayBgm(AudioClip clip)
    {
        var bgmPrefab = GetBgmPrefab();
        var bgmObject = objectPoolAbility.AllocateGameObject(bgmPrefab);
        
        bgmSound = bgmObject.GetComponent<SoundPlayer>();
        bgmSound.AudioSource.loop = true;
        
        bgmSound.Initialize(Entity.RootAbilitySet, clip);
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
