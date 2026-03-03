using UnityEngine;

public static class SoundLogic
{
    public static void PlaySfx(SoundAbility soundAbility, string sfxPath)
    {
        var audioClip = Realm.LoadResources<AudioClip>(sfxPath);
        
        soundAbility.PlaySfx(audioClip);
    }

    public static void PlayBgm(SoundAbility soundAbility, string bgmPath)
    {
        var audioClip = Realm.LoadResources<AudioClip>(bgmPath);
        
        soundAbility.PlayBgm(audioClip);
    }
}