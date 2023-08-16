using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// ����������
/// </summary>

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;    //����ģʽ

    public AudioMixer masterMixer;          //Mixer
    public AudioSource bgmSource;           //����bgm����Ƶ
    public AudioSource soundSource;         //������Ч����Ƶ

    private string bgmMixerParam;           //bgmMixer������С�Ĳ���
    private string soundMixerParam;         //soundMixer������С�Ĳ���

    //private float soundVolume = 1.0f;       //��Ч������С

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        bgmMixerParam = "BgmVolume";
        soundMixerParam = "SoundVolume";

    }
    
    //1.���š���ͣ���

    /// <summary>
    /// ����Bgm
    /// </summary>
    public void PlayBGM(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Bgm/" + name);
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    /// <summary>
    /// ��ͣbgm
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// ������Ч1����Ҫͬʱ���Ŷ����Ч�����(��ʱ����
    /// </summary>
    //public void PlayEffect(string name, Transform pos)
    //{
    //    AudioClip clip = Resources.Load<AudioClip>("Audio/Sound/" + name);
    //    AudioSource.PlayClipAtPoint(clip, pos.position);
    //}

    /// <summary>
    /// ������Ч2������Bgmһ��˼·
    /// </summary>
    /// <param name="name"></param>
    public void PlayEffect(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Sound/" + name);
        soundSource.clip = clip;
        soundSource.Play();
    }

    //2.�����������

    /// <summary>
    /// ����ȫ������
    /// </summary>
    public void AdjustMusicVolume(float value)
    {
        AudioListener.volume = value;
    }

    /// <summary>
    /// ����bgm����
    /// </summary>
    public void AdjustBgmVolume(float value)
    {
        masterMixer.SetFloat(bgmMixerParam, ReflectToDB(value));
    }

    /// <summary>
    /// ������Ч����1(��ʱ����
    /// </summary>
    //public void AdjustSoundVolume(float value)
    //{
    //    soundVolume = value;
    //}

    /// <summary>
    /// ������Ч����2
    /// </summary>
    public void AdjustSoundVolume(float value)
    {
        masterMixer.SetFloat(soundMixerParam, ReflectToDB(value));
    }

    /// <summary>
    /// ��slider�õ���ֵӳ��Ϊ�ֱ�
    /// </summary>
    /// <returns></returns>
    private float ReflectToDB(float value)
    {
        if (value <= 0.0f) value = 0.0001f;
        return Mathf.Log10(value) * 20.0f;
    }

}
