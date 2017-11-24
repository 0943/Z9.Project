/*
 * This demo's code syntax use for C# 6.0, can be use in Visual Studio 2015 or 2017.
 */

using System.Windows;
using System.Windows.Input;
using Z9.Mvvm;
using Z9.Mvvm.Command;

namespace WpfTemp
{
    class MainViewModel : NotificationObject
    {
        #region Property

        // Original
        int prop1;
        public int Prop1
        {
            get { return prop1; }
            set { prop1 = value; OnPropertyChanged(nameof(Prop1)); }
        }

        // Complex
        int prop2;
        public int Prop2
        {
            get { return prop2; }
            set { prop2 = value; OnPropertyChanged(); } 
            // OnPropertyChanged method can automatically get the property name.
        }

        // Simple
        string prop3;
        public string Prop3
        {
            get { return prop3; }
            set { Set(ref prop3, value); }  
            // Set method can automatically get the property name, too.
        }

        // Most simple way (no private member anymore).
        public double Prop4
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Command

        // Normal command
        public ICommand Change => new RelayCommand(() =>
        {
            Prop1 = 943;
            Prop2 = 0x3af;
        });

        // Command width parameter
        public ICommand Check => new RelayCommand<string>(
            str => MessageBox.Show(str));

        // Command width parameter and can execution method
        public ICommand Test => new RelayCommand<string>(
            str => { },
            str => true
        );

        #endregion

        protected override void OnInitializeInDesignMode()
            => Prop3 = $"This is in design mode, you can see this sentence in your xaml designer when a control binding on this property.";

        protected override void OnInitializeInRuntime()
            => Prop3 = $"This is in run-time mode, you can see this sentence in running window when a control binding on this property.";

        // No need use constructor any more
        //public MainViewModel()
        //{

        //}
    }
}
