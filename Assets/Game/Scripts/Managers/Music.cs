 using UnityEngine;
 
 /// <summary>
 /// Music manager.
 /// CURRENTLY NOT IN USE.
 /// </summary>
 public class Music : MonoBehaviour
 {
     private AudioSource _audioSource;
     private void Awake()
     {
         DontDestroyOnLoad(transform.gameObject);
         _audioSource = GetComponent<AudioSource>();
     }
 
     public void PlayMusic()
     {
         if (_audioSource.isPlaying) return;
         _audioSource.Play();
     }
 
     public void StopMusic()
     {
         _audioSource.Stop();
     }
 }
