//------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="CyberFeedForward" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
[module: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1638:FileHeaderFileNameDocumentationMustMatchFileName",
    Justification = "Bug in Style Cop.")]

namespace CyberFeedForward.WUP.Common.WPF
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Implementation of ICommand for use with MVVM WPF applications.
    /// </summary>
    /// <typeparam name="TCommandParameter">WPF argument</typeparam>
    public class RelayCommand<TCommandParameter> : ICommand
    {
        /// <summary>
        /// Command to execute
        /// </summary>
        /// <typeparam name="TCommandParameter">Action parameter</typeparam>
        private readonly Action<TCommandParameter> execute;

        /// <summary>
        /// Return true if execute action available, false otherwise
        /// </summary>
        /// <typeparam name="TCommandParameter">Action parameter</typeparam>
        /// <typeparam name="bool">return parameter</typeparam>
        private readonly Func<TCommandParameter, bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{TCommandParameter}" /> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<TCommandParameter> execute, Func<TCommandParameter, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Raised when RaiseCanExecuteChanged is called.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Determines whether this <see cref="RelayCommand"/> can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter = null)
        {
            execute((TCommandParameter)parameter);
        }

        /// <summary>
        /// Called by WPF. Executes the <see cref="RelayCommand"/> on the current command target.
        /// </summary>
        /// <param name="parameter">Data used by the command</param>
        /// <returns>True if control should be enabled</returns>
        public bool CanExecute(object parameter = null)
        {
            return canExecute == null ? true : canExecute((TCommandParameter)parameter);
        }

        /// <summary>
        /// Method used to raise the <see cref="CanExecuteChanged"/> event
        /// to indicate that the return value of the <see cref="CanExecute"/>
        /// method has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}