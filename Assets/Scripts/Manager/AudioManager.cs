using System.Collections.Generic;
using UnityEngine;




public class AudioManager : MonoBehaviour
{
    private AudioSource audioSourceInst;

    [SerializeField]
    private AudioClip[] cardSFX = new AudioClip[2];
    [SerializeField]
    private List<AudioClip> enemyOutroVoicelines;
    [SerializeField]
    private AudioClip defeatSFX;
    [SerializeField]
    private List<AudioClip> enemyWinsVoicelines;
    [SerializeField]
    AudioClip clipMatchCard;
    [SerializeField]
    AudioClip clipAccessDenied;
    [SerializeField]
    List<AudioClip> clipTrap;
    [SerializeField]
    private AudioClip timeBonusSFX;
    [SerializeField]
    private AudioClip timeDownSFX;
    [SerializeField]
    private List<AudioClip> clipDmgTraps;

    private void Awake()
    {
        audioSourceInst = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a sound effect at full volume
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        audioSourceInst.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays a sound effect at a specified volume
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume)
    {
        audioSourceInst.PlayOneShot(clip, volume);
    }

    /// <summary>
    /// Plays a random sound effect from the clipTrap list at a specified volume
    /// </summary>
    public void PlayRndTrapVoicelineSFX(float volume)
    {
        int randomIndex;
        AudioClip randomSelectedClip;

        randomIndex = Random.Range(0, clipTrap.Count);
        randomSelectedClip = clipTrap[randomIndex];

        PlaySFX(randomSelectedClip, volume);
    }

    //Getter
    public AudioSource AudioSourceInst {get{return audioSourceInst;}}
    public AudioClip[] CardSFXClips {get{return cardSFX;}}
    public List<AudioClip> EnemyOutroVoiceline {get {return enemyOutroVoicelines;}}
    public AudioClip DefeatSFX {get {return defeatSFX;}}
    public List<AudioClip> EnemyWinsVoicelines {get {return enemyWinsVoicelines;}}
    public AudioClip ClipMatchCard {get {return clipMatchCard;}}
    public AudioClip ClipAccessDenied {get {return clipAccessDenied;}}    
    public AudioClip TimeBonusSFX {get{return timeBonusSFX;}}
    public AudioClip TimeDownSFX {get{return timeDownSFX;}}
    public List<AudioClip> ClipDmgTraps {get{return clipDmgTraps;}}
}
