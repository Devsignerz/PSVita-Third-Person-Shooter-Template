using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GhostType{
    Oikake,
    Machibuse,
    Kimagure,
    Otoboke
}

public class GhostTargetFinder : MonoBehaviour{
    public GhostStateSwitcher stateSwitcher;
    public GhostType gtype = GhostType.Oikake;
    public Transform player;
    public Transform oikakeForKimagure;
    public Vector3 scatterTarget = new Vector3(24, 1, 35);
    public Vector3 eatenTarget = new Vector3(0, 0, 8);


    Vector3 GhostTypeTarget(){
        switch (gtype){
            case GhostType.Oikake:
                return player.position;

            case GhostType.Machibuse:
                return player.position + (player.forward * 2 * 3);

            case GhostType.Kimagure:
                Vector3 PFP = player.position + (player.forward * 2 * 3);
                return PFP + (PFP - oikakeForKimagure.position);

            case GhostType.Otoboke:
                return Vector3.Distance(transform.position, player.position) < 10 ? scatterTarget : player.position;

            default:
                return transform.position;
        }
    }

    public Vector3 Target(){
        switch (stateSwitcher.state){
            case GhostState.Scatter:
                return scatterTarget;

            case GhostState.Chase:
                return GhostTypeTarget();

            case GhostState.Frigntened:
                return transform.position;

            case GhostState.Eaten:
                return eatenTarget;

            default:
                return transform.position;
        }
    }
}
