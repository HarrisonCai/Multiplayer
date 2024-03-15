using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
public class CornCounter : NetworkBehaviour
{
    private StorageHouse corn;
    [SerializeField] private Image counter;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int winCorn;

    // Update is called once per frame
    void Update()
    {
        counter.fillAmount = ((float)(corn.Corn)) / winCorn;
        text.text = "" + corn.Corn + "/" + winCorn;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (corn==null && collision.gameObject.CompareTag("Storage") && collision.gameObject.GetComponent<StorageHouse>().ClientID == OwnerClientId)
        {
            collision.gameObject.GetComponent<StorageHouse>();
        }
    }
}
