using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    [DefaultExecutionOrder(int.MinValue)]
    public class BootstrapSceneRunner : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper != null)
            {
                return;
            }

            SceneManager.LoadScene(0);
        }
#endif
    }
}