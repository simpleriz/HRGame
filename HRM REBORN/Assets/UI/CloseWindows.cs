using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CloseWindows : MonoBehaviour
{
    private Button button;
    private GameObject parentPanel; // ������ �� ������������ Panel

    void Start()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("������ �� ������� �� ���� GameObject!");
            enabled = false;
            return;
        }

        // ������� ������������ Panel
        parentPanel = transform.parent.gameObject;

        if (parentPanel == null)
        {
            Debug.LogError("������������ Panel �� �������! ���������, ��� ������ ��������� ������ Panel.");
            enabled = false;
            return;
        }

        button.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        // ��������� ������������ Panel (������������ GameObject)
        parentPanel.SetActive(false);

        // �������������� ������: ���������� Panel
        // Destroy(parentPanel); // ��������: ���������� ������, � �� ������ �������� ���!
    }
}
