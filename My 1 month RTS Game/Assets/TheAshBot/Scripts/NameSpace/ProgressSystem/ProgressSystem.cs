using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheAshBot.ProgressBarSystem
{
    public class ProgressSystem
    {


        public event EventHandler OnProgressFinished;
        public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
        public class OnProgressChangedEventArgs : EventArgs
        {
            public int value;
            public int amount;
        }



        private bool isCountingUp;
        private int maxProgress;
        private int progress;

        /// <summary>
        /// This will create the Pregress system
        /// </summary>
        /// <param name="maxProgress">This is the maxium amout of progress</param>
        /// <param name="isCountingUp">Thid determains if the progress bar gose from 0 to Max progress, or Max progress to 0</param>
        public ProgressSystem(int maxProgress, bool isCountingUp = true)
        {
            this.isCountingUp = isCountingUp;
            this.maxProgress = maxProgress;
            progress = isCountingUp ? 0 : maxProgress;
        }


        #region Max Progress

        public int GetMaxProgress()
        {
            return maxProgress;
        }

        public void AddMaxProgress(int amount)
        {
            maxProgress += amount;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                value = progress,
                amount = 0,
            });
        }

        public void SetMaxProgress(int maxProgress)
        {
            this.maxProgress = maxProgress;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                value = progress,
                amount = 0,
            });
        }

        #endregion


        #region Progress

        public int GetProgress()
        {
            return progress;
        }

        public float GetProgressPercent()
        {
            return (float)progress / maxProgress;
        }

        public void SetProgress(int progress)
        {
            this.progress = progress;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                value = progress,
                amount = 0,
            });
        }

        public void AddProgress(int progress)
        {
            this.progress += progress;

            // At start progress
            if (isCountingUp ? this.progress <= 0 : this.progress >= maxProgress)
            {
                this.progress = isCountingUp ? 0 : maxProgress;
            }

            // At end progress
            if (isCountingUp ? this.progress >= maxProgress : this.progress <= 0)
            {
                this.progress = isCountingUp ? maxProgress : 0;
                OnProgressFinished?.Invoke(this, EventArgs.Empty);
            }

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                value = progress,
                amount = progress,
            });
        }

        public void SetProgressToMaxProgress()
        {
            progress = maxProgress;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                value = progress,
                amount = 0,
            });
        }

        #endregion


        #region Is Counting Up

        private bool GetIsCountingUp()
        {
            return isCountingUp;
        }

        private void SetIsCountingUp(bool isCountingUp)
        {
            this.isCountingUp = isCountingUp;
        }

        #endregion


    }
}
