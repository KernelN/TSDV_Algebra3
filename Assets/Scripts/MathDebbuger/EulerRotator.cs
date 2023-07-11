using UnityEngine;

namespace CustomMath.Testers
{
    public class EulerRotator : MonoBehaviour
    {
        [SerializeField] QuatTester.Q q;
        [SerializeField] bool useQuat;
        
        // Update is called once per frame
        void Update()
        {
            if (useQuat)
                transform.rotation = Quaternion.Euler(((Quat)q).eulerAngles);
            else
                transform.rotation = Quaternion.Euler(((Quaternion)q).eulerAngles);
        }
    }
}