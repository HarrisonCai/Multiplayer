using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
public class HealthDisplay : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Health hp;
    private float target, currentFill;
    [SerializeField] private Image image;
    [SerializeField] private Gradient HpGrad;
    void Start()
    {
        UpdateHealthBarServerRpc();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBarServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateHealthBarServerRpc()
    {
        UpdateHealthBarClientRpc();
    }
    [ClientRpc(RequireOwnership =false)]
    public void UpdateHealthBarClientRpc()
    {
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        target = hp.HP / hp.MaxHP;
        currentFill = image.fillAmount;
        image.fillAmount = Mathf.Lerp(currentFill, target, Time.deltaTime * 10);
        colorCheck();

    }

    private void colorCheck()
    {
        image.color = HpGrad.Evaluate(image.fillAmount);
    }
}
