using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Match3.Scripts.Core;
using System;
using UnityEngine.Animations;
using UnityEngine;


namespace Match3.Scripts.Chips
{
    public class ChipBehaviour : MonoBehaviour
    {
        private Chip chip;
        private Animator animator;
        public GameObject explosion;
        private GameObject activeExpl;
        public bool IsPlayedAnim;
        public void PlayAnim()
        {
            animator = this.GetComponent<Animator>();
            chip = this.GetComponent<Chip>();
            switch(chip.State)
            {
                case ChipState.Explosion:
                    ExplosionAnim();
                break;

                case ChipState.Activate:
                    ActivateAnim();
                break;

                case ChipState.Destroy:
                    DestroyAnim();
                break;

                case ChipState.Appear:
                    AppearAnim();
                break;

                case ChipState.BonusAppear:
                    BonusAppearAnim();
                break;

                case ChipState.Tip:
                    TipAnim();
                break;
            }
        }

        private void AppearAnim()
        {
            animator.SetBool("Appear", true);
            IsPlayedAnim = true;
        }

        private void BonusAppearAnim()
        {
            if (chip.nextType == ChipType.None && chip.nextBonusType == ChipBonusType.None)
                return;
            chip.ChipType = chip.nextType;
            chip.ChipBonusType = chip.nextBonusType;
            chip.nextType = ChipType.None;
            chip.nextBonusType = ChipBonusType.None;
            chip.Initialize();
            animator.SetBool("Bonus_Appear", true);
            IsPlayedAnim = true;
        }

        private void ExplosionAnim()
        {
            animator.SetBool("Explosion", true);
            activeExpl = Instantiate(explosion, transform.position, Quaternion.identity);
            activeExpl.transform.SetParent(GameField.Instance.Effects.transform);
            IsPlayedAnim = true;
        }

        private void DestroyAnim()
        {
            animator.SetBool("Destroy", true);
            IsPlayedAnim = true;
        }

        private void ActivateAnim()
        {
            animator.SetBool("Activate", true);
            if (chip.activateByExplosion)
            {
                GameField.Instance.ActivateBonus(chip);
            }
            IsPlayedAnim = true;
        }

        private void TipAnim()
        {
            animator.SetTrigger("Tip");
            chip.State = ChipState.Idle;
        }

        private void  SetAppeared()
        {
            animator.SetBool("Appear", false);
            chip.State = ChipState.Idle;
            IsPlayedAnim = false;
        }

        private void SetBonusAppeared()
        {
            animator.SetBool("Bonus_Appear", false);
            chip.State = ChipState.Idle;
            IsPlayedAnim = false;
        }

        private void  SetDestroyed()
        {
            animator.SetBool("Destroy", false);
            chip.State = ChipState.Destroyed;
            IsPlayedAnim = false;
        }

        private void  SetExploded()
        {
            animator.SetBool("Explosion", false);
            chip.State = ChipState.Destroyed;
            IsPlayedAnim = false;
        }

        private void  SetActivated()
        {
            animator.SetBool("Activate", false);
            //chip.State = ChipState.Destroyed;
            IsPlayedAnim = false;
        }

        private void  SetTipped()
        {

        }
        
        
    }
}
