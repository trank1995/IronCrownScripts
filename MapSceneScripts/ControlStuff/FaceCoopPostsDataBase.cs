using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCoopPostsDataBase : MonoBehaviour
{
    public static FaceCoopPostsDataBase dataBase;
    public List<Texture2D> warMartPostImgs;
    public List<Texture2D> defaultpostImgs;
    

    // Use this for initialization
    void Awake()
    {
        dataBase = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public FaceCoopPost getFactionVictoryPost(Faction won, Faction lost)
    {
        return new FaceCoopPost("We are awesome", won.ToString(), "we are NO.1", defaultpostImgs[0]);
    }

}

public class FaceCoopPost
{
    public string postTitle, poster, postContent;
    public Texture2D postImg;
    public FaceCoopPost(string postTitleI, string posterI, string postContentI, Texture2D postImgI)
    {
        postTitle = postTitleI;
        poster = posterI;
        postContent = postContentI;
        postImg = postImgI;
    }

}