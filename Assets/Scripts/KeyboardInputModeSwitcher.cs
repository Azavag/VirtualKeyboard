using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����� ����� 
/// </summary>
public enum KeyboardInputMode
{
    Raycast,
    Joystick,
}
/// <summary>
/// �����, ���������� �� ����� ������ �����
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
    /// ���������� ������� �� ������ ����� ������ �����
    /// </summary>
    private void SwitchInputMode()
    {
        currentInputMode = currentInputMode == KeyboardInputMode.Raycast ? KeyboardInputMode.Joystick : KeyboardInputMode.Raycast;
        ToggleInputMode();
    }
    /// <summary>
    /// ����� ������ �����
    /// </summary>
    private void ToggleInputMode()
    {
        //���� ����������� ���� ������������ � ������� ���������
        smoothLocomotion.UpdateMovement = !smoothLocomotion.UpdateMovement;
        //���� ������ ����� � ���������
        keyboardJoystickInputMode.ToggleJoystickMode();
        keyboardInputModeSwitcherText.UpdateInputModeButton(currentInputMode);
    }

}
