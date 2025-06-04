using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalcollidersound : MonoBehaviour
{
    public AudioSource woodworkhit;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            if (!MenuController.ChallengeMode)
            {
                GameController.celebration.SetBool("miss", true);
                GameController.celebration2.SetBool("miss", true);
                GameController.DefenderAnim.SetBool("saved", true);
                GameController.Opponent2.transform.rotation = Quaternion.Euler(0, 240, 0);
            }
            else
            {

            }
            if (!woodworkhit.isPlaying)
            {
                if (MenuSFX.sfx == null)
                {

                }
                else if (MenuSFX.sfx != null)
                {
                    if (MenuSFX.sfx.activeInHierarchy == false)
                    {

                    }
                    else
                    {
                        woodworkhit.Play();
                    }
                }

                if (!MenuController.ChallengeMode)
                {
                    if (!GameController.crowdmiss.isPlaying)
                    {
                        if (GameController.crowdmiss.gameObject.activeInHierarchy == true)
                        {
                            GameController.crowdmiss.Play();
                        }
                        else
                        {

                        }
                    }
                }
                else
                {

                }
            }
        }
    }
}
