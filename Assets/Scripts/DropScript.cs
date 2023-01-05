using UnityEngine;
using System.Collections;

public class DropScript : MonoBehaviour {

    public float counter;

	// Use this for initialization
	void Start () {
        counter = .84f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.down * Time.deltaTime / .84f);
        counter -= Time.deltaTime;
        if(counter<=0)
        {
            int x = Mathf.RoundToInt((transform.position - GameControl.singleton.transform.position).x);
            int y = Mathf.RoundToInt((transform.position - GameControl.singleton.transform.position).y);
            transform.position = (Vector2)GameControl.singleton.transform.position + new Vector2(x, y);
            GetComponent<OrbScript>().X = x;
            GetComponent<OrbScript>().Y = y;
            Destroy(this);
        }
	
	}
}
