using Microsoft.Win32;
using Spproject.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Spproject
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand FromCommand { get; set; }
        public ICommand ToCommand { get; set; }
        public ICommand SuspendCommand { get; set; }
        public ICommand ResumeCommand { get; set; }
        public ICommand AbortCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public CancellationTokenSource cancelEvent { get; set; }
        //
        private int currentProgress;
        private bool current;
        private CancellationTokenSource cts;

        public MainViewModel()
        {
            FromCommand = new RelayCommand(Fromfile);
            ToCommand = new RelayCommand(Tofile);
            CopyCommand = new RelayCommand(Copyexecute);
            cancelEvent = new CancellationTokenSource();
            cts = new CancellationTokenSource();
            SuspendCommand = new RelayCommand(Suspend)
;            ResumeCommand = new RelayCommand(Resume);
;            AbortCommand = new RelayCommand(Abort);
        }

        private void Resume(object? obj)
        {
            current = true;
        }

        private void Suspend(object? obj)
        {
            current = false;
        }

        private void Abort(object? obj)
        {
            cancelEvent.Cancel();
        }





        private void Copyexecute(object? obj)
        {
            try
            {
                Thread thread = new Thread(() =>
                {

                  
                        var fr = File.ReadAllText(Fromtext);
                    MessageBox.Show($"{Fromtext} readed ");
                    for (int i = 0; i < 20; i++)
                    {
                        CurrentProgress += 1;
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(1500);

                    var result = MessageBox.Show("Do you want abort", "confirm", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        Resume(result);
                    }

                    if (cancelEvent.IsCancellationRequested)
                    {
                        Thread.Sleep(500);
                        MessageBox.Show("Aborted , not copied");
                        return;
                    }
                    Thread.Sleep(1000);
                    var result2 = MessageBox.Show("Do you want suspend", "confirm", MessageBoxButton.YesNo);
                    if (result2 == MessageBoxResult.Yes)
                    {
                        Suspend(result2);
                    }
                    while (true)
                    {
                        if (current)
                        {
                            File.WriteAllText(Totext, fr);
                            break;
                        }

                    }
                   
                  

                    for (int i = 0; i < 100; i++)
                    {
                        CurrentProgress += 1;
                        Thread.Sleep(50);

                    }

                    MessageBox.Show("Copied");
                    CurrentProgress = 0;
                    Fromtext = string.Empty;
                    Totext = string.Empty;
                });

                thread.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        public int CurrentProgress
        {
            get { return this.currentProgress; }
            private set
            {
                currentProgress = value; OnPropertyChanged();
            }
        }




        private void Tofile(object? obj)
        {
            Totext = SelectFile();

        }

        private void Fromfile(object? obj)
        {
            Fromtext = SelectFile();
        }

        string SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = " (*.txt)|*.txt|  (*.*)|*.*";

            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Title = "Bir File Seçin";


            if (openFileDialog.ShowDialog() == true)
            {

                string selectedFilePath = openFileDialog.FileName;
                return selectedFilePath;
            }
            return null;
        }

        //

        private string totext { get; set; }
        public string Totext
        {
            get { return totext; }
            set { totext = value; OnPropertyChanged(); }
        }
        private string fromtext { get; set; }
        public string Fromtext
        {
            get { return fromtext; }
            set { fromtext = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
