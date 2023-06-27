using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    private static readonly object lockObject = new object();
    private static Dictionary<string, object> variablesDictionary = new Dictionary<string, object>();
    public static string serverAddress = "3.15.7.16";
    //public static string serverAddress = "http://localhost:80";

}
