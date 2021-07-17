using UnityEngine;

/// <summary>
/// Controls scripted tutorial events
/// </summary>
public class TutorialScript : MonoBehaviour
{
    private PlayerControls playerControls;

    public int tutorialStage = 0;
    public GameObject[] tutorialTooltips = new GameObject[10];
    public GameObject[] tutorialObjects1 = new GameObject[10];
    public GameObject[] tutorialObjects2 = new GameObject[10];
    public GameObject[] tutorialObjects3 = new GameObject[10];
    private GameObject enemy;
    public Transform room0;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Tutorial.AdvanceTutorial.performed += context => AdvanceTutorial();
        }

        playerControls.Enable();
    }

    private void Update()
    {
        if(tutorialStage == 7)
        {
            if (!enemy || GameObject.FindWithTag("GameManager").GetComponent<GameManager>().playerCharacter.gameObject == enemy)
                AdvanceTutorial();
        }
    }

    private void UpdateTutorial()
    {
        switch (tutorialStage)
        {
            case 0:
                break;
            case 1:
                //pausemenu
                tutorialTooltips[0].SetActive(true);
                break;
            case 2:
                //controls
                ShowTooltip(1);
                break;
            case 3:
                //hp&stamina
                ShowTooltip(2);
                ShowObjects(tutorialObjects1, true);
                break;
            case 4:
                //permadeath
                ShowTooltip(3);
                ShowObjects(tutorialObjects2, true);
                break;
            case 5:
                //heal
                ShowTooltip(4);
                ShowObjects(tutorialObjects3, true);
                break;
            case 6:
                //ready
                ShowTooltip(5);
                break;
            case 7:
                //hyi vittu mutta iha sama toimii lol :D 
                ShowTooltip(6);
                if (GameObject.FindWithTag("GameManager").GetComponent<GameManager>().playerFaction == Character.Faction.Blue)
                    enemy = Instantiate(Resources.Load("Characters/Enemy/AiSpider"), Vector2.zero, Quaternion.identity, room0) as GameObject;
                else
                    enemy = Instantiate(Resources.Load("Characters/Enemy/AiHuman"), Vector2.zero, Quaternion.identity, room0) as GameObject;

                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().UpdateAIs();
                break;
            case 8:
                ShowTooltip(7);
                break;
            case 9:
                ShowTooltip(8);
                break;
            case 10:
                ShowTooltip(9);
                break;

        }
    }
    public void AdvanceTutorial()
    {
        if (tutorialStage == 7) return;

        tutorialStage++;
        UpdateTutorial();
    }

    private void ShowObjects(GameObject[] _objects, bool _active)
    {
        foreach (GameObject go in _objects)
        {
            go.SetActive(_active);
        }
    }

    private void ShowTooltip(int _index)
    {
        tutorialTooltips[_index - 1].SetActive(false);
        tutorialTooltips[_index].SetActive(true);
    }
}
