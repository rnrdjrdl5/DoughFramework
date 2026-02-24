using UnityEngine;

public static class SoundLogic
{
    public static void PlaySfx(SoundAbility soundModule, string sfxPath)
    {
        var audioClip = Realm.LoadResources<AudioClip>(sfxPath);
        
        soundModule.PlaySfx(audioClip);
    }

    public static void PlayBgm(SoundAbility soundModule, string bgmPath)
    {
        var audioClip = Realm.LoadResources<AudioClip>(bgmPath);
        
        soundModule.PlayBgm(audioClip);
    }
}