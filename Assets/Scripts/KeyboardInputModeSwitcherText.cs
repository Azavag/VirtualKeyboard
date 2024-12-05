using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Класс, отвечающий за текст на кнопке смены режима ввода
/// </summary>
public class KeyboardInputModeSwitcherText : MonoBehaviour
{
    [SerializeField]
    private string joystickModeEnabledText = "включен";
    [SerializeField]
    private string joystickModeDisabledText = "выключен";

    [SerializeField]
    private TextMeshProUGUI joystickModeInputText;
    private string startText = "Режим джойстика: ";


    /// <summary>
    /// Обновление кнопки от типа ввода
    /// </summary>
    /// <param name="inputMode"></param>
    public void UpdateInputModeButton(KeyboardInputMode inputMode)
    {
        switch (inputMode) 
        {
            case KeyboardInputMode.Raycast:
                UpdateButtonText(joystickModeDisabledText);
                break;
            case KeyboardInputMode.Joystick:
                UpdateButtonText(joystickModeEnabledText);
                break;
        }
    }
    /// <summary>
    /// Обновление текста на кнопке
    /// </summary>
    /// <param name="text"></param>
    private void UpdateButtonText(string text)
    {
        joystickModeInputText.text = $"{startText} {text}";
    }

}
