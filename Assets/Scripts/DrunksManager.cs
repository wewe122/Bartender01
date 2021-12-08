using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunksManager : MonoBehaviour
{
    public GameObject drunkToSpawn;
    public GameObject[] positions;
    public Sprite[] beersSprites;
    public Vector3 startPosition;
    public float delayBetweenDrunks = 10f;
    public float DrunksSpeed;
    public bool instructionsMode = false;

    [SerializeField]
    public instructions instructions;


    private List<GameObject> drunks;
    private int current = 0;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (instructionsMode)
            return;
        drunks = new List<GameObject>();
        GameObject go = Instantiate(drunkToSpawn, startPosition, transform.rotation);
        drunks.Add(go); 
        go.transform.Find("dialogo/BeerDrunkWants").GetComponent<SpriteRenderer>().sprite =
                beersSprites[current % beersSprites.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (instructionsMode)
            return;
        timer += Time.deltaTime;
        if (drunks[current].transform.position != positions[current].transform.position)
            moveDrunk();
        else if (timer > delayBetweenDrunks)
        {
            current = current < positions.Length - 1 ? current + 1 : 0;
            timer = 0;
            GameObject go = Instantiate(drunkToSpawn, startPosition, transform.rotation);
            drunks.Add(go);
            go.transform.Find("dialogo/BeerDrunkWants").GetComponent<SpriteRenderer>().sprite = 
                beersSprites[current % beersSprites.Length];
        }
    }

    void moveDrunk()
    {
        drunks[current].transform.position = Vector2.MoveTowards(drunks[current].transform.position,
                                                                 positions[current].transform.position,
                                                                 DrunksSpeed * Time.deltaTime);
    }
    public void Serve(GameObject glass, GameObject drunk)
    {
        if (instructions != null)
        {
            instructions.Serve(glass, drunk);
            return;
        }

        Debug.Log(glass.GetComponent<SpriteRenderer>().sprite.name + ", " + drunk.transform.Find("dialogo/BeerDrunkWants").GetComponent<SpriteRenderer>().sprite.name);
        if (glass.GetComponent<SpriteRenderer>().sprite.name
            == drunk.transform.Find("dialogo/BeerDrunkWants").GetComponent<SpriteRenderer>().sprite.name)
        {
            Destroy(drunk.transform.Find("dialogo").gameObject);
            Debug.Log("good serve");
        }
        else
        {
            Debug.Log("bad serve");
        }
    }
}
