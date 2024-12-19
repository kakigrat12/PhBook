namespace PhoneBook;

public static class Constants
{
    public static class ChildTables
    {
        public static class Names
        {
            public const string Name = "first_names";
            public const string Field = "name";
            public const string FieldInMain = "name_id";
        }
        
        public static class Otch
        {
            public const string Name = "otchs";
            public const string Field = "otch";
            public const string FieldInMain = "otch_id";
        }
        
        public static class Surnames
        {
            public const string Name = "surnames";
            public const string Field = "surname";
            public const string FieldInMain = "surname_id";
        }
        
        public static class Streets
        {
            public const string Name = "streets";
            public const string Field = "street";
            public const string FieldInMain = "surname_id";
        }

        public static string GetField(string tableName)
            => tableName switch
            {
                Names.Name => Names.Field,
                Otch.Name => Otch.Field,
                Surnames.Name => Surnames.Field,
                Streets.Name => Streets.Field,
                _ => throw new ArgumentException($"Invalid table name: {tableName}")
            };
        
        public static string GetFieldInMain(string tableName)
            => tableName switch
            {
                Names.Name => Names.FieldInMain,
                Otch.Name => Otch.FieldInMain,
                Surnames.Name => Surnames.FieldInMain,
                Streets.Name => Streets.FieldInMain,
                _ => throw new ArgumentException($"Invalid table name: {tableName}")
            };
        
        public static bool TableExists(string tableName)
            => tableName == Names.Name ||
               tableName == Otch.Name ||
               tableName == Surnames.Name ||
               tableName == Streets.Name;
        
        public static IEnumerable<string> GetTableNames()
        {
            yield return Names.Name;
            yield return Otch.Name;
            yield return Surnames.Name;
            yield return Streets.Name;
        }

    }
}