using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAlertMessage : MonoBehaviour, ISubscriber
{
    public void Init()
    {
        imageAlert = GetComponentInChildren<ImageAlertMessage>();
        imageAlert.Init();

        Subscribe();
        gameObject.SetActive(false);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        if (_message.Equals(EMessageType.DANGER_ALERT))
        {
            gameObject.SetActive(true);
            StartCoroutine(AlertDangerCoroutine());
        }
    }

    private IEnumerator AlertDangerCoroutine()
    {
        imageAlert.AlertDanger();
        yield return new WaitForSeconds(alertDelay);

        gameObject.SetActive(false);
    }

    private ImageAlertMessage imageAlert = null;
    [SerializeField]
    private float alertDelay = 3f;
}
