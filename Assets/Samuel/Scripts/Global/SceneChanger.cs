using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public void FadeToScene(int _index)
    {
        StartCoroutine(FadeToSceneCoRoutine(_index));
    }

    private IEnumerator FadeToSceneCoRoutine(int _index)
    {
        GameObject.FindWithTag("FaderOverlay").GetComponent<FaderOverlay>().FadeOut();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(_index);
        Time.timeScale = 1f;
    }
}
