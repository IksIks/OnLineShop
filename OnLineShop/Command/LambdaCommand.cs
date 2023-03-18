using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnLineShop.Command
{
    internal class LambdaCommand : CommandBase
    {

        private Action<object> execute;
        private Func<object, bool> canExecute;


        public LambdaCommand(Action<object> execute, Func<object, bool> canExecuted = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecuted;
        }
        public override bool CanExecute(object parameter)
        {
           return canExecute?.Invoke(parameter) ?? true;
        }

        public override void Execute(object parameter) => execute(parameter);
    }
}
