﻿#pragma checksum "..\..\SendMessage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "417DB4ED37982428FFF4B7ED4D6112925BFA09F497325DEE733B0BE936CB56D4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Coursework2;
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


namespace Coursework2 {
    
    
    /// <summary>
    /// SendMessage
    /// </summary>
    public partial class SendMessage : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\SendMessage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSendMessage;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\SendMessage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClear;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\SendMessage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblMessage;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\SendMessage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxMessage;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\SendMessage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblSubject;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\SendMessage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxSubject;
        
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
            System.Uri resourceLocater = new System.Uri("/Coursework2;component/sendmessage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SendMessage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.btnSendMessage = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\SendMessage.xaml"
            this.btnSendMessage.Click += new System.Windows.RoutedEventHandler(this.Button_Send_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnClear = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\SendMessage.xaml"
            this.btnClear.Click += new System.Windows.RoutedEventHandler(this.Button_Clear_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lblMessage = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.txtBoxMessage = ((System.Windows.Controls.TextBox)(target));
            
            #line 13 "..\..\SendMessage.xaml"
            this.txtBoxMessage.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBoxMessage_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lblSubject = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.txtBoxSubject = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\SendMessage.xaml"
            this.txtBoxSubject.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBoxSubject_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

