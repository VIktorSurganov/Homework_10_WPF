﻿#pragma checksum "..\..\..\..\Windows\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "643F3327DE683F130DDA2059C45E1DDFAD1879C3"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using WPF_SomeName200_bot;


namespace WPF_SomeName200_bot {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SendMsgBtn;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ForMsgLB;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ForInputmsgTBox;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock forIdTB;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SendToAllMsgBtn;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sndMusicButton;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sndPicButton;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sndweatherButton;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock pathToFile;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgToSend;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPF_SomeName200_bot;component/windows/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 15 "..\..\..\..\Windows\MainWindow.xaml"
            ((WPF_SomeName200_bot.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 15 "..\..\..\..\Windows\MainWindow.xaml"
            ((WPF_SomeName200_bot.MainWindow)(target)).Initialized += new System.EventHandler(this.Window_Initialized);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SendMsgBtn = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\Windows\MainWindow.xaml"
            this.SendMsgBtn.Click += new System.Windows.RoutedEventHandler(this.OnSendMsgBtnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ForMsgLB = ((System.Windows.Controls.ListBox)(target));
            
            #line 55 "..\..\..\..\Windows\MainWindow.xaml"
            this.ForMsgLB.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ForMsgLB_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 56 "..\..\..\..\Windows\MainWindow.xaml"
            this.ForMsgLB.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ForMsgLB_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ForInputmsgTBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.forIdTB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.SendToAllMsgBtn = ((System.Windows.Controls.Button)(target));
            
            #line 114 "..\..\..\..\Windows\MainWindow.xaml"
            this.SendToAllMsgBtn.Click += new System.Windows.RoutedEventHandler(this.OnSendMsgToAllBtnClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.sndMusicButton = ((System.Windows.Controls.Button)(target));
            
            #line 133 "..\..\..\..\Windows\MainWindow.xaml"
            this.sndMusicButton.Click += new System.Windows.RoutedEventHandler(this.sndMusicButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.sndPicButton = ((System.Windows.Controls.Button)(target));
            
            #line 139 "..\..\..\..\Windows\MainWindow.xaml"
            this.sndPicButton.Click += new System.Windows.RoutedEventHandler(this.sndPicButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.sndweatherButton = ((System.Windows.Controls.Button)(target));
            
            #line 145 "..\..\..\..\Windows\MainWindow.xaml"
            this.sndweatherButton.Click += new System.Windows.RoutedEventHandler(this.sndweatherButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.pathToFile = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.imgToSend = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

