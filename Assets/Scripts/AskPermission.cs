using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class AskPermission : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] permissions = new string[]
    {
        Permission.ExternalStorageRead,
        Permission.ExternalStorageWrite
    };

        foreach (string permission in permissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
    }

}
