using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TrivialCLR.Example
{
    public abstract class SnakeGameBase : MonoBehaviour
    {
        // Properties
        public int GameWidth { get { return 32; } }

        public int GameHeight { get { return 18; } }
    }
}
