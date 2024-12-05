using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ���������� �������
/// </summary>
public class SoundController : MonoBehaviour
{
    //���� ����� �� �������
    [SerializeField]
    private AudioClip buttonClickClip;     
    //���� ����� ������
    [SerializeField]
    private AudioClip buttonSwapClip;

    /// <summary>
    /// ��������������� ����� ����� ������
    /// </summary>
    /// <param name="keyboardPosition"></param>
    public void PlayButtonSwapSoundAt(Vector3 keyboardPosition)
    {
        GameObject tempObject = new GameObject("SpatialAudio - Swap");
        tempObject.transform.position = keyboardPosition;
        PlayClip(tempObject, buttonSwapClip);
    }
    /// <summary>
    /// ��������������� ����� ����� �� �������
    /// </summary>
    /// <param name="keyboardPosition"></param>
    public void PlayButtonClickSoundAt(Vector3 keyboardPosition)
    {
        GameObject tempObject = new GameObject("SpatialAudio - Click");
        tempObject.transform.position = keyboardPosition;
        PlayClip(tempObject, buttonClickClip);
    }
    /// <summary>
    /// ��������������� ���������� �����
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
