using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingParticleController : MonoBehaviour {

    public List<Transform> affector = new List<Transform>();
    public List<ParticleSystemRenderer> psrs = new List<ParticleSystemRenderer>();

	void Start () {

	}

	void Update () {
        for ( int i = 0; i < affector.Count; ++i) {
            psrs[i].material.SetVector("_Affector", affector[i].position);
        }
    }
}
