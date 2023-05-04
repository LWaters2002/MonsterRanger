using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUI
{
    public class PlayerUI : UI
    {
        private PlayerCharacter _playerCharacter;

        public virtual void Init(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
        }
    }
}   
