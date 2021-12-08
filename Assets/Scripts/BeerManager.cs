using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerManager : MonoBehaviour
{
    [SerializeField]
    public GlassManager glassManager;

    [SerializeField]
    public DrunksManager drunkManager;

    private bool UpArrowPressed;
    private bool carryingBeer;
    private GameObject glass;
    private float DistanceToServe = 4.515f;

    void Start()
    {
        UpArrowPressed = false;
    }

    void Update()
    {
       

        // avoid multiple actions from one long press
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            UpArrowPressed = false;
        }

        // handle the release of beer from bartender hands
        if (glassManager.isHoldingFullGlass() && Input.GetKeyDown(KeyCode.UpArrow) && !UpArrowPressed)
        {
            Debug.Log("Trying to serve beer");
            UpArrowPressed = true;
            GameObject[] goarray = GameObject.FindGameObjectsWithTag("Drunk");
            GameObject closestDrunk = null;
            float minDist = float.MaxValue;
            
            // search for the closest drunk
            foreach (var drunk in goarray)
            {
                var dist = Vector3.Distance(drunk.transform.position, transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestDrunk = drunk;
                }
            }
            Debug.Log(minDist);

           /* 
            * make sure the bartender is not too far from the drunk guy
            * if so, serve him the beer
            */
            if (minDist <= DistanceToServe)
            {
                glass = glassManager.Get();
                glass.GetComponent<SpriteRenderer>().sortingLayerName = "Drunks";
                glass.GetComponent<SpriteRenderer>().sortingOrder = 1;
                glass.transform.position = closestDrunk.transform.position;
                glass.GetComponent<Animator>().enabled = true;
                drunkManager.Serve(glass, closestDrunk);
                glassManager.ReleaseFullGlass();
            }
            
        }
        // avoid multiple actions from one long press
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            UpArrowPressed = false;
        }



    }
}
