using UnityEngine;
using System.Collections;

public class LicenseManager : MonoBehaviour
{
    public AudioSource voice;

    // Use this for initialization
    void Start()
    {
        voice.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!voice.isPlaying)
        {
            System.Threading.Thread.Sleep(1000);
            Application.LoadLevel("Title");
        }
    }
}
