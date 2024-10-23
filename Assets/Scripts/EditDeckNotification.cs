using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditDeckNotification : MonoBehaviour
{
    [SerializeField] private float time_;
    [SerializeField] private GameObject notification_;

    private int notifications_ = 0;

    public void Show(string notification)
    {
        ++notifications_;
        GetComponentInChildren<Text>().text = notification;
        notification_.SetActive(true);

        Invoke("Close", time_);
    }

    void Close()
    {
        if (--notifications_ == 0)
            notification_.SetActive(false);
    }
}
