using UnityEngine;

public class MusicController : MonoBehaviour
{
    public GameObject song1;
    public GameObject song2;

    private AudioSource audioSource;
    private bool isSong1Playing = true;

    private void Start()
    {
        audioSource = song1.GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isSong1Playing)
            {
                audioSource.Stop();
                audioSource = song2.GetComponent<AudioSource>();
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
                audioSource = song1.GetComponent<AudioSource>();
                audioSource.Play();
            }
            isSong1Playing = !isSong1Playing;
        }
    }
}


