using Blaster.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Target
{
    public class BlockView : TargetView
    {
        
        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}