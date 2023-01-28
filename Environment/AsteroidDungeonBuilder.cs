using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDungeonBuilder : MonoBehaviour
{
    [SerializeField] GameObject startZone;
    [SerializeField] GameObject endZone;
    [SerializeField] GameObject joint;
    [SerializeField] GameObject leg;
    
    [SerializeField] ColorPalette[] colorPallets;
    int paletteIndex;

    GameObject player;

    int level;
    int numJoints = 3;
    Tree msTree = new Tree();
    GameObject[] legs;
    GameObject[] joints;

    

    private void Awake()
    {
        level = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>().GetLevel();
        numJoints += level;

        player = GameObject.FindGameObjectWithTag("Player");
        
        msTree = MSTree.MakeTreeWithDegrees(numJoints);

        paletteIndex = Random.Range(0, colorPallets.Length);

        SetupLegs();

        joints = new GameObject[msTree.points.Count];
        SetupStartZone();
        SetupJoints();

        endZone = Instantiate(endZone, msTree.points[msTree.points.Count - 1].thisPoint, Quaternion.identity);
        endZone.transform.parent = transform;
        joints[joints.Length - 1] = endZone;

        
    }

    private void SetupJoints()
    {        
        for (int i = 1; i < msTree.points.Count - 1; i++)
        {
            GameObject jointClone = Instantiate(joint, msTree.points[i].thisPoint, Quaternion.identity);
            jointClone.transform.parent = transform;
            jointClone.GetComponent<AsteroidFieldJoint>().GetLastAndNextLeg(legs[i-1], legs[i]);
            joints[i] = jointClone;
        }
    }

    private void SetupStartZone()
    {
        startZone = Instantiate(startZone, msTree.points[0].thisPoint, Quaternion.identity);
        startZone.transform.parent = transform;

        Vector3 dir = msTree.points[1].thisPoint.normalized;

        // i'd like to move all this into this script but it wasnt working so i did it here
        //StartZone sz = startZone.GetComponent<StartZone>();
        //sz.SetUpVector(dir);


        // this all places our player at the surface on start,
        // we briefly parent out player to the startZone so we can easily inherit the rotation
        float startSurfaceYPos= 26f;
        player.transform.position = new Vector3(0, startSurfaceYPos, 0);

        player.transform.parent = startZone.transform;        

        Quaternion newUp = Quaternion.FromToRotation(Vector3.up, dir);
        startZone.transform.rotation = newUp;
        
        player.transform.parent = null;

        joints[0] = startZone;
    }

    private void SetupLegs()
    {
        legs = new GameObject[msTree.points.Count - 1];
        for (int i = 0; i < msTree.points.Count - 1; i++)
        {
            MakeLeg(i);
        }
        legs[0].SetActive(true);

    }

    private void MakeLeg(int i)
    {

        Vector3 dir = msTree.points[i].minimumEdge.otherPoint - msTree.points[i].thisPoint;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 lerpedCenter = Vector3.Lerp(msTree.points[i].thisPoint, msTree.points[i].minimumEdge.otherPoint, 0.5f);


        GameObject legGameObject = Instantiate(leg, lerpedCenter, rotation);
        legGameObject.transform.parent = transform.transform;

        legGameObject.GetComponent<ILevelLeg>().SetupLeg(msTree.points[i].minimumEdge.distance);

        legGameObject.SetActive(false);
        legs[i] = legGameObject;        
    }
  
}
