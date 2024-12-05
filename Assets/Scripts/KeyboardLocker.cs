using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за присоединение клавиатуры к телу пользователя
/// </summary>
public class KeyboardLocker : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [Header("UI Elements")]
    [SerializeField]
    private Button lockButton;
    [SerializeField]
    private Image lockButtonImage;
    [Header("UI Elements - sprites")]
    [SerializeField]
    private Sprite lockedSprite;
    [SerializeField]
    private Sprite unlockedSprite;

    private bool isKeyboardLockedToPlayer = false;

    private void OnEnable()
    {
        lockButton.onClick.AddListener(ToggleLockKeyboard);
    }

    private void OnDisable()
    {
        lockButton.onClick.RemoveListener(ToggleLockKeyboard);
    }
    /// <summary>
    /// Тогл состояния клавиатуры
    /// </summary>
    private void ToggleLockKeyboard()
    {
        isKeyboardLockedToPlayer = !isKeyboardLockedToPlayer;
        if (isKeyboardLockedToPlayer)
        {
            LockKeyboard();
        }
        else 
        {
            UnlockKeyboard();
        }
    }
    /// <summary>
    /// Присоединение клавиатуры к пользователю
    /// </summary>
    private void LockKeyboard()
    {
        SetButtonImage(lockedSprite);
        transform.SetParent(playerObject.transform);
    }
    /// <summary>
    /// Отсоединие клавиатуры от пользователя
    /// </summary>
    private void UnlockKeyboard()
    {
        SetButtonImage(unlockedSprite);
        transform.SetParent(null);
    }
    /// <summary>
    /// Смена спрайта на кнопке
    /// </summary>
    /// <param name="sprite"></param>
    private void SetButtonImage(Sprite sprite)
    {
        lockButtonImage.sprite = sprite; 
    }


}
