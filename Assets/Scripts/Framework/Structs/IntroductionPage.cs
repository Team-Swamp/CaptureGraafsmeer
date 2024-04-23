using System;
using UnityEngine;

namespace FrameWork.Structs
{
    [Serializable]
    public struct IntroductionPage
    {
        [SerializeField, TextArea(3, 6)] private string info;
        [SerializeField] private Texture2D image;

        public string Info() => info;

        public Texture2D Image() => image;
    }
}