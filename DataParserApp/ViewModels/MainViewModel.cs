using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;


namespace DataParserApp.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        IRelationProvider fileProvider;

        public ObservableCollection<Relation> Relations { get; set; }

        public bool CanLoadDataFromFile() => fileProvider == null || fileProvider.CanUpdateRelationsTable();

        [Command(CanExecuteMethodName = "CanLoadDataFromFile")]
        public void LoadDataFromFile()
        {
            if (SplashScreenManagerService != null)
            {
                fileProvider = new RelationProviderFromFile(SplashScreenManagerService, DispatcherService, Relations);
                fileProvider.UpdateRelationsTableAsync();
            }
        }

        public MainViewModel()
        {
            Relations = new ObservableCollection<Relation>();
        }

        public ISplashScreenManagerService SplashScreenManagerService
        {
            get { return GetService<ISplashScreenManagerService>(); }
        }

        public IDispatcherService DispatcherService
        {
            get { return GetService<IDispatcherService>(); }
        }
    }
}
