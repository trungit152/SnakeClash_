using UnityEngine;

public class ItemController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHead") || other.CompareTag("Head"))
        {
            Destroy(gameObject);
        }
    }
}
