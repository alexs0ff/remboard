using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// Главная модель 
    /// </summary>
    public class MainModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Происходит при изменении одного из свойств.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public void RisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _currentFile;

        /// <summary>
        /// Содержит название текущего файла.
        /// </summary>
        public string CurrentFile
        {
            get { return _currentFile; }
            set
            {
                if (value != _currentFile)
                {
                    _currentFile = value;
                    RisePropertyChanged("CurrentFile");
                } //if
            }
        }

        private string _description;

        /// <summary>
        /// Задает или получает описание сообщения.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    RisePropertyChanged("Description");
                } //if
            }
        }

        private const double EPSILON = 0.01;

        private double _totalPercent;

        /// <summary>
        /// Общий прогресс.
        /// </summary>
        public double TotalPercent
        {
            get { return _totalPercent; }
            set
            {
                if (Math.Abs(value - _totalPercent) > EPSILON)
                {
                    _totalPercent = value;
                    RisePropertyChanged("TotalPercent");
                } //if
            }
        }

        private bool _hasError;

        /// <summary>
        /// Задает или получает признак наличия ошибки.
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }
            set
            {
                if (value != _hasError)
                {
                    _hasError = value;
                    RisePropertyChanged("HasError");
                } //if
            }
        }

        private double _currentPercent;

        /// <summary>
        /// Текущий процент выполнения на скачиваемом файле.
        /// </summary>
        public double CurrentPercent
        {
            get { return _currentPercent; }
            set
            {
                if (Math.Abs(value - _currentPercent) > EPSILON)
                {
                    _currentPercent = value;
                    RisePropertyChanged("CurrentPercent");
                } //if
            }
        }

        private double _currentTransaferRate;

        /// <summary>
        /// Задает или получает текущую 
        /// </summary>
        public double CurrentTransaferRate
        {
            get { return _currentTransaferRate; }
            set
            {
                if (Math.Abs(value - _currentTransaferRate) > EPSILON)
                {
                    _currentTransaferRate = value;
                    RisePropertyChanged("CurrentTransaferRate");
                } //if
            }
        }

        private int _currentBytesRead;

        /// <summary>
        /// Задает или получает текущее значение полученных байтов.
        /// </summary>
        public int CurrentBytesRead
        {
            get { return _currentBytesRead; }
            set
            {
                if (value != _currentBytesRead)
                {
                    _currentBytesRead = value;
                    RisePropertyChanged("CurrentBytesRead");
                } //if
            }
        }
    }
}
