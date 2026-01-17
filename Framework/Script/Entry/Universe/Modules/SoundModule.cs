using UnityEngine;

public class SoundModule : Module
{
    public static string BgmSource = nameof(BgmSource);
    public static string SfxSource = nameof(SfxSource);
    
    ObjectPoolModule objectPoolModule;
    SoundPlayer bgmSound;
    
    protected override void Initialize(GameObject universeObject, Environment environment)
    {
        base.Initialize(universeObject, environment);
        
        objectPoolModule = Environment.GetModule<ObjectPoolModule>();
    }
    
    public void PlaySfx(AudioClip clip)
    {
        var sfxPrefab = GetSfxPrefab();
        var sfxObject = objectPoolModule.AllocateGameObject(sfxPrefab);
        
        var sfxSound = sfxObject.GetComponent<SoundPlayer>();
        sfxSound.AudioSource.loop = false;
        
        sfxSound.Initialize(Environment, clip);
    }
    
    public void PlayBgm(AudioClip clip)
    {
        var bgmPrefab = GetBgmPrefab();
        var bgmObject = objectPoolModule.AllocateGameObject(bgmPrefab);
        
        bgmSound = bgmObject.GetComponent<SoundPlayer>();
        bgmSound.AudioSource.loop = true;
        
        bgmSound.Initialize(Environment, clip);
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
        return Universe.LoadResources<GameObject>(BgmSource);
    }
    
    GameObject GetSfxPrefab()
    {
        return Universe.LoadResources<GameObject>(SfxSource);
    }
}
