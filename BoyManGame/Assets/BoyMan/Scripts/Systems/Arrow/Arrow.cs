using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    //prefab for arrow head
    public GameObject ArrowHeadPrefab;
    //prefab for arrow nodes
    public GameObject ArrowNodePrefab;

    //number of arrow nodes
    public int arrowNodeNum;

    //scale factor for the arrow nodes
    public float scaleFactor =1f;

    //rect transform for the arrow
    private RectTransform orgin;

    //List to store arrow nodes
    private List<RectTransform> arrowNodes = new List<RectTransform>();

    //List to store control points for the arrow curve
    private List<Vector2> controlPoints = new List<Vector2>();

    // List of control point factors for the arrow curve
    private readonly List<Vector2> controlPointFactos = new List<Vector2> {new Vector2 (-0.3f,0.8f), new Vector2(0.1f, 1.4f) };


private void Awake(){
    // Get the RectTransform component of the arrow
    this.orgin = this.GetComponent<RectTransform>();

    for(int i = 0; i < this.arrowNodeNum; ++i)
    {
        //Instantiate arrow nodes and add it to the list
        this.arrowNodes.Add(Instantiate(this.ArrowNodePrefab, this.transform).GetComponent<RectTransform>());
    }
    //Instantiate arrow head and add it to the list
    this.arrowNodes.Add(Instantiate(this.ArrowHeadPrefab, this.transform).GetComponent<RectTransform>());

    //Set initial positions of arrow nodes off-screen
    this.arrowNodes.ForEach (a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

    
    for (int i = 0; i <4; ++i){
        //Add four zero vectors as initial control points
        this.controlPoints.Add(Vector2.zero);
    }
}


    // Update is called once per frame
    void Update()
    {
        // Set the first control point to the position of the arrow
        this.controlPoints[0] = new Vector2(this.orgin.position.x, this.orgin.position.y);

        // Set the last control point to the current mouse position
        this.controlPoints[3] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

    // Calculate the second control point based on the first control point and the control point factor
    this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactos[0];
    // Calculate the third control point based on the first control point and the control point factor
    this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactos[1];


    for(int i = 0; i < this.arrowNodes.Count; ++i){

        // Calculate t value for the current arrow node
        var t = Mathf.Log(1f * i/ (this.arrowNodes.Count - 1) + 1f, 2f);


        this.arrowNodes[i].position = 
        Mathf.Pow(1 - t, 3) * this.controlPoints[0] + 
        3 * Mathf.Pow(1 - t, 2) * t * this.controlPoints[1] + 
        3 * (1 - t) * Mathf.Pow(t,2)* this.controlPoints[2] + 
        Mathf.Pow(t,3) * this.controlPoints[3];

        //Calculate Rotation
        if(i > 0 ){
            var euler = new Vector3(0,0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i - 1].position));
            this.arrowNodes[i].rotation = Quaternion.Euler(euler);
        }

        //calculate scale
        var scale = this.scaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
        this.arrowNodes[i].localScale = new Vector3(scale, scale,1f);

    }
    // Set the rotation of the first arrow node to match the second arrow node
    this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
    }
}

