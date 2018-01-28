using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    class PlayerControllerSettings : MonoBehaviour
    {

        public void ChangePlayerOne(bool value)
        {
            Debug.Log("P1 : Key : " + !value);
            PlayerPrefs.SetInt("P1_Keyboard",value?0:1);
        }
        public void ChangePlayerTwo(bool value) {
            Debug.Log("P2 : Key : " + !value);

            PlayerPrefs.SetInt("P2_Keyboard", value ? 0:1);
        }
    }
}
