using TMPro;
using UnityEngine;

public class PlayerNameTag : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.2f, 0f);
    [SerializeField] private Transform target;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;

        if (target == null)
            target = transform.parent;

        if (PlayerSession.Instance != null && PlayerSession.Instance.IsLoggedIn)
        {
            nameText.text = PlayerSession.Instance.CurrentUsername;
        }
        else
        {
            nameText.text = "Gast";
        }
    }

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;

        if (mainCam != null)
        {
            transform.forward = mainCam.transform.forward;
        }
    }
}