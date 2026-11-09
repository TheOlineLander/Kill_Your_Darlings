using UnityEngine;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private GameObject panel, finishedText, unfinishedText;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           if(other.GetComponent<PlayerQuest>().GetStrawberries() >= other.GetComponent<PlayerQuest>().GetStrawberriesToCollect())
            {
                panel.SetActive(true);
                finishedText.SetActive(true);
                anim.SetTrigger("Flag");
                //TODO change level
            }
            else
            {
                panel.SetActive(true);
                unfinishedText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        panel.SetActive(false);
        finishedText.SetActive(false);
        unfinishedText.SetActive(false);
    }
}
