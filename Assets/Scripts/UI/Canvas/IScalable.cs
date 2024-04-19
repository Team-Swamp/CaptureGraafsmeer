using System.Collections;
using UnityEngine;

namespace UI.Canvas
{
    public interface IScalable
    {
        IEnumerator AnimateScale(Vector3 targetScale);
    }
}