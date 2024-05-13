using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkinButtons : MonoBehaviour
{
    [SerializeField] private Button backToHomeBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;

    private SnakeSkinCtr snakeSkinCtr;
    private SnakeSkinCtr SnakeSkinCtr
    {
        get
        {
            if (snakeSkinCtr == null)
            {
                snakeSkinCtr = GameObject.Find("SnakeImage").GetComponent<SnakeSkinCtr>();
            }
            return snakeSkinCtr;
        }
        set
        {
            snakeSkinCtr = value;
        }
    }
    private void Awake()
    {
        backToHomeBtn.onClick.AddListener(BackToHomeSceneClick);
        nextBtn.onClick.AddListener(NextBtn);
        prevBtn.onClick.AddListener(PrevBtn);
    }
    private void BackToHomeSceneClick()
    {
        SceneManager.LoadScene("HomeScene");
    }

    private void NextBtn()
    {
        SnakeSkinCtr.LoadNextSkin();
    }
    private void PrevBtn()
    {
        SnakeSkinCtr.LoadPreviousSkin();
    }
}
