using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform TargetPos = null;
    [SerializeField] private AudioClip TeleportSound = null;
    private AudioSource AM;

    private void Awake()
    {
        AM = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }
    private void Teleportation(Transform obj)
    {
        AM.PlayOneShot(TeleportSound);
        obj.position = TargetPos.position;
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if(CrashObj.gameObject.CompareTag("Player"))
            Teleportation(CrashObj.gameObject.transform);
    }
}
