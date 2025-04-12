using UnityEngine;

namespace slc.NIGHTSWIM
{
    public class FishMovement : MonoBehaviour
    {
        public GameObject monster;
        public GameObject player;
        public float monsterSpeed = 7;

        public bool isMonsterAwoken = false;
        bool MonsterHasBeenAwoken = false;
        // Start is called before the first frame update
        void Start()
        {
            monster.SetActive(false);
            //monster.transform.position = new Vector3(player.transform.position.x, -1450, player.transform.position.z - 2000);
        }

        // Update is called once per frame
        void Update()
        {
            if(isMonsterAwoken)
            {
                ActivateMonster();
                monster.SetActive(true);
                monster.transform.position += Vector3.forward * monsterSpeed;


            }

        }

        public void ActivateMonster()
        {
            
            if (!MonsterHasBeenAwoken)
            {
                monster.SetActive(true);
                //monster.transform.position = player.transform.position - new Vector3(0, 0, -100);
                //monster.transform.rotation = player.transform.rotation;
                monster.transform.position = new Vector3(player.transform.position.x, -1450, player.transform.position.z - 1000);
            }
            MonsterHasBeenAwoken = true;
            
        }
    }
}
