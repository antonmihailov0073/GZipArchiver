using System.Threading;

namespace VeeamTest.Threading
{
    public class SpinBlock
    {
        private const int DefaultSpinTimesBeforeBlock = 20;
        
        
        private readonly object _sync = new object();
        
        private readonly int _spinTimesBeforeBlock;

        
        public SpinBlock(int spinTimesBeforeBlock = DefaultSpinTimesBeforeBlock)
        {
            _spinTimesBeforeBlock = spinTimesBeforeBlock;
        }


        public void Enter()
        {
            var spinner = new SpinWait();
            
            // spin [_spinTimesBeforeBlock] times trying to enter
            var isMonitorEntered = false;
            for (var i = 0; i < _spinTimesBeforeBlock; ++i)
            {
                isMonitorEntered = Monitor.TryEnter(_sync);
                if (isMonitorEntered)
                {
                    break;
                }
                spinner.SpinOnce();
            }

            // if spin enter was failed, block for it
            if (!isMonitorEntered)
            {
                Monitor.Enter(_sync);
            }
        }

        public void Exit()
        {
            Monitor.Exit(_sync);
        }
    }
}