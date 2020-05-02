using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuSolver.Gui
{
    /// <summary>
    /// Interaktionslogik für NumberItem.xaml
    /// </summary>
    public partial class NumberItem : UserControl, INotifyPropertyChanged, IDisposable
    {
        public NumberItem()
        {
            InitializeComponent();
            IsEnabledChanged += NumberItem_IsEnabledChanged;
        }

        private void NumberItem_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue as bool? == true)
            {
                Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                Background = new SolidColorBrush(Colors.Gray);
                NumberBox.Background= new SolidColorBrush(Colors.Transparent);
                NumberBox.Foreground= new SolidColorBrush(Colors.White);
            }
        }



        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int), typeof(NumberItem), new PropertyMetadata((s,e)=> {
                (s as NumberItem)?.OnPropertyChanged("Number");
            }));

        public int NumberLength
        {
            get { return (int)GetValue(NumberLengthProperty); }
            set { SetValue(NumberLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberLengthProperty =
            DependencyProperty.Register("NumberLength", typeof(int), typeof(NumberItem), new PropertyMetadata(0));


        public int[] PossibleNumbers
        {
            get { return (int[])GetValue(PossibleNumbersProperty); }
            set { SetValue(PossibleNumbersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PossibleNumbers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PossibleNumbersProperty =
            DependencyProperty.Register("PossibleNumbers", typeof(int[]), typeof(NumberItem), new PropertyMetadata((s,e)=>
            { }));

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            IsEnabledChanged -= NumberItem_IsEnabledChanged;
        }
    }
}
