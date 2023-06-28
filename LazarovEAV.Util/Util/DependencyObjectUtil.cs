using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LazarovEAV.Util.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyObjectUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="dpName"></param>
        /// <returns></returns>
        public static DependencyProperty GetDependencyPropertyByName(DependencyObject dependencyObject, string dpName)
        {
            return GetDependencyPropertyByName(dependencyObject.GetType(), dpName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyObjectType"></param>
        /// <param name="dpName"></param>
        /// <returns></returns>
        public static DependencyProperty GetDependencyPropertyByName(Type dependencyObjectType, string dpName)
        {
            DependencyProperty dp = null;

            var fieldInfo = dependencyObjectType.GetField(dpName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (fieldInfo != null)
            {
                dp = fieldInfo.GetValue(null) as DependencyProperty;
            }

            return dp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="source"></param>
        /// <param name="mode"></param>
        public static void BindByName(DependencyObject target, string propertyName, object source, BindingMode mode = BindingMode.TwoWay)
        {
            DependencyProperty prop = DependencyObjectUtil.GetDependencyPropertyByName(target, propertyName + "Property");

            if (prop != null)
                BindingOperations.SetBinding(target, prop, new Binding(propertyName) { Source = source, Mode = mode });
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="dpName"></param>
        public static void SetValueByName(object target, string propertyName, object value)
        {
            if (target == null)
            {
                Debug.Print("SetValueByName(): Target object is null, target property name is " + propertyName + "\r\n");
                return;
            }

            if (!(target is DependencyObject))
            {
                Debug.Print("SetValueByName(): Target object is not a DependencyObject, target property name is " + propertyName + "\r\n");
                SetValueByPOCOPropertyName(target, propertyName, value);
                return;
            }

            DependencyProperty prop = DependencyObjectUtil.GetDependencyPropertyByName((DependencyObject)target, propertyName + "Property");

            if (prop != null)
            {
                ((DependencyObject)target).SetValue(prop, value);
            }
            else
            {
                Debug.Print("SetValueByName(): Property " + propertyName + " not found on object " + target.ToString() + "\r\n");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="dpName"></param>
        public static object GetValueByName(object target, string propertyName)
        {
            if (target == null)
            {
                Debug.Print("GetValueByName(): Target object is null, target property name is " + propertyName + "\r\n");
                return null;
            }

            if (!(target is DependencyObject))
            {
                Debug.Print("GetValueByName(): Target object is not a DependencyObject, target property name is " + propertyName + "\r\n");               
                return GetValueByPOCOPropertyName(target, propertyName);
            }

            DependencyProperty prop = DependencyObjectUtil.GetDependencyPropertyByName((DependencyObject)target, propertyName + "Property");

            if (prop != null)
                return ((DependencyObject)target).GetValue(prop);


            Debug.Print("GetValueByName(): Property " + propertyName + " not found on object " + target.ToString() + "\r\n");
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetValueByPOCOPropertyName(object target, string propertyName)
        {
            if (target == null)
            {
                Debug.Print("GetValueByPOCOPropertyName(): Target object is null, target property name is " + propertyName + "\r\n");
                return null;
            }


            PropertyInfo propInfo = target.GetType().GetProperty(propertyName);

            if (propInfo == null)
            {
                Debug.Print("GetValueByPOCOPropertyName(): Target object does not have property with name " + propertyName + "\r\n");
                return null;
            }

            return propInfo.GetValue(target);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        public static void SetValueByPOCOPropertyName(object target, string propertyName, object value)
        {
            if (target == null)
            {
                Debug.Print("SetValueByPOCOPropertyName(): Target object is null, target property name is " + propertyName + "\r\n");
                return;
            }


            PropertyInfo propInfo = target.GetType().GetProperty(propertyName);

            if (propInfo == null)
            {
                Debug.Print("SetValueByPOCOPropertyName(): Target object does not have property with name " + propertyName + "\r\n");
                return;
            }

            propInfo.SetValue(target, value);
        }
    }
}
