using UnityEngine;

public class PrintPersistentPath : MonoBehaviour
{
	void Start()
	{
		Debug.Log("persistentDataPath = " + Application.persistentDataPath);
	}
}
