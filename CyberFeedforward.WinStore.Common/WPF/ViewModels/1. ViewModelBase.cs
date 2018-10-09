//------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="CyberFeedForward" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Windows.Storage;

    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// Data Limit: https://msdn.microsoft.com/en-us/library/windows/apps/windows.storage.applicationdata.roamingsettings.aspx
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            propertyStore = new Dictionary<string, object>();
        }

        /// <summary>
        /// Backing store for properties
        /// </summary>
        private Dictionary<string, object> propertyStore;

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// For use with calculated properties. 
        /// Notify listeners that the specified property has changed. Only use in properties.
        /// </summary>
        /// <param name="calculatedPropertyName">Calculated property name</param>
        protected void NotifyPropertyUpdated(string calculatedPropertyName)
        {
            OnStateChanged(calculatedPropertyName);
        }

        /// <summary>
        /// Set property state.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="value">Desired value for the property</param>
        /// <param name="propertyName">Do not set</param>
        /// <returns>True if the value was changed, false otherwise</returns>
        protected bool SetState<T>(
            T value,
            SaveType saveType = SaveType.Application,
            [CallerMemberName] string propertyName = null)
        {
            switch (saveType)
            {
                case SaveType.Application:
                    if (propertyStore.ContainsKey(propertyName))
                    {
                        if (Equals(propertyStore[propertyName], value))
                        {
                            return false;
                        }
                        else
                        {
                            propertyStore[propertyName] = value;
                            OnStateChanged(propertyName);
                            return true;
                        }
                    }
                    else
                    {
                        propertyStore[propertyName] = value;
                        return true;
                    }

                case SaveType.RoamingSettings:
                    if (ApplicationData.Current.RoamingSettings.Values[propertyName] == null)
                    {
                        ApplicationData.Current.RoamingSettings.Values[propertyName] = value;
                        ApplicationData.Current.SignalDataChanged();
                        OnStateChanged(propertyName);
                        return true;
                    }
                    else if (Equals(ApplicationData.Current.RoamingSettings.Values[propertyName], value))
                    {
                        return false;
                    }
                    else
                    {
                        ApplicationData.Current.RoamingSettings.Values[propertyName] = value;
                        ApplicationData.Current.SignalDataChanged();
                        OnStateChanged(propertyName);
                        return true;
                    }

                case SaveType.LocalSettings:
                    if (ApplicationData.Current.LocalSettings.Values[propertyName] == null)
                    {
                        ApplicationData.Current.LocalSettings.Values[propertyName] = value;
                        ApplicationData.Current.SignalDataChanged();
                        OnStateChanged(propertyName);
                        return true;
                    }
                    else if (Equals(ApplicationData.Current.LocalSettings.Values[propertyName], value))
                    {
                        return false;
                    }
                    else
                    {
                        ApplicationData.Current.LocalSettings.Values[propertyName] = value;
                        ApplicationData.Current.SignalDataChanged();
                        OnStateChanged(propertyName);
                        return true;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Retrieve stored data
        /// </summary>
        /// <typeparam name="T">Data type to store</typeparam>
        /// <param name="initialValue">Initial value to set</param>
        /// <param name="saveType">Save type</param>
        /// <param name="propertyName">Leave blank.</param>
        /// <returns>Get stored value</returns>
        protected T GetState<T>(
        T initialValue = default(T),
            SaveType saveType = SaveType.Application,
            [CallerMemberName] string propertyName = null)
        {
            switch (saveType)
            {
                case SaveType.Application:
                    if (!propertyStore.ContainsKey(propertyName))
                    {
                        propertyStore[propertyName] = initialValue;
                    }

                    return (T)propertyStore[propertyName];

                case SaveType.RoamingSettings:
                    if (ApplicationData.Current.RoamingSettings.Values[propertyName] == null)
                    {
                        ApplicationData.Current.RoamingSettings.Values[propertyName] = initialValue;
                        ApplicationData.Current.SignalDataChanged();
                    }

                    return (T)ApplicationData.Current.RoamingSettings.Values[propertyName];

                case SaveType.LocalSettings:
                    if (ApplicationData.Current.LocalSettings.Values[propertyName] == null)
                    {
                        ApplicationData.Current.LocalSettings.Values[propertyName] = initialValue;
                        ApplicationData.Current.SignalDataChanged();
                    }

                    return (T)ApplicationData.Current.LocalSettings.Values[propertyName];

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get relay command for the specified action. Only use in get property.
        /// </summary>
        /// <param name="execute">The action</param>
        /// <param name="canExecute">Is enabled Function</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Relay command</returns>
        protected RelayCommand Command(Action execute, Func<bool> canExecute = null, [CallerMemberName] string propertyName = null)
        {
            if (!propertyStore.ContainsKey(propertyName))
            {
                propertyStore[propertyName] = new RelayCommand(execute, canExecute);
            }

            return propertyStore[propertyName] as RelayCommand;
        }

        /// <summary>
        /// Get relay command for the specified action. Only use in get property
        /// </summary>
        /// <typeparam name="TCommandParameter">WPF <code>CommandParameter</code></typeparam>
        /// <param name="execute">The action</param>
        /// <param name="canExecute">Is enabled Function</param>
        /// <param name="propertyName">Do not set</param>
        /// <returns>Relay command</returns>
        protected RelayCommand<TCommandParameter> Command<TCommandParameter>(
            Action<TCommandParameter> execute,
            Func<TCommandParameter, bool> canExecute = null,
            [CallerMemberName] string propertyName = null)
        {
            if (!propertyStore.ContainsKey(propertyName))
            {
                propertyStore[propertyName] = new RelayCommand<TCommandParameter>(execute, canExecute);
            }

            return (RelayCommand<TCommandParameter>)propertyStore[propertyName];
        }

        /// <summary>
        /// Notifies listeners that a property value has changed
        /// </summary>
        /// <param name="propertyName">Property whose name has been changed</param>
        private void OnStateChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// View model Storage type.
    /// </summary>
    public enum SaveType
    {
        /// <summary>
        /// Application storage storage for settings
        /// </summary>
        Application,

        /// <summary>
        /// Local persistant storage for settings
        /// </summary>
        LocalSettings,

        /// <summary>
        /// Roaming persistant storage for settings
        /// </summary>
        RoamingSettings
    }
}
