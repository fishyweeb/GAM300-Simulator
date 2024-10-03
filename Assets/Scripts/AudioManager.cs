using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<AudioSource> ListofAudioSources = new List<AudioSource>();

    [SerializeField]
    AudioSource source;

    public AudioSource BGMSource;


    //hard code babeyyy~
    public AudioClip[] HitSoundsFX;
    public AudioClip[] WhooshSoundsFX;
    public AudioClip[] PlayerHurtFX;
    public AudioClip[] EnemySounds;
    public AudioClip[] MiscSounds;
    public AudioClip[] ExplosionSounds;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
        //if (isGameOver)
        //{
        //    fadeTime += Time.deltaTime;
        //    if (fadeTime < 1f)
        //    {
        //        BGMSource.volume = Mathf.Lerp(0.234f, 0f, fadeTime);
        //        GameOverSource.volume = Mathf.Lerp(0, 0.234f, fadeTime);
        //        GameOverSource.Play();
        //    }
        //}
        // if (GameManager.instance.gameState == GameManager.GameState.GAMEOVER)
        // {
        //     fadeTime += Time.deltaTime;
        //     BGMSource.volume -= Time.unscaledTime;
        //     if (fadeTime < 1f)
        //     {
        //         BGMSource.volume = 0;
        //         //GameOverSource.volume = Mathf.Lerp(0, 0.234f, fadeTime);
        //         //GameOverSource.Play();
        //     }
        // }
    }

    //default volume is 0.3f
    public void PlaySoundAtLocation(AudioClip clip, Vector3 pos)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        temp.clip = clip;
        temp.Play();
        RemoveUnusedAudioSource();
    }
    public void PlaySoundAtLocation(AudioClip clip, float volume, Vector3 pos)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        temp.volume = volume;
        temp.clip = clip;
        temp.Play();
        RemoveUnusedAudioSource();
    }
    public void PlaySoundAtLocation(AudioClip clip, float volume, Vector3 pos, bool randPitch = false)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        if (randPitch) temp.pitch *= Random.Range(0.8f, 2.5f);   //below 0 can't hear anything, 0.5f sounds slowww 2.5f sounds fast
        temp.volume = volume;
        temp.clip = clip;
        temp.Play();
        RemoveUnusedAudioSource();
    }
    public void PlayCachedSound(AudioClip[] clips, Vector3 pos, float volume, bool randPitch = false)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        if (randPitch) temp.pitch *= Random.Range(0.8f, 2.5f);
        temp.volume = volume;
        temp.clip = clips[Random.Range(0, clips.Length)];
        temp.Play();
        RemoveUnusedAudioSource();
    }

    public void StopThisSound(AudioClip clip)
    {
        if (ListofAudioSources != null)
        {
            for (int i = 0; i < ListofAudioSources.Count; i++)
            {
                if (ListofAudioSources[i].clip.Equals(clip))
                {
                    ListofAudioSources[i].Stop();
                    Destroy(ListofAudioSources[i].gameObject);
                    ListofAudioSources.RemoveAt(i);
                }
            }
        }
    }

    void RemoveUnusedAudioSource()
    {
        if (ListofAudioSources != null)
        {
            for (int i = 0; i < ListofAudioSources.Count; i++)
            {
                if (!ListofAudioSources[i].isPlaying)
                {
                    Destroy(ListofAudioSources[i].gameObject);
                    ListofAudioSources.RemoveAt(i);
                }
            }
        }
    }


}
