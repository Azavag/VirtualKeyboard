using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// �����, ���������� �� ����� �� ������ ����� ������ �����
/// </summary>
public class KeyboardInputModeSwitcherText : MonoBehaviour
{
    [SerializeField]
    private string joystickModeEnabledText = "�������";
    [SerializeField]
    private string joystickModeDisabledText = "��������";

    [SerializeField]
    private TextMeshProUGUI joystickModeInputText;
    private string startText = "����� ���������: ";


    /// <summary>
    /// ���������� ������ �� ���� �����
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
    /// ���������� ������ �� ������
    /// </summary>
    /// <param name="text"></param>
    private void UpdateButtonText(string text)
    {
        joystickModeInputText.text = $"{startText} {text}";
    }

}
