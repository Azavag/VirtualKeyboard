using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Режим ввода 
/// </summary>
public enum KeyboardInputMode
{
    Raycast,
    Joystick,
}
/// <summary>
/// Класс, отвечающий за смену режима ввода
/// </summary>
public class KeyboardInputModeSwitcher : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private SmoothLocomotion smoothLocomotion;
    [SerializeField]
    private KeyboardJoystickInputMode keyboardJoystickInputMode;
    [SerializeField]
    private KeyboardInputModeSwitcherText keyboardInputModeSwitcherText;

    [SerializeField]
    private UnityEngine.UI.Button switchInputModeButton;
    KeyboardInputMode currentInputMode = KeyboardInputMode.Raycast;

    private void OnEnable()
    {
        switchInputModeButton.onClick.AddListener(SwitchInputMode);
    }

    private void OnDisable()
    {
        switchInputModeButton.onClick.RemoveListener(SwitchInputMode);
    }
    private void Start()
    {
        keyboardInputModeSwitcherText.UpdateInputModeButton(currentInputMode);
    }
    /// <summary>
    /// Обработчик нажатия по кнопке смены режима ввода
    /// </summary>
    private void SwitchInputMode()
    {
        currentInputMode = currentInputMode == KeyboardInputMode.Raycast ? KeyboardInputMode.Joystick : KeyboardInputMode.Raycast;
        ToggleInputMode();
    }
    /// <summary>
    /// Смена режима ввода
    /// </summary>
    private void ToggleInputMode()
    {
        //Тогл перемещения тела пользователя с помощью джойстика
        smoothLocomotion.UpdateMovement = !smoothLocomotion.UpdateMovement;
        //Тогл режима ввода с джойстика
        keyboardJoystickInputMode.ToggleJoystickMode();
        keyboardInputModeSwitcherText.UpdateInputModeButton(currentInputMode);
    }

}
