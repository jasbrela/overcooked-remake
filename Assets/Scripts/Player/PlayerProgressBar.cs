using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerProgressBar : MonoBehaviour
    {
        [SerializeField] private float refreshRate = 0.1f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator StartProgress()
        {
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
