using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip music;

    [SerializeField] private int channelsAmount;
    private List<AudioSource> sources = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        InstantiateAudioSources();
        PlaySource(0, music);
    }

    void InstantiateAudioSources()
    {
        GameObject child = transform.GetChild(0).gameObject;
        for (int i = 0; i < channelsAmount; i++)
        {
            AudioSource source = child.AddComponent<AudioSource>();
            sources.Add(source);
        }
    }
    public void PlaySource(int id = -1, AudioClip clip = null, bool randomise = false)
    {
        // Check if sources array or clip is null
        if (sources == null || sources.Count == 0 || clip == null)
        {
            print("Sources array is null or empty, or clip is null.");
            return;
        }

        // Find the first available audio source if id is -1
        if (id == -1)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                if (!sources[i].isPlaying)
                {
                    id = i;
                    break;
                }
            }

            // Return if no available source is found
            if (id == -1)
            {
                print("No available audio source found.");
                return;
            }
        }

        // Check if id is within valid range
        if (id < 0 || id >= sources.Count)
        {
            print("Invalid source ID.");
            return;
        }

        // Set the clip and play the source
        sources[id].clip = clip;
        if (randomise)
            RandomiseSource(sources[id]);
        sources[id].Play();

        if (clip == music)
        {
            sources[id].loop = true;
        }
    }

    void RandomiseSource(AudioSource source)
    {
        source.pitch = 2;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
