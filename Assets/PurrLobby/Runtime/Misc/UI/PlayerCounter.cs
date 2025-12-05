using UnityEngine;

namespace PurrLobby
{
    public class PlayerCounter : MonoBehaviour
    {
        public int playerCount = 0;

        public void Increment()
        {
            playerCount++;
        }

        public void Decrement()
        {
            playerCount--;
        }
    }
}
