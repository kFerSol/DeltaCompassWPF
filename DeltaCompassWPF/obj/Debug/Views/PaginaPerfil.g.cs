﻿#pragma checksum "..\..\..\Views\PaginaPerfil.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CEB7CAC0D68D8C977100D137501F3D8CB32D3843BE1059DE1A11801348E6F0D3"
//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

using DeltaCompassWPF.Converters;
using DeltaCompassWPF.ViewModels;
using DeltaCompassWPF.Views;
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


namespace DeltaCompassWPF.Views {
    
    
    /// <summary>
    /// PaginaPerfil
    /// </summary>
    public partial class PaginaPerfil : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 57 "..\..\..\Views\PaginaPerfil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel btnSair;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\Views\PaginaPerfil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridSair;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\Views\PaginaPerfil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnJanelaSlot;
        
        #line default
        #line hidden
        
        
        #line 220 "..\..\..\Views\PaginaPerfil.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border picPerfil;
        
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
            System.Uri resourceLocater = new System.Uri("/DeltaCompassWPF;component/views/paginaperfil.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\PaginaPerfil.xaml"
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
            this.btnSair = ((System.Windows.Controls.StackPanel)(target));
            
            #line 58 "..\..\..\Views\PaginaPerfil.xaml"
            this.btnSair.MouseEnter += new System.Windows.Input.MouseEventHandler(this.btnSair_MouseEnter);
            
            #line default
            #line hidden
            
            #line 58 "..\..\..\Views\PaginaPerfil.xaml"
            this.btnSair.MouseLeave += new System.Windows.Input.MouseEventHandler(this.btnSair_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 2:
            this.gridSair = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.btnJanelaSlot = ((System.Windows.Controls.Button)(target));
            
            #line 112 "..\..\..\Views\PaginaPerfil.xaml"
            this.btnJanelaSlot.Click += new System.Windows.RoutedEventHandler(this.BtnJanelaSlot_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.picPerfil = ((System.Windows.Controls.Border)(target));
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
            case 3:
            
            #line 78 "..\..\..\Views\PaginaPerfil.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ItemsControl_MouseDown);
            
            #line default
            #line hidden
            break;
            case 4:
            
            #line 80 "..\..\..\Views\PaginaPerfil.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSlot_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

