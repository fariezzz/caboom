using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject targetGameObject;

    private void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            ToggleGameObject();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameObject();
        }
    }

    private void ToggleGameObject()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(!targetGameObject.activeSelf);
        }
    }
}
