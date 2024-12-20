using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCurrentGameObject : MonoBehaviour
{
    [SerializeField] private GameObject UIParent;
    public void disableCurrentGameObject()
    {
        UIParent?.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(waitDisable());
    }
    private IEnumerator waitDisable()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }
}
