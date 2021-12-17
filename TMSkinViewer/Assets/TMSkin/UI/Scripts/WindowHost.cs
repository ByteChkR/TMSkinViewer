using UnityEngine;

namespace UI
{

    public class WindowHost : MonoBehaviour
    {

        private void Awake()
        {
            Window.DefaultHost = transform;
        }

    }

}
