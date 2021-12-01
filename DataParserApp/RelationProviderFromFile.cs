using DevExpress.Mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DataParserApp
{
    public class RelationProviderFromFile : IRelationProvider
    {
        IFileReader fileReader;
        ITextParser textParser;
        BackgroundWorker worker;
        IDispatcherService dispatcherService;
        ISplashScreenManagerService splashScreenManagerService;
        ObservableCollection<Relation> currentRelations;

        public void UpdateRelationsTableAsync()
        {
            InitialLoadView();
            RunBackgroundWorker();
        }

        public bool CanUpdateRelationsTable() => worker == null || !worker.IsBusy;

        public RelationProviderFromFile(ISplashScreenManagerService splashScreenManagerService, IDispatcherService dispatcherService, ObservableCollection<Relation> currentRelations)
        {
            this.dispatcherService = dispatcherService;
            this.splashScreenManagerService = splashScreenManagerService;
            this.currentRelations = currentRelations;
            fileReader = new TextFileReader();
            textParser = new TextParser();
        }

        IEnumerable<Relation> GetRelations(string pathToFile)
        {
            double progressValue = 0;
            var sourceText = fileReader.ReadText(pathToFile, UpdateSplashScreenContent, worker, ref progressValue);
            if (string.IsNullOrEmpty(sourceText))
                return null;
            var relations = textParser.ConvertToRelations(sourceText, UpdateSplashScreenContent, worker, ref progressValue);
            return relations;
        }

        void UpdateRelationsTable(object sender, DoWorkEventArgs e)
        {
            string fileName = OpenFileDialogAndGetFileName();
            var newRelationsValue = GetRelations(fileName);

            if (newRelationsValue != null)
                UpdateRelations(newRelationsValue);

            DisposeResource();
        }

        void DisposeResource()
        {
            dispatcherService.Invoke(() =>
            {
                splashScreenManagerService.Close();
                worker.DoWork -= UpdateRelationsTable;
                worker = null;
            });
        }

        void UpdateRelations(IEnumerable<Relation> newRelationsValue)
        {
            Action<Relation> add = currentRelations.Add;
            Action action = () =>
            {
                currentRelations.Clear();
                foreach (var element in newRelationsValue)
                {
                    currentRelations.Add(element);
                }

            };
            Application.Current.Dispatcher.BeginInvoke(action);
        }

        string OpenFileDialogAndGetFileName()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string fileName = string.Empty;
            if (openFileDialog.ShowDialog() == true)
                fileName = openFileDialog.FileName;
            return fileName;
        }

        void UpdateSplashScreenContent(double progressValue)
        {
            dispatcherService
                .Invoke(() =>
                {
                    splashScreenManagerService.ViewModel.Progress = progressValue;
                    splashScreenManagerService.ViewModel.Status = $"({progressValue}%)";
                });
        }

        void InitialLoadView()
        {
            splashScreenManagerService.ViewModel = new DXSplashScreenViewModel()
            {
                Tag = new DelegateCommand(CancelOperation, CanCancelOperation)
            };
            splashScreenManagerService.Show();
        }

        void RunBackgroundWorker()
        {
            worker = new BackgroundWorker();
            worker.DoWork += UpdateRelationsTable;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();
        }

        bool CanCancelOperation() => worker != null && worker.IsBusy;

        void CancelOperation() => worker.CancelAsync();
    }
}
