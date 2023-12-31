﻿#pragma checksum "..\..\..\UI\SubstanceSelector.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EFFEC4D6F31F8B06C17616ECA20272D410114C8564504F518779D73C219B0AD4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Aga.Controls.Tree;
using Itenso.Windows.Controls.ListViewLayout;
using LazarovEAV.Config;
using LazarovEAV.Model;
using LazarovEAV.UI;
using LazarovEAV.UI.Util;
using LazarovEAV.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace LazarovEAV.UI {
    
    
    /// <summary>
    /// SubstanceSelector
    /// </summary>
    public partial class SubstanceSelector : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 66 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Aga.Controls.Tree.TreeList substancesTree;
        
        #line default
        #line hidden
        
        
        #line 170 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeButton;
        
        #line default
        #line hidden
        
        
        #line 205 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox substanceName;
        
        #line default
        #line hidden
        
        
        #line 219 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox substanceDescription;
        
        #line default
        #line hidden
        
        
        #line 230 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox substanceType;
        
        #line default
        #line hidden
        
        
        #line 250 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox substancePotency;
        
        #line default
        #line hidden
        
        
        #line 257 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox substancePotencyCombo;
        
        #line default
        #line hidden
        
        
        #line 291 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox substanceListButton;
        
        #line default
        #line hidden
        
        
        #line 307 "..\..\..\UI\SubstanceSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button applyButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LazarovEAV;component/ui/substanceselector.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UI\SubstanceSelector.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.substancesTree = ((Aga.Controls.Tree.TreeList)(target));
            
            #line 78 "..\..\..\UI\SubstanceSelector.xaml"
            this.substancesTree.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.substancesTree_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.closeButton = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.substanceName = ((System.Windows.Controls.ComboBox)(target));
            
            #line 213 "..\..\..\UI\SubstanceSelector.xaml"
            this.substanceName.KeyUp += new System.Windows.Input.KeyEventHandler(this.comboBox_KeyUp);
            
            #line default
            #line hidden
            
            #line 214 "..\..\..\UI\SubstanceSelector.xaml"
            this.substanceName.DropDownClosed += new System.EventHandler(this.substanceName_DropDownClosed);
            
            #line default
            #line hidden
            return;
            case 5:
            this.substanceDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.substanceType = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.substancePotency = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.substancePotencyCombo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.substanceListButton = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 10:
            this.applyButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 2:
            
            #line 124 "..\..\..\UI\SubstanceSelector.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.substanceListItem_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

