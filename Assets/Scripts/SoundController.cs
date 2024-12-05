using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс управления звуками
/// </summary>
public class SoundController : MonoBehaviour
{
    //Звук клика по клавише
    [SerializeField]
    private AudioClip buttonClickClip;     
    //Звук смены клавиш
    [SerializeField]
    private AudioClip buttonSwapClip;

    /// <summary>
    /// Воспроизведение звука смены клавиш
    /// </summary>
    /// <param name="keyboardPosition"></param>
    public void PlayButtonSwapSoundAt(Vector3 keyboardPosition)
    {
        GameObject tempObject = new GameObject("SpatialAudio - Swap");
        tempObject.transform.position = keyboardPosition;
        PlayClip(tempObject, buttonSwapClip);
    }
    /// <summary>
    /// Воспроизведение звука клика по клавише
    /// </summary>
    /// <param name="keyboardPosition"></param>
    public void PlayButtonClickSoundAt(Vector3 keyboardPosition)
    {
        GameObject tempObject = new GameObject("SpatialAudio - Click");
        tempObject.transform.position = keyboardPosition;
        PlayClip(tempObject, buttonClickClip);
    }
    /// <summary>
    /// Воспроизведение выбранного клипа
    /// </summary>
    /// <param name="placeObject"></param>
    /// <param name="clip"></param>
    private void PlayClip(GameObject placeObject, AudioClip clip)
    {
        AudioSource source = placeObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        Destroy(placeObject, clip.length);
    }
}
