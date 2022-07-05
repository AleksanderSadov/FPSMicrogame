using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> bgmList;

    private AudioSource bgmSource;

    // Start is called before the first frame update
    void Start()
    {
        bgmSource = GetComponent<AudioSource>();

        if (!bgmSource.playOnAwake)
        {
            PlayRandomTrack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!bgmSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    private void PlayRandomTrack()
    {
        bgmSource.clip = bgmList[Random.Range(0, bgmList.Count)];
        bgmSource.Play();
    }
}
