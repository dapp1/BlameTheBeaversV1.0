using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class Initialization : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}