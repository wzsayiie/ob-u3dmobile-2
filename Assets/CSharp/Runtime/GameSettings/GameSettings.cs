using UnityEngine;

namespace U3DMobile
{
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private string _flavor;

        public string flavor
        {
            get { return _flavor ; }
            set { _flavor = value; }
        }
    }
}
