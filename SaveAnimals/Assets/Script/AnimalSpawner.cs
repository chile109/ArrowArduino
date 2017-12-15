using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour {

    public float timer = 0;

    public int max_animal = 5;

    public int NowCount = 0;
	
	// Update is called once per frame
	void Update () {
        
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 2.0f;

            if (NowCount >= max_animal)
                return;

            GameObject AniPrefab = (GameObject)Resources.Load("Animal1");

            float cameraZ = Camera.main.transform.position.z;

            Vector3 InitPos = Camera.main.WorldToViewportPoint(AniPrefab.transform.position);
            Vector3 RandomPos = new Vector3(Random.value, InitPos.y, -cameraZ);

            RandomPos = Camera.main.ViewportToWorldPoint(RandomPos);

            Target _target = Random.value > 0.5f ? Target.Right : Target.Left;

            AnimalController animal = AnimalController.Create(AniPrefab, _target, RandomPos);

            NowCount++;
            animal.OnArrested += whenCapture;                                             

        }
	}

    void whenCapture(AnimalController animal)
    {
        NowCount--;
    }
}
