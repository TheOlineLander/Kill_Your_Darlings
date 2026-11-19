using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
   [SerializeField] private GameObject panel, finishedText, unfinishedText;
    private float delayBeforeNextScene = 2f;

    private Animator anim;
    private int currentSceneIndex;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            panel.SetActive(true);
            finishedText.SetActive(true);
            anim.SetTrigger("Flag");

            StartCoroutine(LoadNextSceneAfterDelay());
        }
    }

private IEnumerator LoadNextSceneAfterDelay()
{
    yield return new WaitForSeconds(delayBeforeNextScene);

    int next = SceneManager.GetActiveScene().buildIndex + 1;
    SceneManager.LoadScene(next < SceneManager.sceneCountInBuildSettings ? next : 0);
}


private void OnTriggerExit2D(Collider2D collision)
    {
        panel.SetActive(false);
        finishedText.SetActive(false);
        unfinishedText.SetActive(false);
    }
}
