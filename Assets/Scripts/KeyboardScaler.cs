using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за изменение масштаба клавиатуры
/// </summary>
public class KeyboardScaler : MonoBehaviour
{
    [SerializeField]
    private Transform keyboardTransform;
    [Header("UI elements")]
    [SerializeField]
    private Button increaseScaleButton;
    [SerializeField]
    private Button decreaseScaleButton;
    [SerializeField]
    private TextMeshProUGUI scaleValueText;

    [Header("Scale Values")]
    [SerializeField]
    private List<float> scaleList = new List<float>();
    private int currentScaleIndex = 2;

    private void Start()
    {
        UpdateKeyboardScale();
    }
    private void OnEnable()
    {
        increaseScaleButton.onClick.AddListener(IcreaseKeyboardScale);
        decreaseScaleButton.onClick.AddListener(DecreaseKeyboardScale);
    }

    private void OnDisable()
    {
        increaseScaleButton.onClick.RemoveListener(IcreaseKeyboardScale);
        decreaseScaleButton.onClick.RemoveListener(DecreaseKeyboardScale);
    }
    
    /// <summary>
    /// Увеличение масштаба клавиатуры
    /// </summary>
    private void IcreaseKeyboardScale()
    {       
        if (currentScaleIndex < scaleList.Count - 1)
        {
            currentScaleIndex++;
            UpdateScaleButtonsState();
            UpdateKeyboardScale();
        }                
    }
    /// <summary>
    /// Уменьшение масштаба клавиатуры
    /// </summary>
    private void DecreaseKeyboardScale()
    {
        if (currentScaleIndex > 0)
        {
            currentScaleIndex--;
            UpdateScaleButtonsState();
            UpdateKeyboardScale();
        }        
    }

    /// <summary>
    /// Изменение скейла объекта клавиатуры
    /// </summary>
    private void UpdateKeyboardScale()
    {
        keyboardTransform.localScale = new Vector3(scaleList[currentScaleIndex], scaleList[currentScaleIndex], scaleList[currentScaleIndex]);
        UpdateKeyboardScaleText();
    }
    /// <summary>
    /// Обновление текста значения масштаба 
    /// </summary>
    private void UpdateKeyboardScaleText()
    {
        scaleValueText.text = (scaleList[currentScaleIndex] * 100).ToString() + "%";
    }

    /// <summary>
    /// Изменение состояния кнопок изменения масшатаб
    /// </summary>
    private void UpdateScaleButtonsState()
    {
        increaseScaleButton.interactable = currentScaleIndex < scaleList.Count-1;
        decreaseScaleButton.interactable = currentScaleIndex > 0;
    }
}
