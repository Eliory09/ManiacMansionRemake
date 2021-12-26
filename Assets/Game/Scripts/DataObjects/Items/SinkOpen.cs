using UnityEngine;

public class SinkOpen : MonoBehaviour
{
    #region MonoBehaviour

    /// <summary>
        ///   Handles activation of sink/faucet action of open/close.
        /// </summary>
        public void Activate()
        {
            if (GameManager.GetCommand() == Command.Open)
                gameObject.SetActive(true);
            else if (GameManager.GetCommand() == Command.Close)
                gameObject.SetActive(false);
        }

    #endregion
    
}
