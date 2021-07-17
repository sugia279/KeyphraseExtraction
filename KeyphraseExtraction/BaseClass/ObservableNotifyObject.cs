using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KeyphraseExtraction.BaseClass
{
    /// <summary>
    /// This is the abstract base class for any object 
    /// that provides property change notifications. 
    /// </summary>
    public abstract class ObservableNotifyObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Constructor
        /// </summary>
        protected ObservableNotifyObject()
        {

        }
        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        /// <summary>
        /// Get name of property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        /// <summary>
        /// Raise when property value propertychanged or override propertychage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(GetPropertyName(propertyExpression));
        }
    }
}
