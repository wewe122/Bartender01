using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlassManager : MonoBehaviour
{

    public GameObject[] glasses;
    public GameObject[] smallFilledGlasses;
    public GameObject[] largeFilledGlasses;
    public int BigGlassQuantity = 0;
    public int SmallGlassQuantity = 0;
    public TextMeshProUGUI smallGlassTmp;
    public TextMeshProUGUI bigGlassTmp;

    private bool SpacePressed;
    private float MinDistToLiftGlass = 1.62f;
    private float distToReleaseGlass = 2f;
    private GameObject EmptyGlassObject = null;
    private GameObject FullGlassObject = null;
    private Transform PlayerTransform = null;
    private static bool instructionsMode;

    void Awake() 
    {
        instructionsMode = false;
    }
    void Start()
    {
        smallGlassTmp.text = SmallGlassQuantity+"";
        bigGlassTmp.text = BigGlassQuantity+"";
        SpacePressed = false;
        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (EmptyGlassObject != null) // demonstrate the bartender holding the empty glass
            EmptyGlassObject.transform.position = PlayerTransform.position;
        
        if (FullGlassObject != null) // demonstrate the bartender holding the full glass
            FullGlassObject.transform.position = PlayerTransform.position;
        // bartender wants to take an empty glass
        else if (Input.GetKeyDown(KeyCode.Space) && !SpacePressed)
        {
            SpacePressed = true;
            var distToSmallGlass = Vector3.Distance(GameObject.Find("Canvas/Porto_0").GetComponent<SpriteRenderer>().transform.position,
                PlayerTransform.position);
            var distToBigGlass = Vector3.Distance(GameObject.Find("Canvas/chop roja_0").GetComponent<SpriteRenderer>().transform.position,
                PlayerTransform.position);
            Debug.Log("distToBig: " + distToBigGlass + ", distToSmall: " + distToSmallGlass);
            if (EmptyGlassObject == null)
            {
                if (distToBigGlass <= MinDistToLiftGlass && (BigGlassQuantity > 0 || instructionsMode))
                {
                    EmptyGlassObject = Instantiate(glasses[1], PlayerTransform.position, PlayerTransform.rotation);
                    EmptyGlassObject.GetComponent<SpriteRenderer>().enabled = true;
                    bigGlassTmp.text = --BigGlassQuantity + "";
                    Debug.Log("picking up big glass");
                }
                else if (distToSmallGlass <= MinDistToLiftGlass && (SmallGlassQuantity > 0 ||  instructionsMode))
                {
                    EmptyGlassObject = Instantiate(glasses[0], PlayerTransform.position, PlayerTransform.rotation);
                    EmptyGlassObject.GetComponent<SpriteRenderer>().enabled = true;
                    smallGlassTmp.text = --SmallGlassQuantity + "";
                    Debug.Log("picking up small glass");
                }
            }
            // bartender wants to release the empty glass
            else if(distToBigGlass <= distToReleaseGlass || distToSmallGlass <= distToReleaseGlass)
            {
                //if current empty glass is a small one
                if (EmptyGlassObject.GetComponent<SpriteRenderer>().sprite.name == glasses[0].GetComponent<SpriteRenderer>().sprite.name)
                    smallGlassTmp.text = ++SmallGlassQuantity + "";
                else //if current empty glass is a big one
                    bigGlassTmp.text = ++BigGlassQuantity + "";
                Destroy(EmptyGlassObject);
                EmptyGlassObject = null;
            }

        }

        // avoid multiple actions from one long press
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpacePressed = false;
        }

    }
    public GameObject Get()
    {
        return FullGlassObject;        
    }
    public void ReleaseEmptyGlass()
    {
        EmptyGlassObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void ReleaseFullGlass()
    {
        FullGlassObject = null;
    }
    public bool isHoldingEmptyGlass()
    {
        return EmptyGlassObject != null;
    }
    public bool isHoldingFullGlass()
    {
        return FullGlassObject != null;
    }
    public void FillGlass(int type)
    {
        FullGlassObject = EmptyGlassObject.GetComponent<SpriteRenderer>().sprite.name == "Porto_0" ?
            Instantiate(smallFilledGlasses[type], PlayerTransform.position, PlayerTransform.rotation) :
            Instantiate(largeFilledGlasses[type], PlayerTransform.position, PlayerTransform.rotation);
        
        Destroy(EmptyGlassObject);// destroy empty glass object
        EmptyGlassObject = null;
        //build new filled glass object
        FullGlassObject.GetComponent<SpriteRenderer>().enabled = true;

    }
    public static void ToggleInstructionsMode()
    {
        instructionsMode = !instructionsMode;
    }
}