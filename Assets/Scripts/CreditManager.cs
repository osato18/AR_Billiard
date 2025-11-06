using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    [SerializeField] private GameObject _creditPanel;
    public void OpenCreditPanel()
    {
        _creditPanel.SetActive(true);
    }
    public void CloseCreditPanel()
    {
        _creditPanel.SetActive(false);
    }
}
