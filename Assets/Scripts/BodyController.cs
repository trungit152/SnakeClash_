using UnityEngine;

public class BodyController : MonoBehaviour
{
    public bool isBit;
    public HeadController headController;
    public HeadController HeadController
    {
        get
        {
            if (headController == null)
            {
                headController = GameObject.Find("PlayerHead").GetComponent<HeadController>();
            }
            return headController;
        }
        set
        {
            headController = value;
        }
    }
    private void Awake()
    {
        isBit = false;
    }
    void Start()
    {

    }
    public int GetLevel()
    {
        return HeadController.level;
    }
    public void Bit()
    {
        isBit = true;
        HeadController.HeadBit();
    }
}
