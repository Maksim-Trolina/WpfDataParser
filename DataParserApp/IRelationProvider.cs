namespace DataParserApp
{
    public interface IRelationProvider
    {
        bool CanUpdateRelationsTable();

        void UpdateRelationsTableAsync();
    }
}
