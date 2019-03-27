using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class ScreenLoading : GUIScreen
    {
        protected override void OnShow()
        {
            GameManager.Instance.Restart();
        }
    }
}
