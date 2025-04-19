using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CloseWindows : MonoBehaviour
{
    private Button button;
    private GameObject parentPanel; // Ссылка на родительскую Panel

    void Start()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Кнопка не найдена на этом GameObject!");
            enabled = false;
            return;
        }

        // Находим родительскую Panel
        parentPanel = transform.parent.gameObject;

        if (parentPanel == null)
        {
            Debug.LogError("Родительская Panel не найдена! Убедитесь, что кнопка находится внутри Panel.");
            enabled = false;
            return;
        }

        button.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        // Закрываем родительскую Panel (деактивируем GameObject)
        parentPanel.SetActive(false);

        // Альтернативный способ: уничтожить Panel
        // Destroy(parentPanel); // Внимание: уничтожает объект, а не просто скрывает его!
    }
}
