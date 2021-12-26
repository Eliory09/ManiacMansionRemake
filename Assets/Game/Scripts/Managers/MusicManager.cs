using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class MusicManager : MonoBehaviour
{
  #region Fields

  private static MusicManager _shared;

  private AudioSource _audio;

  #endregion


  #region MonoBehaviour

  private void Awake ()
  {
    // Init as shared manager throughout the game, or delete this object since
    // we already have a manager.
    if ( _shared == null )
    {
      // First time opening the scene – set as shared manager
      _shared = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      // Already exist, no need for another manager
      Destroy(this);
    }

    // Get audio source
    _audio = GetComponent<AudioSource>();
  }

  private void Start ()
  {
    // We don’t set the AudioSource to play on load to prevent tiny noise before
    // deleting unwanted managers on scene switch.
    _audio.Play();
  }

  #endregion


  #region Methods

  /// <summary>
  ///   Replace the background music plating.
  /// </summary>
  /// <param name="music">Music to play</param>
  public static void ChangeMusic (AudioClip music)
  {
    _shared._audio.clip = music;

    // When replacing a clip we need to explicitly Play.
    _shared._audio.Play();
  }

  /// <summary>
  ///   Play a sound effect in addition to the music.
  /// </summary>
  /// <param name="effect">Sound effect to play</param>
  public static void PlayEffect (AudioClip effect)
  {
    _shared._audio.PlayOneShot(effect);
  }
  
  /// <summary>
  ///   Set audio volume. Parameter is between 0.0-1.0.
  /// </summary>
  /// <param name="vol">Volume (is between 0.0-1.0)</param>
  public static void SetVolume (float vol)
  {
    _shared._audio.volume = vol;
  }
  
  /// <summary>
  ///   Set loop value.
  /// </summary>
  /// <param name="val">Boolean value. True will loop the current clip.</param>
  public static void SetLoop (bool val)
  {
    _shared._audio.loop = val;
  }

  #endregion
}