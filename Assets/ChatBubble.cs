using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var children = GetComponentsInChildren<SpriteRenderer>();
        foreach (var child in children)
        {
            child.transform.LookAt(player);
        }
    }

    public void ResetChatBubbles()
    {
        var children = GetComponentsInChildren<SpriteRenderer>();
        foreach (var child in children)
        {
            child.color = new Color( 1f, 1f, 1f, 1f);
        }
    }

    public void UpdateChatBubbles(bool isAuto)
    {
        var children = GetComponentsInChildren<SpriteRenderer>();
        foreach (var child in children)
        {
            if (isAuto)
                child.color = new Color( 1f, 1f, 1f, child.color.a - 0.25f);
            else
                child.color = new Color( 1f, 1f, 1f, child.color.a - 0.08f);
        }
    }
}
