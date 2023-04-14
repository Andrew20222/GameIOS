using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using NaughtyAttributes;

namespace Yobzh.Platform.UI
{
    [AddComponentMenu("Yobzh Platform/UI/Components/UI Rotator")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class UIRotator : MonoBehaviour
    {
        #region Variables
        
        [SerializeField]
        private UIRotatorMode m_Mode = UIRotatorMode.Default;
        
        private bool IsRandomMode => m_Mode == UIRotatorMode.RandomBetweenTwoValues;
        
        [SerializeField] [ShowIf(nameof(m_WithStep))] [Range(0.001f, 1f)] 
        private float m_CycleTime = 0.5f;

        [SerializeField] [ShowIf(nameof(IsRandomMode))]
        private Vector2 m_ValuesForRandom = new Vector2(10, 20);
        
        [SerializeField] [HideIf(nameof(m_WithStep))] [Range(10f, 360f)]
        private float m_Speed = 10f;

        [SerializeField] [ShowIf(nameof(m_WithStep))] [Range(5f, 120f)]
        private float m_StepValue = 30f;
        
        [SerializeField] [Space]
        private bool m_WithStep;
        
        [SerializeField]
        private bool m_Invert;
        
        private float m_TargetSpeed;
        
        #endregion

        private void Start()
        {
            if (m_WithStep)
                StartCoroutine(RotateWithStep());
            else Configure();
        }

        private void Configure()
        {
            m_TargetSpeed = m_Mode == UIRotatorMode.Default ? m_Speed : 
                Random.Range(m_ValuesForRandom.x, m_ValuesForRandom.y);
        }

        private void Update()
        {
            if (!m_WithStep)
                transform.Rotate(0f, 0f, m_Invert ? 1f : -1f * Time.deltaTime * m_TargetSpeed);
        }

        private IEnumerator RotateWithStep()
        {
            while (true)
            {
                transform.Rotate(0f, 0f, transform.localRotation.z +
                                         (m_Invert ? 1f * m_StepValue : -1f * m_StepValue));
                
                yield return new WaitForSeconds(m_CycleTime);
            }
            
            // ReSharper disable once IteratorNeverReturns
        }
        
        #region Public Variables

        public UIRotatorMode WorkMode
        {
            get => m_Mode;
            set
            {
                m_Mode = value;
                Configure();
            }
        }

        public float Speed
        {
            get => m_Speed;
            set
            {
                m_Speed = value;
                Configure();
            }
        }

        public Vector2 ValuesForRandom
        {
            get => m_ValuesForRandom;
            set
            {
                m_ValuesForRandom = value;
                Configure();
            }
        }

        public float CycleTime
        {
            get => m_CycleTime;
            set => m_CycleTime = value;
        }

        public float StepValue
        {
            get => m_StepValue;
            set => m_StepValue = value;
        }

        public bool Invert
        {
            get => m_Invert;
            set => m_Invert = value;
        }
        #endregion
    }
}