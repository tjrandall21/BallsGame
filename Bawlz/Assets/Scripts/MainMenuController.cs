using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup OptionPanel;
    [SerializeField] private CanvasGroup PlayPanel;

    

    public void OpenOptions()
    {
        OptionPanel.alpha = 1;
        OptionPanel.interactable = true;
        OptionPanel.blocksRaycasts = true;
    }

    public void StartGame()
    {
        PlayPanel.alpha = 1;
        PlayPanel.interactable = true;
        PlayPanel.blocksRaycasts = true;
    }

    public void PlayBack() //Back button for the play panel
    {
        PlayPanel.alpha = 0;
        PlayPanel.interactable = false;
        PlayPanel.blocksRaycasts = false;
    }
    public void Back() //Back button for the options panel
    {
        OptionPanel.alpha = 0;
        OptionPanel.interactable = false;
        OptionPanel.blocksRaycasts = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
