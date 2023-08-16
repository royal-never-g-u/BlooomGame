using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 声音管理器
/// </summary>

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;    //单例模式

    public AudioMixer masterMixer;          //Mixer
    public AudioSource bgmSource;           //播放bgm的音频
    public AudioSource soundSource;         //播放音效的音频

    private string bgmMixerParam;           //bgmMixer音量大小的参数
    private string soundMixerParam;         //soundMixer音量大小的参数

    //private float soundVolume = 1.0f;       //音效音量大小

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        bgmMixerParam = "BgmVolume";
        soundMixerParam = "SoundVolume";

    }
    
    //1.播放、暂停相关

    /// <summary>
    /// 播放Bgm
    /// </summary>
    public void PlayBGM(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Bgm/" + name);
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    /// <summary>
    /// 暂停bgm
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// 播放音效1：需要同时播放多个音效的情况(暂时弃用
    /// </summary>
    //public void PlayEffect(string name, Transform pos)
    //{
    //    AudioClip clip = Resources.Load<AudioClip>("Audio/Sound/" + name);
    //    AudioSource.PlayClipAtPoint(clip, pos.position);
    //}

    /// <summary>
    /// 播放音效2：跟放Bgm一个思路
    /// </summary>
    /// <param name="name"></param>
    public void PlayEffect(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Sound/" + name);
        soundSource.clip = clip;
        soundSource.Play();
    }

    //2.音量调节相关

    /// <summary>
    /// 调节全局音量
    /// </summary>
    public void AdjustMusicVolume(float value)
    {
        AudioListener.volume = value;
    }

    /// <summary>
    /// 调节bgm音量
    /// </summary>
    public void AdjustBgmVolume(float value)
    {
        masterMixer.SetFloat(bgmMixerParam, ReflectToDB(value));
    }

    /// <summary>
    /// 调节音效音量1(暂时弃用
    /// </summary>
    //public void AdjustSoundVolume(float value)
    //{
    //    soundVolume = value;
    //}

    /// <summary>
    /// 调节音效音量2
    /// </summary>
    public void AdjustSoundVolume(float value)
    {
        masterMixer.SetFloat(soundMixerParam, ReflectToDB(value));
    }

    /// <summary>
    /// 将slider得到的值映射为分贝
    /// </summary>
    /// <returns></returns>
    private float ReflectToDB(float value)
    {
        if (value <= 0.0f) value = 0.0001f;
        return Mathf.Log10(value) * 20.0f;
    }

}
