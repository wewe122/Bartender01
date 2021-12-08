using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class instructions : MonoBehaviour
{
    public Button continueBtn;
    public Button nextBtn;
    public TextMeshProUGUI[] instructionsTexts;
    public TextMeshProUGUI[] missionTexts;
    public TextMeshProUGUI greatText;
    public TextMeshProUGUI tryAgainText;
    public float DELAY_BETWEEN_TEXTS = 5f;
    public GameObject pauseButton;

    private int current = 0;
    private const int MOVE_INDEX = 9;
    private const float TRY_AGAIN_DELAY = 1.5f;
    private bool Paused = false;

    void Start()
    {
        instructionsTexts[current].gameObject.SetActive(true);
        BarManMovement.ToggleFreezeMovement();// freeze bartender movement
        GlassManager.ToggleInstructionsMode();// inform glass manager we`re in instructions mode
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void playTutorial()
    {
        StartCoroutine(Tutorial());
    }
    public void OnClick() 
    {
        instructionsTexts[current++].gameObject.SetActive(false);
        if (current < instructionsTexts.Length)
            instructionsTexts[current].gameObject.SetActive(true);
        else 
        {
            continueBtn.gameObject.SetActive(false);
            current = 0;
            missionTexts[current].gameObject.SetActive(true);
        }
    }
    public void Serve(GameObject glass, GameObject drunk)
    {
        if (glass.GetComponent<SpriteRenderer>().sprite.name
            == drunk.transform.Find("dialogo/BeerDrunkWants").GetComponent<SpriteRenderer>().sprite.name)
        {
            greatText.gameObject.SetActive(true);
            nextBtn.gameObject.SetActive(true);
            Destroy(drunk.transform.Find("dialogo").gameObject);
            Debug.Log("good serve");
            BarManMovement.ToggleFreezeMovement();
        }
        else
        {
            Debug.Log("bad serve");
            StartCoroutine(DisplayTextDestroyGlass(glass));
        }
    }
    public void Next()
    {
        BarManMovement.ToggleFreezeMovement();
        greatText.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        missionTexts[current++].gameObject.SetActive(false);
        if(current < missionTexts.Length)
            missionTexts[current].gameObject.SetActive(true);
        else 
        {
            SceneManager.LoadScene("firstScene");
            Debug.Log("next scene");
        }
    }
    public void TogglePause(Sprite[] play)
    {
    }
    public IEnumerator Tutorial()
    {
        if (!Paused)
            yield return null;
        yield return new WaitForSeconds(DELAY_BETWEEN_TEXTS);
        instructionsTexts[current++].gameObject.SetActive(false);
        if (current < MOVE_INDEX)
        {
            instructionsTexts[current].gameObject.SetActive(true);
            StartCoroutine(Tutorial());
        }
        else 
        {
            instructionsTexts[current].gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            continueBtn.gameObject.SetActive(true);
            BarManMovement.ToggleFreezeMovement();
        }
    }
    public IEnumerator DisplayTextDestroyGlass(GameObject glass)
    {
        tryAgainText.gameObject.SetActive(true);
        yield return new WaitForSeconds(TRY_AGAIN_DELAY);
        Destroy(glass);
        tryAgainText.gameObject.SetActive(false);

    }

}
