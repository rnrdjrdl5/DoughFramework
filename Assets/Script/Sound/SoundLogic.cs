using UnityEngine;

public static class SoundLogic
{
    public static void PlaySfx(SoundModule soundModule, string sfxPath)
    {
        var audioClip = Universe.LoadResources<AudioClip>(sfxPath);
        
        soundModule.PlaySfx(audioClip);
    }

    public static void PlayBgm(SoundModule soundModule, string bgmPath)
    {
        var audioClip = Universe.LoadResources<AudioClip>(bgmPath);
        
        soundModule.PlayBgm(audioClip);
    }
}