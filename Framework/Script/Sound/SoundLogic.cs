using UnityEngine;

public static class SoundLogic
{
    public static void PlaySfx(SoundTrait soundModule, string sfxPath)
    {
        var audioClip = Realm.LoadResources<AudioClip>(sfxPath);
        
        soundModule.PlaySfx(audioClip);
    }

    public static void PlayBgm(SoundTrait soundModule, string bgmPath)
    {
        var audioClip = Realm.LoadResources<AudioClip>(bgmPath);
        
        soundModule.PlayBgm(audioClip);
    }
}