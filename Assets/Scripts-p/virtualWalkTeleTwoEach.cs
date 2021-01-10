using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class virtualWalkTeleTwoEach : MonoBehaviour
{
    float timeLeft;//just for test
    int[,] densityWidth;
    int densityIndex;
    int widthIndex;
    int numPresent; //number of active objects(cubes) in scene
    System.Random rand;
    int[] widths;
    int[] densities;
    Vector3 rb;
    Vector3 rt;
    Vector3 lt;
    Vector3 lb;

    GameObject cube;
    List<Vector3> targets;

    AudioSource correctAudioData;

    AudioSource errorAudioData;


    //file related
    StreamWriter outStream;
    string filePath;
    StringBuilder sb;
    string localDateTime;
    Stopwatch stopwatch;

    int setCompleted;
    int currentDensity;
    int currentWidth;
    float completionTime;
    int errorCount;

    //List<string> objectSide;
    //List<string> handUsed;

    string objectSideStr;
    string handUsedStr;



    void Start()
    {

        correctAudioData = GameObject.Find("correctAudio").GetComponent<AudioSource>();

        errorAudioData = GameObject.Find("errorAudio").GetComponent<AudioSource>();

        // file
        localDateTime = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();
        sb = new StringBuilder();
        completionTime = 0;
        errorCount = 0;
        stopwatch = new Stopwatch();
        Directory.CreateDirectory("./file");
        filePath = "./file/" + "virtualWalkTeleTwoEach" + localDateTime + ".csv";
        outStream = System.IO.File.CreateText(filePath);


        sb.AppendLine("condition" + ',' + "objectSide" + ',' + "handUsed" + ',' + "blockNumber" + ',' + "density" + ',' + "width(cm)" + ',' + "completionTime(ms)" + ',' + "errorCount");
        print(sb.ToString());
        outStream.WriteLine(sb);
        sb.Length = 0;

        //

        setCompleted = 0;

        densityWidth = new int[2, 3];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                densityWidth[i, j] = 1;
            }
        }


        cube = GameObject.Find("object-sample");
        targets = new List<Vector3>();
        densities = new int[2] { 10, 20 };
        widths = new int[3] { 1, 2, 3 };


        //x left 3 o 6
        //x right 11 o 14 
        //y 1 o 5
        //z -5.5 2.5

        rb = new Vector3(1, 0, (float)-0);
        rt = new Vector3(1, 0, (float)6);
        lt = new Vector3(-1, 0, (float)6);
        lb = new Vector3(-1, 0, (float)-0);


        //Shuffle(widths);
        //Shuffle(densities);

        rand = new System.Random();

        generateRandom();

        //Instantiate(cube, new Vector3(8, 0, 0), Quaternion.identity);
        //Instantiate(cube, new Vector3(3, 0, 0), Quaternion.identity);

        //Debug.Log("gfsv");
    }


    void Update()
    {

        var input = Input.inputString;
        switch (input)
        {
            case "z":
                SceneManager.LoadScene("c1Z-v2-withoutWalkOne");
                break;
            case "x":
                SceneManager.LoadScene("c2X-v2-withoutWalkTwo");
                break;
            case "c":
                SceneManager.LoadScene("c3C-v2-actualWalkOne");
                break;
            case "v":
                SceneManager.LoadScene("c4V-v2-actualWalkTwo");
                break;
            case "b":
                SceneManager.LoadScene("c5B-v2-virtualWalkTeleOne");
                break;
            case "n":
                SceneManager.LoadScene("c6N-v2-virtualWalkTeleTwoBoth");
                break;
            case "m":
                SceneManager.LoadScene("c7M-v2-virtualWalkTeleTwoEach");
                break;
        }


        if (numPresent == 0)
        {
            setCompleted++;
            completionTime = stopwatch.ElapsedMilliseconds; ;
            file();
            generateRandom();
        }

        //timeLeft -= Time.deltaTime;
        //if (timeLeft < 0)
        //{
        //    timeLeft = 10;
        //    setCompleted++;
        //    generateRandom();
        //}
    }


    public void file()
    {

        sb.AppendLine("virtualWalkTeleTwoEach" + ',' + objectSideStr + ',' + handUsedStr + ',' + setCompleted + ',' + currentDensity + ',' + currentWidth * 10 + ',' + completionTime + ',' + errorCount);

        outStream.WriteLine(sb);
        sb.Length = 0;

    }

    public void OnSelectRight(Zinnia.Pointer.ObjectPointer.EventData data)
    {

        GameObject g;
        if (data != null)
        {
            g = data.CollisionData.transform.gameObject;

            handUsedStr += "right;";
            if (g.transform.position.x > 0.5)
            {
                objectSideStr += "right;";
            }
            else
            {
                objectSideStr += "left;";
            }

            g.SetActive(false);
            numPresent = numPresent - 1;
            playCorrectAudio();

        }
        else
        {
            errorCount++;
            playErrorAudio();
        }
    }


    public void OnSelectLeft(Zinnia.Pointer.ObjectPointer.EventData data)
    {

        GameObject g;
        if (data != null)
        {
            g = data.CollisionData.transform.gameObject;

            handUsedStr += "left;";
            if (g.transform.position.x > 0.5)
            {
                objectSideStr += "right;";
            }
            else
            {
                objectSideStr += "left;";
            }

            g.SetActive(false);
            numPresent = numPresent - 1;
            playCorrectAudio();

        }
        else
        {
            errorCount++;
            playErrorAudio();
        }
    }


    void playCorrectAudio()
    {
        correctAudioData.Play(0);
    }

    void playErrorAudio()
    {
        errorAudioData.Play(0);
    }




    public void generateRandom()
    {

        print(setCompleted);

        if (setCompleted >= 6)
        {
            print("finished");
            outStream.Close();
            quit();
        }
        else
        {

            while (densityWidth[densityIndex, widthIndex] == 0)
            {
                densityIndex = UnityEngine.Random.Range(0, 2);


                widthIndex = UnityEngine.Random.Range(0, 3);
            }

            densityWidth[densityIndex, widthIndex] = 0;

            generateCubes(densityIndex, widthIndex);

        }

    }


    public void generateCubes(int i, int j)
    {
        objectSideStr = "";
        handUsedStr = "";

        for (int num = 0; num < densities[i] / 2; num++)
        { // the total number of objects we need on the right side
            bool found = false;
            Vector3 newCube = new Vector3();
            while (!found)
            {
                double x = rand.NextDouble() * (3 - rb.x) + (rb.x);
                double y = rand.NextDouble() * (2) + 0.5; // must check in the environment 
                double z = rand.NextDouble() * (rt.z - rb.z) + (rb.z);

                newCube = new Vector3((float)x, (float)y, (float)z);

                found = !collisionDetector(newCube, (float)(widths[j] * 0.05));
            }

            GameObject go = Instantiate(cube, newCube, Quaternion.identity);
            go.transform.localScale = new Vector3((float)(widths[j] * 0.1), (float)(widths[j] * 0.1), (float)(widths[j] * 0.1));
            go.GetComponent<Renderer>().material.color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            targets.Add(newCube);

        }

        for (int num = 0; num < densities[i] / 2; num++)
        { // the total number of objects we need on the left side
            bool found = false;
            Vector3 newCube = new Vector3();
            while (!found)
            {
                double x = rand.NextDouble() * (lb.x - (-3)) + (-3);
                double y = rand.NextDouble() * (2) + 0.5; // must check in the environment
                double z = rand.NextDouble() * (lt.z - lb.z) + (lb.z);

                newCube = new Vector3((float)x, (float)y, (float)z);

                found = !collisionDetector(newCube, (float)(widths[j] * 0.05));
            }

            GameObject go = Instantiate(cube, newCube, Quaternion.identity);
            go.transform.localScale = new Vector3((float)(widths[j] * 0.1), (float)(widths[j] * 0.1), (float)(widths[j] * 0.1));
            go.GetComponent<Renderer>().material.color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            targets.Add(newCube);
        }


        numPresent = densities[i];
        currentDensity = densities[i];
        currentWidth = widths[j];
        completionTime = 0;
        errorCount = 0;
        stopwatch.Reset();
        stopwatch.Start();

    }

    //public void Shuffle(int[] array)
    //{
    //    int tempGO;
    //    for (int i = 0; i < array.Length; i++)
    //    {
    //        int rnd = UnityEngine.Random.Range(0, array.Length);
    //        tempGO = array[rnd];
    //        array[rnd] = array[i];
    //        array[i] = tempGO;
    //    }
    //}

    public bool collisionDetector(Vector3 newTarget, float realWidth)
    {

        for (int i = 0; i < targets.Count; i++)
        {
            if (Vector3.Distance(targets[i], newTarget) <= realWidth)
                return true;
        }
        return false;
    }

    public void quit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
